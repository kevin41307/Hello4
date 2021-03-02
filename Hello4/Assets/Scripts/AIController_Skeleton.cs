using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController_Skeleton : AIController_Base
{
    Animator animator;
    NavMeshAgent agent;
    Transform player;
    Transform targetPlayer;
    Transform nearByPlayer;
    RagDoll ragdoll;

    const float soundRangeDefault = 5f;
    const float sightRangeDefault = 10f;
    const float attackIntervalDefault = 3f;
    const float disappearTime = 10f;
    const float markTime = 10f;
    const float sensorInterval = 1f;
    const float runSpeedDefault = 1.4f;

    float attackInterval = 3f;
    float soundRange;
    float sightRange;
    float sightAngle = 60f;
    float senseTimer = 0f;
    float wanderScope = 5f;
    float wanderSpeed = 1f;
    float seekDistance = 5f;
    float stopTime = 0f;
    float runSpeed = 1.4f;
    float attackRange = 1.7f;
    float attackFieldOfView = 140f;

    float attackTimer = 0f;
    float disappearTimer = 0f;
    float markTimer = 0f;
    bool disappeared = false;
    bool isInteracting = false;


    //事件管理
    public event System.EventHandler<System.EventArgs> OnEnemyDeath;
    public enum FSMState
    {
        Wander,
        Seek,
        Chase,
        Attack,
        Skill,
        Damaged,
        Dead
    }
    FSMState currentState;

    EnemyBattle enemyBattle;
    SkeletonAttribute enemyAttribute;
    SkeletonAnimations enemyAnimations;
    public GameObject colliderGo;
    CapsuleCollider enemyCollider;
    EnemyHpBar enemyHpBar;
    private void Awake()
    {
        enemyBattle = GetComponent<EnemyBattle>();
        enemyAttribute = GetComponent<SkeletonAttribute>();
        agent = GetComponent<NavMeshAgent>();
        player = Game.playerAttrSingle.player.transform;
        animator = GetComponent<Animator>();
        enemyAnimations = GetComponent<SkeletonAnimations>();
        enemyCollider = colliderGo.GetComponent<CapsuleCollider>();
        enemyHpBar = GetComponentInChildren<EnemyHpBar>();
        attackTimer = attackInterval;
        ragdoll = GetComponent<RagDoll>();

        agent.baseOffset = -0.06666655f;
    }

    private void Start()
    {
        
    }
    public void Reset()
    {
        targetPlayer = null;
        nearByPlayer = null;
        markTimer = 0;
        attackInterval = attackIntervalDefault;
        attackTimer = attackInterval;
        disappearTimer = 0;
        disappeared = false;
        runSpeed = runSpeedDefault;
        //agent.isStopped = false;
        agent.enabled = true;
        enemyCollider.enabled = true;
        enemyHpBar.Appear();
        enemyAttribute.Reset();
        //Debug.Log(enemyAttribute.firstInDead + " " + enemyAttribute.firstInBurst + " " + enemyAttribute.isAlive );
        SetMaxAgentSpeed(wanderSpeed);
        animator.SetFloat("animationSpeed", 1f);
        StateChange(FSMState.Wander);
        ragdoll.DoRagdoll(false);

        animator.Rebind();
        enemyAnimations.Reset();
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            animator.Rebind();
        }


        if (enemyAnimations.currentState != SkeletonAnimations.AniState.Dead)
        {
            if (senseTimer >= sensorInterval)
            {
                senseTimer = 0;
                SenseNearbyPlayer();
            }
            senseTimer += Time.deltaTime;
            if (attackTimer <= attackInterval) attackTimer += Time.deltaTime;
            if (markTimer <= markTime)
            {
                markTimer += Time.deltaTime;
            }
            else
            {
                SenseNearbyPlayer();
                if(nearByPlayer == null)
                {
                    targetPlayer = null;
                    //Debug.Log("targetplayernull");
                }
            }

            //Debug.Log("attackTimerr"+ attackTimer);
            CheckIsAlive();
            
            //Debug.Log("zombieAni" + enemyAnimations.currentState);
            Debug.DrawLine(transform.position, agent.destination, Color.green);
        }
        FSMUpdate();

    }

    void FixBaseOffset()
    {
        if (Mathf.Abs(transform.position.y) > 0.01f)
        {
            agent.baseOffset = -Mathf.Abs(transform.position.y);
        }
    }

    public Transform GetNearbyPlayer()
    {
        return nearByPlayer;
    }
    void SenseNearbyPlayer()
    {
        float distance = Vector3.Distance(player.position, this.transform.position);
        soundRange = (currentState == FSMState.Chase || currentState == FSMState.Attack) ? soundRangeDefault * 1.5f: soundRangeDefault;
        sightRange = (currentState == FSMState.Chase || currentState == FSMState.Attack) ? sightRangeDefault * 1.5f : sightRangeDefault;

        if (distance < soundRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            Debug.DrawLine(player.position, this.transform.position, Color.green);
            nearByPlayer = player.transform;
            targetPlayer = nearByPlayer;
            markTimer = 0;
        }
        else
        {
            nearByPlayer = null;

        }
        
        if (distance < sightRange)
        {
            Vector3 direction = player.transform.position - this.transform.position;
            float degree = Vector3.Angle(direction, this.transform.forward);

            if (degree < sightAngle / 2 && degree > -sightAngle / 2)
            {
                Ray ray = new Ray();
                ray.origin = this.transform.position;
                ray.direction = direction;
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, sightRange))
                {
                    if (hitInfo.transform == player.transform)
                    {
                        nearByPlayer = player.transform;
                        targetPlayer = nearByPlayer;
                        markTimer = 0;
                    }
                    else
                    {
                        nearByPlayer = null;
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
            case FSMState.Skill:
                UpdateSkillState();
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
        enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
        //targetPlayer = GetNearbyPlayer();
        if (targetPlayer != null)
        {
            StateChange(FSMState.Chase);
            agent.ResetPath();
            return;
        }

        //如果受到伤害，那么进入搜索状态
        if (enemyBattle.getDamaged)
        {
            StateChange(FSMState.Seek);
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
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
        //限制游荡的速度
        SetMaxAgentSpeed(wanderSpeed);
        //统计僵尸在当前位置附近的停留时间
        CaculateStopTime();

    }

    void UpdateSeekState()
    {
        enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
        //如果僵尸感知范围内有玩家，进入追踪状态
        //targetPlayer = GetNearbyPlayer();
        if (targetPlayer != null)
        {
            StateChange(FSMState.Chase);
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
            StateChange(FSMState.Wander);
            agent.ResetPath();
            return;
        }
        //减速度限制为奔跑速度
        SetMaxAgentSpeed(runSpeed);

    }

    void UpdateChaseState()
    {
        enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Chase);
        
        //如果僵尸感知范围内没有玩家，进入游荡状态
        //targetPlayer = GetNearbyPlayer();
        if (targetPlayer == null)
        {
            StateChange(FSMState.Wander);
            agent.ResetPath();
            return;
        }
        //如果玩家与僵尸的距离，小于僵尸的攻击距离，那么进入攻击状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) <= attackRange)
        {
            StateChange(FSMState.Attack);
            agent.ResetPath();
            return;
        }

        //设置移动目标为玩家
        agent.SetDestination(targetPlayer.position);
        SetMaxAgentSpeed(runSpeed);
    }
    void UpdateAttackState()
    {
        if (enemyAnimations.currentState == SkeletonAnimations.AniState.Attack)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        //如果僵尸感知范围内没有玩家，进入游荡状态
        //targetPlayer = GetNearbyPlayer();
        if (targetPlayer == null)
        {          
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
            StateChange(FSMState.Wander);
            return;
        }
        //如果玩家与僵尸的距离，大于僵尸的攻击距离，那么进入追踪状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
        {
           
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Chase);
            StateChange(FSMState.Chase);
            return;
        }

        if(!enemyAttribute.firstInBurst && enemyHpBar.hpImage.fillAmount < 0.5f)
        {
            BurstMode();
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Skill);
            StateChange(FSMState.Skill);
            return;

        }

        //计算僵尸的正前方和玩家的夹角，只有玩家在僵尸前方才能攻击
        Vector3 direction = targetPlayer.position - this.transform.position;
        float degree = Vector3.Angle(direction, this.transform.forward);
        if (degree < attackFieldOfView / 2 && degree > -attackFieldOfView / 2)
        {
            //animator.SetBool("isAttack", true);

            //Debug.Log("attackTimer" + attackTimer);
            
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0;
                enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Attack);
                /*
                if (enemyAttribute.attackAudio != null)
                    AudioSource.PlayClipAtPoint(enemyAttribute.attackAudio, this.transform.position);
                */
                //ph.TakeDamage(attackDamage);

            }
            else if(enemyAnimations.currentState != SkeletonAnimations.AniState.Attack )
            {
                enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Stand);
            }   
        }
        else
        {
            //如果玩家不在僵尸前方，僵尸需要转向后才能攻击
            //animator.SetBool("isAttack", false);
            //agent.isStopped = true;
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Chase);

            Vector3 directionN = direction.normalized;
            directionN.y = 0;
            float rs = 3f;
            Quaternion tr = Quaternion.LookRotation(direction);
            Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
            this.transform.rotation = targetRotation;
            //agent.isStopped = false;
        }

    }

    void UpdateSkillState()
    {
        if (enemyAnimations.currentState == SkeletonAnimations.AniState.Skill)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        //如果僵尸感知范围内没有玩家，进入游荡状态
        if (targetPlayer == null)
        {
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
            StateChange(FSMState.Wander);
            return;
        }
        //如果玩家与僵尸的距离，大于僵尸的攻击距离，那么进入追踪状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
        {

            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Chase);
            StateChange(FSMState.Chase);
            return;
        }
        //如果玩家与僵尸的距离，小于僵尸的攻击距离，那么进入攻击状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) <= attackRange)
        {
            StateChange(FSMState.Attack);
            agent.ResetPath();
            return;
        }
    }
    void UpdateDamagedState()
    {
        if (enemyAnimations.currentState == SkeletonAnimations.AniState.Damaged)
        {
            return;
        }

        if (!enemyAttribute.firstInBurst && enemyHpBar.hpImage.fillAmount < 0.5f)
        {
            BurstMode();
            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Skill);
            StateChange(FSMState.Skill);
            return;
        }

        //如果玩家与僵尸的距离，小于僵尸的攻击距离，那么进入攻击状态
        if (Vector3.Distance(targetPlayer.position, this.transform.position) <= attackRange)
        {
            StateChange(FSMState.Attack);
            agent.ResetPath();
            return;
        }

        if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
        {

            agent.ResetPath();
            //animator.SetBool("isAttack", false);
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Chase);
            StateChange(FSMState.Chase);
            return;
        }

        if (enemyAnimations.currentState != SkeletonAnimations.AniState.Damaged )
        {
            enemyBattle.hitRecover = false;
            enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Walk);
            StateChange(FSMState.Seek);
            agent.ResetPath();
            return;
        }
    }

    void UpdateDeadState()
    {
        //Debug.Log("disappearTimer" + disappearTimer);
        enemyAnimations.ChangeAnimation(SkeletonAnimations.AniState.Dead);
        //如果僵尸初次进入死亡状态，那么需要禁用僵尸的一些组件(导航代理组件和碰撞体组件)
        if (enemyAttribute.firstInDead)
        {
            //Debug.Log("disappeared0" + disappeared);
            enemyAttribute.firstInDead = false;

            agent.ResetPath();
            agent.isStopped = true;
            agent.enabled = false;
            ragdoll.DoRagdoll(true);

            QuestManager.UpdateEliminateQuest(new QuestEventArgs(enemyAttribute.enemyName));
            this.SendMessage("BurstVFX_Off", SendMessageOptions.DontRequireReceiver);

            enemyCollider.enabled = false;
            enemyHpBar.Disappear();
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if( playerMovement != null)
            {
                if (playerMovement.lookAtTarget != null)
                {
                    if (playerMovement.lookAtTarget.root == this.transform.root)
                    {
                        playerMovement.OnClearLookOverride();
                    }                   
                }
            }

            //播放死亡动画
            //animator.applyRootMotion = true;
            //animator.SetTrigger("toDie");
            //开始统计死亡时间
        }

        //统计僵尸死亡后经过的时间，如果超过了尸体消失时间，那么禁用该僵尸对象
        if (!disappeared)
        {
            //Debug.Log("disappeared1" + disappeared);
            if (disappearTimer > disappearTime)
            {
                //CachedObjectIDCard cachedObjectIDCard = GetComponent<CachedObjectIDCard>();
                if( co != null )
                {
                    Game.cacheObjectControllerSingle.GiveBackAsset(co);

                    //Debug.Log(cachedObjectIDCard.objType.ToString() + cachedObjectIDCard.ID + " giveback!");
                }
                else
                {
                    this.gameObject.SetActive(false);
                }  
                disappeared = true;
                Reset();

            }
            //消失计时器不断统计时间
            disappearTimer += Time.deltaTime;
        }
    }

    void BurstMode()
    {
        enemyAttribute.firstInBurst = true;
        runSpeed = runSpeedDefault * 1.35f;
        attackInterval = attackIntervalDefault * 0.7f;
        animator.SetFloat("animationSpeed", 1.35f);

    }


    void HitRecover()
    {
        if (enemyBattle.hitRecover)
            StateChange(FSMState.Damaged);
    }

    void CheckIsAlive()
    {
        //如果僵尸处于非死亡状态，但是生命值减为0，那么进入死亡状态
        if (enemyAnimations.currentState != SkeletonAnimations.AniState.Dead && !enemyAttribute.isAlive)
        {
            StateChange(FSMState.Dead);
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
        if (nextState == currentState) return;
        currentState = nextState;
        Debug.Log("FSMState" + nextState);
    }

    public void ResetAnimator()
    {
        animator.Rebind();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, soundRange);
        Vector3 direction = transform.forward * sightRange;
        Gizmos.DrawLine(transform.position, transform.position + direction);

    }





}
