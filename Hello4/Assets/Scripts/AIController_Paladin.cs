using System.Collections;
using UnityEngine;


public class AIController_Paladin : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo animatorState;
    Rigidbody rb;
    Transform target;
    Transform player;
    Transform targetPlayer;
    Transform nearByPlayer;
    float soundRange = 10f;
    float sightRange = 10f;
    float sightAngle = 60f;
    float senseTimer = 0f;
    float sensorInterval = 1f;
    float wanderScope = 5f;
    float wanderSpeed = 1f;
    float seekDistance = 5f;
    float stopTime = 0f;
    float walkSpeed = 2.5f;
    float runSpeed = 10f; //angry = 16f

    int attackCount = 0;
    int attackCountMax = 3;
    bool attackMode = false;
    float attackRange = 1.65f;
    float longAttackRange = 5f;
    float attackFieldOfView = 160f;
    float attackInterval = 1f;
    bool observeMode = false;
    float observeTimer = 0f;
    float observeSubTimer = 0f;
    const float observeTime = 5f;
    float observeScope = 30f;
    float observeSpeed = 1f;
    bool repeatThisMode = false;
    
    bool observingTarget = false;
    float rotateSpeed = 4f;
    float stopDistance = 1.33f;
    float disappearTime = 10f;
    bool isAttacking = false;
    float gravity = -12f;
    float targetSpeed;
    float speedSmoothVelocity;
    bool close = false;
    bool isRelaxingBody = false;
    bool isRelaxingBodyFast = false;
    bool isClosingTarget = false;
    bool isClosingTargetRun = false;
    bool wasHitted= false;
    FSMState currentState;

    PaladinBattle paladinBattle;
    EnemyAttribute enemyAttribute;
    PaladinAnimation paladinAnimation;
    InteractiveWithPlayer interactiveWithPlayer;
    CharacterController cc;
    AIComboSystem combo;

    //區域變數外移
    bool attacked = false;
    string[] comboNames;
    const float secsIn60fps = 0.016667f;
    float durationH;
    float forceH;
    float durationV;
    float forceV;
    float stopForceH;
    float speed;
    float distance;
    float speedPercent = 1f;
    bool isAttackSupport = false;
    bool watchdoging = false;
    
    float sign ;
    Vector3 velocity = Vector3.zero;
    Vector2 movement = Vector2.zero;
    Vector3 direction;
    int playerDistanceState;


    public enum FSMState
    {
        Wander,
        Attack,
        Observe,
        Dead,
        Test
    }
    private void Awake()
    {
        paladinBattle = GetComponent<PaladinBattle>();
        enemyAttribute = GetComponent<EnemyAttribute>();
        player = Game.playerAttrSingle.player.transform;
        animator = GetComponent<Animator>();
        paladinAnimation = GetComponent<PaladinAnimation>();
        interactiveWithPlayer = new InteractiveWithPlayer(transform, rotateSpeed, attackRange);
        cc = GetComponent<CharacterController>();
        combo = new AIComboSystem(interactiveWithPlayer);
    }

    private void Start()
    {
        currentState = FSMState.Attack;
        paladinBattle.BattleStart();
        targetPlayer = player;
        animator.SetFloat("lockOn", 1f);
        StopAllCoroutines();
    }
    // Update is called once per frame

    void Update()
    {
        //Debug.Log("Time.deltaTime" + Time.deltaTime);
        FSMUpdate();
        /*
        if (attackMode && observeMode)
        {
            Debug.Log(attackMode + " 45645645646545645645646456464566465 " + observeMode);
        }
        if (!attackMode && !observeMode)
        {
            Debug.Log(attackMode + " 45645645646545645645646456464566465 " + observeMode);
        }
        */

        #region closeTargetTest
        /*
        if( close )
        {
            if (Vector3.Distance(targetPlayer.position, this.transform.position) > 2f)
            {
                Debug.Log("distance" + Vector3.Distance(targetPlayer.position, this.transform.position));
                speed = Mathf.SmoothDamp(speed, 10f, ref speedSmoothVelocity, .1f);
                cc.Move(transform.forward * speed * Time.deltaTime);
                interactiveWithPlayer.LookAtTarget(2.5f * rotateSpeed);
                animator.SetFloat("direction", 1f, 0.1f, Time.deltaTime);
                animator.SetFloat("speedPercent", 1f, 0.1f, Time.deltaTime);

            }
            else
            {
                Debug.Log("distance" + Vector3.Distance(targetPlayer.position, this.transform.position));
                speed = 0f;
                animator.SetFloat("direction", 0f , 0.1f, Time.deltaTime );
                animator.SetFloat("speedPercent", 0f, 0.1f, Time.deltaTime);
                if( animator.GetFloat("direction") < 0.01f)
                {
                    animator.SetFloat("direction", 0f);
                    animator.SetFloat("speedPercent", 0f);
                    close = false;
                }
               
            }

        }

        if (closingTarget)
        {
            if (Vector3.Distance(targetPlayer.position, this.transform.position) >6f)
            {
               
                
                close = true;
            }
        }
        */
        #endregion

        #region Observe Target

        if(observingTarget)
        {
            ObserveTarget();
        }
        #endregion

        #region relexBody
        if(isRelaxingBody)
        {
            RelaxBody();
        }

        if(isRelaxingBodyFast)
        {
            RelaxBodyFast();
        }


        #endregion

        if (isClosingTarget)
        {
            CloseTarget();
        }

        if (isClosingTargetRun)
        {
            CloseTargetRun();
        }

        if(isAttackSupport)
        {
            AttackSupport();
        }
    }

    private void LateUpdate()
    {
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
            //agent.isStopped = false;
            //agent.SetDestination(player.position);
            Debug.DrawLine(player.position, this.transform.position, Color.green);
            nearByPlayer = player.transform;
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
                    }
                }
            }
        }
    }

    void FSMUpdate()
    {
        switch (currentState)
        {
            case FSMState.Observe:
                if (!observeMode && !observeMode)
                {
                    UpdateObserveState();
                }
                break;
            case FSMState.Attack:
                if( !attackMode && !observeMode)
                {
                    UpdateAttackState();
                }               
                break;


            case FSMState.Dead:
                UpdateDeadState();
                break;
            case FSMState.Test:
                UpdateTestState();
                break;

        }
    }
    void UpdateTestState()
    {
        #region closeTarget test
        /*
        if (!isClosingTarget)
        {
            Debug.Log("cc");
            isClosingTarget = true;
        }
        */
        /*
        if (!closingTarget)
        {
            StartCoroutine(CloseTarget());
        }
        */
        /*
        if (!isClosingTargetRun)
        {
            isClosingTargetRun = true;
        }
        */
        #endregion
    }

    #region observe state...
    void UpdateObserveState()
    {
        if( !observeMode )
        {
            StartCoroutine(StartObserveMode());
        }

    }
    IEnumerator StartObserveMode()
    {
        observeMode = true;
        Debug.Log("Start_ObserveState");
        //animator.applyRootMotion = false;

        soundRange = 100f;
        paladinAnimation.ChangeAnimation(PaladinAnimation.State.Observe);
        yield return new WaitForSeconds(1f);

        observeTimer = Random.Range(1f, observeTime) ;
        Debug.Log("observeTimer" + observeTimer);
        sign = interactiveWithPlayer.RandomSign();
        velocity = Vector3.zero;
        movement = Vector2.zero;
        direction = interactiveWithPlayer.PlayerDirectionNormailized();
        playerDistanceState = interactiveWithPlayer.GetCurrenDistanceState();
        velocity = sign * this.transform.right * walkSpeed;
        movement = new Vector2(sign, 0);

        observingTarget = true;
        while (observeTimer >= 0f) //在協程裡面移動會有問題 外移至Update
        {
            yield return new WaitForSeconds(observeTimer);
        }
        observingTarget = false;

        if (!isRelaxingBody)
        {
            isRelaxingBody = true;
        }
        while (isRelaxingBody)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //observingTarget = false;
        ReapeatThisMode(FSMState.Observe, FSMState.Attack);
        //paladinAnimation.ChangeAnimation(PaladinAnimation.State.Attack);
        //animator.applyRootMotion = true;
        observeMode = false;
        Debug.Log("End_ObserveState");
    }

    void ObserveTarget()
    {
        if (isRelaxingBody) isRelaxingBody = false;
        observeTimer -= Time.deltaTime;

        playerDistanceState = interactiveWithPlayer.GetCurrenDistanceState();
        if (playerDistanceState == 5)
        {
            Debug.Log("玩家逃離戰場");
            observeTimer = 0f;
        }
        if (wasHitted)
        {
            observeTimer -= 0.5f;
            Debug.Log("observeTimer" + observeTimer);
            wasHitted = false;
        }

        cc.Move(velocity * Time.deltaTime);
        animator.SetFloat("X", movement.x, 0.1f, Time.deltaTime);
        animator.SetFloat("Y", movement.y, 0.1f, Time.deltaTime);
        interactiveWithPlayer.LookAtTarget(1f);
    }

    #endregion

    void UpdateAttackState()
    {
        if( !attackMode )
        {
            StartCoroutine(AttackModeStart());
        }
    }

    IEnumerator AttackModeStart()
    {
        attackMode = true;
        //agent.isStopped = true;

        targetPlayer = player;
        soundRange = 100f;
        Debug.Log("Start_AttackMode");
        paladinAnimation.ChangeAnimation(PaladinAnimation.State.Attack);
        attackCount = Random.Range(1, attackCountMax);
        attacked = false;
        //Debug.Log("attackCount" + attackCount);

        while (animator.GetBool("isInteracting"))
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if ( !interactiveWithPlayer.PlayerInDistance() ) //遠距離攻擊
        {
            if(Random.Range(0,2f) >= 1f)
            {
                if (!isAttacking)
                {
                    comboNames = null;
                    comboNames = combo.GetSkillNames(1, 5f);
                    StartCoroutine(AttackTarget(comboNames, 5f));
                    isAttacking = true;
                }
                    
                while (isAttacking)
                {
                    yield return new WaitForSeconds(secsIn60fps);
                }
                attacked = true;
            }
        }

        if( attacked == false ) //近距離攻擊
        {
            if (!isAttacking)
            {
                int count = Random.Range(2, 3);
                comboNames = null;
                comboNames = combo.GetSkillNames(count);
                StartCoroutine(AttackTarget(comboNames, attackRange));
                isAttacking = true;  
            }
            
            while (isAttacking)
            {
                yield return new WaitForSeconds(secsIn60fps);
            }
          
        }
        soundRange /= 5 ;
        //currentState = FSMState.Observe;
        ReapeatThisMode(FSMState.Attack, FSMState.Observe);
        attackMode = false;
        Debug.Log("End_AttackMode");
    }
    IEnumerator AttackTarget(string[] comboNames, float _distance)
    {
        isAttacking = true;
        distance = _distance;
        for (int i = 0; i < comboNames.Length; i++)
        {
            Debug.Log("Attack " + comboNames[i]);
            if (comboNames[i].Equals("Walk"))
            {
                isClosingTarget = true;
                while (isClosingTarget)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                isRelaxingBodyFast = true;
                animator.SetBool("isInteracting", false);
                continue;
            }
            if (comboNames[i].Equals("Run"))
            {
                isClosingTargetRun = true;
                while (isClosingTargetRun)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                isRelaxingBodyFast = true;
                animator.SetBool("isInteracting", false);
                continue;
            }

            if (!animator.GetBool("isInteracting"))
            {
                paladinAnimation.Play_Skill(comboNames[i]);
                animator.SetBool("isInteracting", true);
                //yield return new WaitForSeconds(0.1f);
            }

            isAttackSupport = true;
            while(animator.GetBool("isInteracting"))
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            isAttackSupport = false;
        }
        
        isAttacking = false;
    }


    void AttackSupport()
    {
        if (animator.GetBool("aimSupport"))
            interactiveWithPlayer.LookAtTargetButNotAccuracy();
        if (animator.GetBool("acceSupport"))
        {
            cc.Move(Vector3.zero);

            durationH = animator.GetFloat("acceHorizontalDuration");
            forceH = animator.GetFloat("acceForceH");

            durationV = animator.GetFloat("acceVerticalDuration");
            forceV = animator.GetFloat("acceForceV");

            stopForceH = Vector3.Distance(this.transform.position, player.position) < 1.5f ? 0 : 1;
            //Debug.Log("durationV" + durationV);

            float gravityLocal = -7f;
            Vector3 velocity = interactiveWithPlayer.PlayerDirectionNormailized() * forceH * durationH * stopForceH + Vector3.up * forceV * durationV + Vector3.up * gravityLocal * (1 - durationV);

            cc.Move(velocity * Time.deltaTime);

            //Debug.Log("acce");
        }
        

    }
    void CloseTarget()
    {
        if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
        {
            if (isRelaxingBody) isRelaxingBody = false;
            targetSpeed = Mathf.SmoothDamp(targetSpeed, walkSpeed, ref speedSmoothVelocity, .1f);
            cc.Move(transform.forward * targetSpeed * Time.deltaTime + Vector3.up * gravity);
            interactiveWithPlayer.LookAtTarget(rotateSpeed);

            animator.SetFloat("X", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("Y", 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            isClosingTarget = false;
            isRelaxingBody = true; //這樣寫可能有問題 會
            if( !isRelaxingBody )
            {
                isClosingTarget = false;
            }
        }
    }
    void CloseTargetRun()
    {
        if (Vector3.Distance(targetPlayer.position, this.transform.position) > distance)
        {
            if (isRelaxingBodyFast) isRelaxingBodyFast = false;
            targetSpeed = Mathf.SmoothDamp(targetSpeed, runSpeed, ref speedSmoothVelocity, .1f);
            cc.Move(transform.forward * targetSpeed * Time.deltaTime + Vector3.up * gravity);
            interactiveWithPlayer.LookAtTarget(rotateSpeed * 2.5f);

            animator.SetFloat("X", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("Y", 1f, 0.1f, Time.deltaTime);
            animator.SetFloat("speedPercent", 1f, 0.1f, Time.deltaTime);
        }
        else
        {

            isClosingTargetRun = false;
        }

    }

    void UpdateDeadState()
    {
        paladinAnimation.ChangeAnimation(PaladinAnimation.State.Dead);
        //如果僵尸初次进入死亡状态，那么需要禁用僵尸的一些组件(导航代理组件和碰撞体组件)
        if (enemyAttribute.firstInDead)
        {
            enemyAttribute.firstInDead = false;

            //agent.ResetPath();
            //agent.isStopped = true;
            //agent.enabled = false;

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

    void ReapeatThisMode(FSMState curState, FSMState nextState)
    {
        if (Random.Range(0f, 2f) > 1f && repeatThisMode)
        {
            currentState = curState;
            Debug.Log("Repeat This Mode");
            repeatThisMode = false;
        }
        else
        {
            currentState = nextState;
            repeatThisMode = true;
        }
    }

    void RelaxBody()
    {
        //Debug.Log("RelaxBody");
        speedPercent = (speedPercent == -1f) ? 1f : speedPercent;
        if (Mathf.Abs(animator.GetFloat("X")) > 0.01f || Mathf.Abs(animator.GetFloat("Y")) > 0.01f)
        {
            speedPercent = Mathf.Lerp(speedPercent, 0, 10 * Time.deltaTime);
            cc.Move(Vector3.zero * speedPercent);
            animator.SetFloat("X", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("Y", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("speedPercent", 0f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
            animator.SetFloat("speedPercent", 0f);
            speedPercent = -1f;
            isRelaxingBody = false;

        }
    }

    void RelaxBodyFast()
    {
        //Debug.Log("RelaxBodyFast");
        speedPercent = (speedPercent == -1f) ? 1f : speedPercent;
        if (Mathf.Abs(animator.GetFloat("X")) > 0.01f || Mathf.Abs(animator.GetFloat("Y")) > 0.01f)
        {
            speedPercent = Mathf.Lerp(speedPercent, 0, 100 * Time.deltaTime);
            cc.Move(Vector3.zero * speedPercent);
            animator.SetFloat("X", 0f, 0.03f, Time.deltaTime);
            animator.SetFloat("Y", 0f, 0.03f, Time.deltaTime);
            animator.SetFloat("speedPercent", 0f, 0.03f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
            animator.SetFloat("speedPercent", 0f);
            speedPercent = -1f;
            isRelaxingBodyFast = false;
            Debug.Log("isRelaxingBody" + isRelaxingBody);
        }
    }



    public void OnHitted()
    {
        wasHitted = true;
    }

    void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtWeight(0.6f);
        if (animator)
        {
            if (targetPlayer != null)
            {
                animator.SetLookAtPosition(targetPlayer.position);
            }
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
            //agent.destination = nextDestination;
        }
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
        Gizmos.DrawLine(transform.position, transform.position + direction*3);

    }
}

/*
IEnumerator StartRelaxBodyFast()
{
    isRelaxingBodyFast = true;
    Debug.Log("StartRelaxBodyFast");
    speedPercent = 1f;
    while (Mathf.Abs(animator.GetFloat("X")) > 0.01f || Mathf.Abs(animator.GetFloat("Y")) > 0.01f)
    {
        yield return new WaitForSeconds(Time.deltaTime);
    }
    isRelaxingBodyFast = false;
}
*/


/*
    void UpdateChaseState()
    {
        paladinAnimation.ChangeAnimation(PaladinAnimation.State.Chase);
        /*
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

    }
 
agent.SetDestination(targetPlayer.position);
*/

/*
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
*/
/*
void UpdateDamagedState()
{
    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
    {
        paladinBattle.hitRecover = false;
        paladinAnimation.ChangeAnimation(PaladinAnimation.State.Walk);
        currentState = FSMState.Seek;
        //agent.ResetPath();
    }
}
*/


/*
void HitRecover()
{
    if (paladinBattle.hitRecover)
        currentState = FSMState.Damaged;
}

void CheckIsAlive()
{
    //如果僵尸处于非死亡状态，但是生命值减为0，那么进入死亡状态
    if (paladinAnimation.currentState != PaladinAnimation.State.Dead && !enemyAttribute.isAlive)
    {
        currentState = FSMState.Dead;
    }
}
*/
/*
//计算僵尸的正前方和玩家的夹角，只有玩家在僵尸前方才能攻击
Vector3 direction = targetPlayer.position - this.transform.position;
float degree = Vector3.Angle(direction, this.transform.forward);
float attackTimer = 0f;
if (degree < attackFieldOfView / 2 && degree > -attackFieldOfView / 2)
{
    //animator.SetBool("isAttack", true);
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Attack);
    if (attackTimer > attackInterval)
    {
        attackTimer = 0;
        if (enemyAttribute.attackAudio != null)
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
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Chase);
    Vector3 directionN = direction.normalized;
    directionN.y = 0;
    float rs = 50f;
    Quaternion tr = Quaternion.LookRotation(direction);
    Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
    this.transform.rotation = targetRotation;
    //agent.isStopped = false;
}
*/


//animator.applyRootMotion = true;
//Debug.Log("observeTimer" + observeTimer + "observeSubTimer" + observeSubTimer); 
/*
if ( observeSubTimer <= 0f )
{
    observeSubTimer = Random.Range(0, observeTimer);
    if (observeSubTimer > observeTime / 2) observeTimer = observeTime / 2;

    randomRange = new Vector2(Random.Range(-1, 1), Random.Range(-0.5f, 0.7f));//Random.Range(-1, 1), Random.Range(-0.5f, 0.7f)        
    speedPercent = Random.Range(0.25f, 0.75f);


}
if (Vector3.Distance(player.transform.position, this.transform.position) > observeScope)
{
    //玩家逃離觀察範圍

    break;
}
if (Vector3.Distance(player.transform.position, this.transform.position) > 10f)
{
    //玩家距離過遠
    Debug.Log("玩家距離過遠");
    randomRange.y = 1f;
    speedPercent = Random.Range(0.5f, 1f);

}
if (Vector3.Distance(player.transform.position, this.transform.position) < 1.5f)
{
    //玩家距離過近
    Debug.Log("玩家距離過近");
    randomRange.y = -0.5f;
}

if (observeSubTimer < 0f) 
    observeSubTimer *= randomRange.magnitude; //短距離移動時間短

animator.SetFloat("X", randomRange.x, 0.1f, Time.deltaTime);
animator.SetFloat("Y", randomRange.y, 0.1f, Time.deltaTime);
animator.SetFloat("speedPercent", speedPercent);

Vector3 direction = player.position - this.transform.position;
Vector3 directionN = direction.normalized;
directionN.y = 0;
float rs = 15f;
Quaternion tr = Quaternion.LookRotation(direction);
//Debug.Log(tr);
Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
this.transform.rotation = targetRotation;
*/

/*
//如果僵尸感知范围内没有玩家，进入游荡状态
targetPlayer = GetNearbyPlayer();
if (targetPlayer == null)
{
    currentState = FSMState.Wander;
    agent.ResetPath();
    //animator.SetBool("isAttack", false);
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Walk);
    return;
}

//如果玩家与僵尸的距离，大于僵尸的攻击距离，那么进入追踪状态
if (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange)
{
    currentState = FSMState.Chase;
    agent.ResetPath();
    //animator.SetBool("isAttack", false);
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Chase);
    return;
}
*/
/*
void UpdateWanderState()
{
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Walk);
    targetPlayer = GetNearbyPlayer();

    if (targetPlayer != null)
    {
        currentState = FSMState.Chase;
        //agent.ResetPath();
        return;
    }

    /*
    //如果受到伤害，那么进入搜索状态
    if (enemyBattle.getDamaged)
    {
        currentState = FSMState.Seek;
        agent.ResetPath();
        return;
    }
    */

//如果没有目标位置，那么随机选择一个目标位置
/*
if (AgentDone())
{  //判断僵尸是否到达上一次随机游荡的目的地
    Vector3 randomRange = new Vector3((Random.value - 0.5f) * 2 * wanderScope,
                                    0, (Random.value - 0.5f) * 2 * wanderScope);
    Vector3 nextDestination = this.transform.position + randomRange;
    //agent.destination = nextDestination;
}

//if (agent.destination != Vector3.zero)
//    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Walk);
//限制游荡的速度
//SetMaxAgentSpeed(wanderSpeed);
//统计僵尸在当前位置附近的停留时间
CaculateStopTime();

}
*/
/*
void UpdateSeekState()
{
    paladinAnimation.ChangeAnimation(PaladinAnimation.State.Walk);
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
    SetMaxAgentSpeed(runSpeed);

}
*/

/*
Vector3 velocity = Vector3.zero;
Vector2 movement = Vector2.zero;
float sign = interactiveWithPlayer.RandomSign();
Vector3 direction = interactiveWithPlayer.PlayerDirectionNormailized();
int playerDistanceState = interactiveWithPlayer.GetCurrenDistanceState();
while (watchdogTimer >= 0f)
{
    interactiveWithPlayer.LookAtTargetButNotAccuracy();
    yield return new WaitForSeconds(Time.deltaTime);
}


if (playerDistanceState == 0)
{
    // <1.33f 玩家距離過近
    velocity = sign * this.transform.right *0.5f;
    velocity -= transform.forward * 0.7f;
    velocity *= walkSpeed;
    movement = new Vector2(sign * 0.5f, -0.7f);

    //Debug.Log("玩家距離過近");
}
if (playerDistanceState == 1 || playerDistanceState == 2)
{
    //2f ~ 5f
    //Debug.Log("玩家距離適中");
    velocity = sign * this.transform.right * walkSpeed;
    movement = new Vector2(sign, 0);
}
if (playerDistanceState == 3)
{
    //5f~10f 
    //Debug.Log("玩家距離稍遠");
    velocity = sign * this.transform.right * 0.5f;
    velocity += transform.forward;
    velocity *= walkSpeed;
    movement = new Vector2(0.5f * sign, 1);

}
if (playerDistanceState == 4)
{
    velocity += transform.forward;
    velocity *= runSpeed;
    movement = new Vector2(0, 1);
    animator.SetFloat("speedPercent", 1f, 0.1f, Time.deltaTime);
    Debug.Log("玩家距離過遠");
    //10f~30f 玩家距離過遠
}
if (playerDistanceState == 5)
{
    //>30f 玩家逃離戰場
    velocity = Vector3.zero;
    movement = new Vector2(0, 0);
    Debug.Log("玩家逃離戰場");
}
direction = interactiveWithPlayer.PlayerDirectionNormailized();

while (watchdogTimer >= 0f)
{
    watchdogTimer -= Time.deltaTime;

    cc.Move(velocity * Time.deltaTime);
    animator.SetFloat("X", movement.x, 0.1f, Time.deltaTime);
    animator.SetFloat("Y", movement.y, 0.1f, Time.deltaTime);

    //interactiveWithPlayer.LookAtTargetButNotAccuracy();
    yield return new WaitForSeconds(Time.deltaTime);
}
*/

/*
IEnumerator CloseTarget2()
{
    isClosingTarget = true;
    targetPlayer = player;
    while (Vector3.Distance(targetPlayer.position, this.transform.position) > attackRange) 
    {
        targetSpeed = Mathf.SmoothDamp(targetSpeed, walkSpeed, ref speedSmoothVelocity, .1f);
        cc.Move(transform.forward * targetSpeed * Time.deltaTime + Vector3.up * gravity);
        interactiveWithPlayer.LookAtTarget(rotateSpeed);

        animator.SetFloat("X", 0f, 0.1f, Time.deltaTime);
        animator.SetFloat("Y", 1f, 0.1f, Time.deltaTime);
        yield return new WaitForSeconds(Time.deltaTime);
    }
    if (!isRelaxingBody)
    {
        StartCoroutine(StartRelaxBodyFast());
    }
    while (isRelaxingBody)
    {
        yield return new WaitForSeconds(Time.deltaTime);
    }
    isClosingTarget = false;
}
*/


/*
IEnumerator CloseTarget_Run(float distance)
{
    isClosingTarget = true;
    while (Vector3.Distance(targetPlayer.position, this.transform.position) > distance)
    {
        targetSpeed = Mathf.SmoothDamp(targetSpeed, runSpeed, ref speedSmoothVelocity, .1f);
        cc.Move(transform.forward * targetSpeed * Time.deltaTime + Vector3.up * gravity);
        interactiveWithPlayer.LookAtTarget(rotateSpeed * 2.5f);

        animator.SetFloat("X", 0f, 0.1f, Time.deltaTime);
        animator.SetFloat("Y", 1f, 0.1f, Time.deltaTime);
        animator.SetFloat("speedPercent", 1f, 0.1f, Time.deltaTime);
        yield return new WaitForSeconds(Time.deltaTime);
    }

    if(!isRelaxingBody)
    {
        StartCoroutine(StartRelaxBodyFast());

    }

    /*
    while( isRelaxingBody )
    {
        yield return new WaitForSeconds(Time.deltaTime);
    }

    isClosingTarget = false;
}
*/
/*
while (animator.GetBool("isInteracting"))
{
    if (animator.GetBool("aimSupport"))
        interactiveWithPlayer.LookAtTargetButNotAccuracy();
    if (animator.GetBool("acceSupport"))
    {
        cc.Move(Vector3.zero);

        durationH = animator.GetFloat("acceHorizontalDuration");
        forceH = animator.GetFloat("acceForceH");

        durationV = animator.GetFloat("acceVerticalDuration");
        forceV = animator.GetFloat("acceForceV");

        stopForceH = Vector3.Distance(this.transform.position, player.position) < 1.5f ? 0 : 1;
        //Debug.Log("durationV" + durationV);

        float gravityLocal = -7f;
        Vector3 velocity = interactiveWithPlayer.PlayerDirectionNormailized() * forceH * durationH * stopForceH + Vector3.up * forceV * durationV + Vector3.up * gravityLocal * (1 - durationV);

        cc.Move(velocity * Time.deltaTime);

        //Debug.Log("acce");
    }
    yield return new WaitForSeconds(Time.deltaTime);
}
*/
