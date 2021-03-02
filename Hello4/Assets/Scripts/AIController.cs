using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyAttribute), typeof(EnemyBattle), typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AIController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform target;
    Transform player;
    Transform targetPlayer;
    Transform nearByPlayer;
    float soundRange = 5f;
    float sightRange = 10f;
    float sightAngle = 60f;
    float senseTimer = 0f;
    float sensorInterval = 1f;
    float wanderScope = 5f;
    float wanderSpeed = 1f;
    float seekDistance = 5f;
    float stopTime = 0f;
    float runSpeed = 6f;
    float attackRange = 1.02f;
    float attackFieldOfView = 160f;
    float attackInterval = 1f;
    float disappearTime = 10f;

    FSMState currentState;
    
    EnemyBattle enemyBattle;
    EnemyAttribute enemyAttribute;
    ZombieAnimation zombieAnimation;
    
    public enum FSMState
    {
        Wander,
        Seek,
        Chase,
        Attack,
        Damaged,
        Dead
    }
    private void Awake()
    {
        enemyBattle = GetComponent<EnemyBattle>();
        enemyAttribute = GetComponent<EnemyAttribute>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        zombieAnimation = GetComponent<ZombieAnimation>();
    }


    // Update is called once per frame
    void Update()
    {

        if ( zombieAnimation.currentState != ZombieAnimation.State.Dead)
        {
            if (senseTimer >= sensorInterval)
            {
                senseTimer = 0;
                SenseNearbyPlayer();
            }
            senseTimer += Time.deltaTime;
            CheckIsAlive();
            FSMUpdate();
            

            //Debug.Log("zombieAni" + zombieAnimation.currentState);
            Debug.DrawLine(transform.position, agent.destination, Color.green);
        }


    }

    public Transform GetNearbyPlayer()
    {
        return nearByPlayer;
    }
    void SenseNearbyPlayer()
    {
        float distance = Vector3.Distance(player.position, this.transform.position);
        if (distance < soundRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            Debug.DrawLine(player.position, this.transform.position, Color.green);
            nearByPlayer = player.transform;
        }

        if (distance < sightRange )
        {
            Vector3 direction = player.transform.position - this.transform.position;
            float degree = Vector3.Angle(direction, this.transform.forward);

            if( degree < sightAngle/2 && degree > -sightAngle/2)
            {
                Ray ray = new Ray();
                ray.origin = this.transform.position;
                ray.direction = direction;
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, sightRange))
                {
                    if (hitInfo.transform == player.transform)
                    {
                        nearByPlayer = player.transform;
                    }
                }
            }
        }
    }

    void FSMUpdate()
    {
        switch (currentState)
        {
            case FSMState.Wander:
                UpdateWanderState();
                break;
            case FSMState.Seek:
                UpdateSeekState();
                break;
            case FSMState.Chase:
                UpdateChaseState();
                break;
            case FSMState.Attack:
                UpdateAttackState();
                break;
            case FSMState.Damaged:
                UpdateDamagedState();
                break;
            case FSMState.Dead:
                UpdateDeadState();
                break;

        }
    }

    void UpdateWanderState()
    {
        zombieAnimation.ChangeAnimation(ZombieAnimation.State.Walk);
        targetPlayer = GetNearbyPlayer();
        if (targetPlayer != null)
        {
            currentState = FSMState.Chase;
            agent.ResetPath();
            return;
        }

        //如果受到伤害，那么进入搜索状态
        if (enemyBattle.getDamaged)
        {
            currentState = FSMState.Seek;
            agent.ResetPath();
            return;
        }
        //如果没有目标位置，那么随机选择一个目标位置
        if (AgentDone())
        {  //判断僵尸是否到达上一次随机游荡的目的地
            Vector3 randomRange = new Vector3((Random.value - 0.5f) * 2 * wanderScope,
                                            0, (Random.value - 0.5f) * 2 * wanderScope);
            Vector3 nextDestination = this.transform.position + randomRange;
            agent.destination = nextDestination;
        }

        if (agent.destination != Vector3.zero)
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Walk);
        //限制游荡的速度
        SetMaxAgentSpeed(wanderSpeed);
        //统计僵尸在当前位置附近的停留时间
        CaculateStopTime();
        
    }

    void UpdateSeekState()
    {
        zombieAnimation.ChangeAnimation(ZombieAnimation.State.Walk);
        //如果僵尸感知范围内有玩家，进入追踪状态
        targetPlayer = GetNearbyPlayer();
        if (targetPlayer != null)
        {
            currentState = FSMState.Chase;
            agent.ResetPath();
            return;
        }
        //如果僵尸受到攻击，那么向着玩家开枪时所在的方向进行搜索
        if (enemyBattle.getDamaged)
        {
            Vector3 seekDirection = enemyBattle.damagedDirection;
            agent.destination = this.transform.position
                + seekDirection * seekDistance;
            //将getDamaged设置为false，表示已经处理了这次攻击
            enemyBattle.getDamaged = false;
        }

        //如果到达搜索目标，或者卡在某个地方无法到达目标位置，那么回到游荡状态
        if (AgentDone() || stopTime > 1.0f)
        {
            currentState = FSMState.Wander;
            agent.ResetPath();
            return;
        }
        //减速度限制为奔跑速度
        SetMaxAgentSpeed(runSpeed );

    }

    void UpdateChaseState()
    {
        zombieAnimation.ChangeAnimation(ZombieAnimation.State.Chase);

        //如果僵尸感知范围内没有玩家，进入游荡状态
        targetPlayer = GetNearbyPlayer();
        if (targetPlayer == null)
        {
            currentState = FSMState.Wander;
            agent.ResetPath();
            return;
        }
        //如果玩家与僵尸的距离，小于僵尸的攻击距离，那么进入攻击状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) <= attackRange)
        {
            currentState = FSMState.Attack;
            agent.ResetPath();
            return;
        }

        //设置移动目标为玩家
        agent.SetDestination(targetPlayer.position);
    }
    void UpdateAttackState()
    {
        
        //如果僵尸感知范围内没有玩家，进入游荡状态
        targetPlayer = GetNearbyPlayer();
        if (targetPlayer == null)
        {
            currentState = FSMState.Wander;
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Walk);
            return;
        }
        //如果玩家与僵尸的距离，大于僵尸的攻击距离，那么进入追踪状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
        {
            currentState = FSMState.Chase;
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Chase);
            return;
        }
                              
        //计算僵尸的正前方和玩家的夹角，只有玩家在僵尸前方才能攻击
        Vector3 direction = targetPlayer.position - this.transform.position;
        float degree = Vector3.Angle(direction, this.transform.forward);
        float attackTimer = 0f;
        if (degree < attackFieldOfView / 2 && degree > -attackFieldOfView / 2)
        {
            //animator.SetBool("isAttack", true);
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Attack);
            if (attackTimer > attackInterval)
            {
                attackTimer = 0;
                if ( enemyAttribute.attackAudio != null)
                    AudioSource.PlayClipAtPoint(enemyAttribute.attackAudio, this.transform.position);
                
                //ph.TakeDamage(attackDamage);
            }
            attackTimer += Time.deltaTime;
        }
        else
        {
            //如果玩家不在僵尸前方，僵尸需要转向后才能攻击
            //animator.SetBool("isAttack", false);

            //agent.isStopped = true;
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Chase);
            Vector3 directionN = direction.normalized;
            directionN.y = 0;
            float rs = 50f;
            Quaternion tr = Quaternion.LookRotation(direction);
            Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
            this.transform.rotation = targetRotation;
            //agent.isStopped = false;
        }

    }

    void UpdateDamagedState()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            enemyBattle.hitRecover = false;
            zombieAnimation.ChangeAnimation(ZombieAnimation.State.Walk);
            currentState = FSMState.Seek;
            agent.ResetPath();
        }
    }

    void UpdateDeadState()
    {
        zombieAnimation.ChangeAnimation(ZombieAnimation.State.Dead);
        //如果僵尸初次进入死亡状态，那么需要禁用僵尸的一些组件(导航代理组件和碰撞体组件)
        if (enemyAttribute.firstInDead)
        {
            enemyAttribute.firstInDead = false;

            agent.ResetPath();
            agent.isStopped =true;
            agent.enabled = false;

            GetComponent<CapsuleCollider>().enabled = false;
            //播放死亡动画
            //animator.applyRootMotion = true;
            //animator.SetTrigger("toDie");
            //开始统计死亡时间
            float disappearTimer = 0;
            bool disappeared = false;
            //统计僵尸死亡后经过的时间，如果超过了尸体消失时间，那么禁用该僵尸对象
            if (!disappeared)
            {

                if (disappearTimer > disappearTime)
                {
                    this.gameObject.SetActive(false);
                    disappeared = true;
                }
                //消失计时器不断统计时间
                disappearTimer += Time.deltaTime;
            }
        }
    }

    void HitRecover()
    {
        if(enemyBattle.hitRecover)
             currentState = FSMState.Damaged;
    }

    void CheckIsAlive()
    {
        //如果僵尸处于非死亡状态，但是生命值减为0，那么进入死亡状态
        if (zombieAnimation.currentState != ZombieAnimation.State.Dead && !enemyAttribute.isAlive)
        {
            currentState = FSMState.Dead;
        }
    }
    void CaculateStopTime()
    {
        // 如果在一个地方停留太久（各种原因导致僵尸卡住）
        // 那么选择僵尸背后的一个位置当做下一个目标
        if (stopTime > 1.0f)
        {
            Vector3 nextDestination = this.transform.position
                - this.transform.forward * (Random.value) * wanderScope;
            agent.destination = nextDestination;
        }
    }
    //判断僵尸是否在一次导航中到达了目的地
    bool AgentDone()
    {
        //有两个条件，一个是导航代理的 pathPending 属性变为 false。另一个条件是导航代理与目的地的剩余距离 remainingDistance 小于其停止距离 stoppingDistance。
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
    void SetMaxAgentSpeed(float wanderSpeed)
    {
        agent.speed = wanderSpeed;
    }

    public void StateChange(FSMState nextState)
    {
        currentState = nextState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, soundRange);
        Vector3 direction = transform.forward * sightRange;
        Gizmos.DrawLine(transform.position, transform.position + direction );

    }
}
