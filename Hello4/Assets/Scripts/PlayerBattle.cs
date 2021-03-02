using System.Collections;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    Animator animator;
    AnimatorStateInfo stateInfo;

    private CharacterAnimationDelegate player_Anim;
    float knockDownValue = 0f;
    float wonderPower = 0f;
    float atkBuffer = 0f;
    [HideInInspector]
    public bool isInvinsible = false;
    [HideInInspector]
    public float invinsibleTime = 0.2f;
    const float invinsibleTime_original = 0.2f;
    bool isLookingEnemy = false;
    float lookEnemyTimer = 0;
    float lookEnemyTime = 0.2f;
    bool isEating = false;
    public GameObject gun;
    public Transform gunShootPoint;
    [HideInInspector]
    public bool isFinishMoving = false;
    CacheObjectController CoController;
    SoundContainer soundContainer;

    const float knockDownResistance = 0.4f;

    //block system
    bool blockBtn = false;
    float blockForceSuccessMultiply = 0.1f;
    float blockForceFailMultiply = 0.3f;
    const float blockEnergyBarMaximum = 100f;
    float blockEnergyBar = blockEnergyBarMaximum;
    const float justGuardTime = 0.2f;
    bool isJustGuard = false;
    public GameObject shield;

    private void Awake()
    {
        player_Anim = GetComponentInChildren<CharacterAnimationDelegate>();
        animator = GetComponent<Animator>();
        CoController = Game.cacheObjectControllerSingle;
        soundContainer = this.GetComponent<SoundContainer>();
    }

    private void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        BattleInput();
    }

    void BattleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            if (animator.GetFloat("LockOn") == 1f) this.SendMessage("StartChangeRotateSpeed", SendMessageOptions.DontRequireReceiver);
            LightAttack();

        } // combo attacks

        if (Input.GetMouseButtonDown(1))
        {
            if (animator.GetFloat("LockOn") == 1f) this.SendMessage("StartChangeRotateSpeed", SendMessageOptions.DontRequireReceiver);
            StrongAttack();
        } // combo attacks


        if(Input.GetMouseButtonDown(1))
        {
            if (animator.GetFloat("BlockState") > 0.9f)
                return;
            animator.SetBool("chargeBtn", true);
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            /*
            if ( animator.GetFloat("BlockState") <= 0f && stateInfo.IsTag("Move"))
                animator.CrossFadeInFixedTime("Base Layer.Charge", 0.08f);
            */
        }

        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("chargeBtn", false);
        }

        if (Input.GetKey(KeyCode.F))
        {
            blockBtn = true;
        }
        else
        {
            blockBtn = false;
        }

        if (blockBtn)
        {
            animator.SetFloat("BlockState", 1f);

        }    
        else
        {
            animator.SetFloat("BlockState", 0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Game.quickItemBarSingle.IsPotionEnough())
            {
                if (!isEating)
                {
                    stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfo.IsName("Move"))
                    {
                        StartCoroutine(StartEat());
                    }
                        
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C) )
        {
            
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(stateInfo.IsName("Move") && Game.quickItemBarSingle.IsBulletEnough())
            {
                gun.SetActive(true);
                animator.CrossFadeInFixedTime("UpperBody Layer.Shoot", 0.08f);
            }

            if(!Game.quickItemBarSingle.IsBulletEnough())
            {
                animator.CrossFadeInFixedTime("UpperBody Layer.NoBullet", 0.08f);
            }      
        }
    } 
 

    public void TakeDamage(ChangeInfo info)
    {
        if (isInvinsible) return;
        bool damageHandled = false;
        AnimatorStateInfo stateinfo1 = animator.GetCurrentAnimatorStateInfo(1);

        if (!damageHandled)
        {
            //Block system caculate

            Vector3 direction = info.murderer.position - this.transform.position;
            bool enemyInFront = Vector3.Angle(direction, this.transform.forward) < 90f ? true : false;

            if (enemyInFront && animator.GetFloat("BlockState") >= 1f)
            {
                if (isJustGuard)
                {
                    Debug.Log("JustGuard!");
                    Quaternion rot = Quaternion.LookRotation(direction);
                    Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.guardGlow_big, shield.transform.position, rot.eulerAngles);
                    info.delta = 0f;
                }

                Debug.Log("blockEnergyBar" + blockEnergyBar);

                if (blockEnergyBar < 0)
                {
                    animator.Play("Block_Damage");
                    info.delta = info.delta * blockForceFailMultiply;
                    blockEnergyBar = blockEnergyBarMaximum;
                }
                else
                {
                    animator.Play("Block_Guard");
                    Quaternion rot = Quaternion.LookRotation(direction);
                    Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.guardGlow_small, shield.transform.position, rot.eulerAngles);
                    info.delta = info.delta * blockForceSuccessMultiply;
                    this.transform.SendMessage("AddMove_Backward", SendMessageOptions.DontRequireReceiver);
                    this.transform.SendMessage("PlayGuardSound", SendMessageOptions.DontRequireReceiver);
                }

                blockEnergyBar += info.delta;
                damageHandled = true;
            }

        }

        //Debug.Log("info.delta / Game.playerAttrSingle.maxHp" + info.delta / Game.playerAttrSingle.maxHp);
        if(!damageHandled)
        {
            animator.SendMessage("PlaySwordHitSound", SendMessageOptions.DontRequireReceiver);

            if (Mathf.Abs(info.delta) / Game.playerAttrSingle.maxHp > knockDownResistance)
            {
                animator.Play("KnockDown");
                InvinsibleBuff(1.2f);

            }
            else
            {
                animator.Play("Hit");
                GenerateBloodVFX(info.motionRate, info.attackDirection, info.contactPoint);
            }

            damageHandled = true;
        }

        Game.hpBarSingle.ChangeBarValue(info);
    }

    void GenerateBloodVFX(float motionRate, Vector3 hitDirection, Vector3 contactPoint)
    {

        if (motionRate  >= 3f)
        {
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseBig");
        }

        CachedObject co;

        Quaternion rot = Quaternion.LookRotation(hitDirection);

        if (motionRate >= 2f)
        {
            Vector3 place = new Vector3(this.transform.position.x, 0.001f, this.transform.position.z);
            Game.plastererSingle.Construction(place);
            co = CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_more, contactPoint, rot.eulerAngles, this.transform);
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseSmall");
        }
        else
        {
            if (Random.Range(0, 3f) > 2)
            {
                Vector3 place = new Vector3(this.transform.position.x, 0.001f, this.transform.position.z);
                Game.plastererSingle.Construction(place);
            }
            co = CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_less, contactPoint, rot.eulerAngles, this.transform);
        }
    }


    public void LightAttack()
    {
        animator.SetInteger("LightAttack", animator.GetInteger("LightAttack") + 1);


        if (animator.GetInteger("LightAttack") >= 1 && stateInfo.IsTag("Move") && !Game.IsPlayerAttacking() )
            animator.CrossFadeInFixedTime("Base Layer.LightAttack1", 0.1f);

    }

    public void StrongAttack()
    {
        animator.SetInteger("strongAttack", animator.GetInteger("strongAttack") + 1);
    }


    void LookEnemy()
    {
        if (!isLookingEnemy)
            StartCoroutine(StartLookEnemy());
    }

    IEnumerator StartGunShootVFX()
    {
        Game.quickItemBarSingle.UseBullet();
        CachedObject co = CoController.BorrowAsset(CacheObjectController.ObjType.shoot_explosion, gunShootPoint.position, gunShootPoint.eulerAngles);
        soundContainer.PlayGunShootSound();

        RaycastHit hitInfo;
        Vector3 direction = Vector3.zero;
        if(Game.Singleton.nearbyEnemy == null)
        {
            direction = this.transform.forward;
        }
        else
        {
            direction = Game.Singleton.nearbyEnemy.position - this.transform.position;
        }

        if(Physics.Raycast(this.transform.position, direction, out hitInfo, 15f, Game.Singleton.enemyLayer ))
        {
            Debug.Log("Shoot " + hitInfo.transform.name);
            ChangeInfo info = new ChangeInfo(-Random.Range(8, 15));
            info.hitByGun = true;
            hitInfo.transform.SendMessage("TakeDamage", info);
            CounterAttackSystem otherCa =  hitInfo.transform.GetComponent<CounterAttackSystem>();
            if(otherCa != null )
            {
                if(otherCa.isCounterAttackedPointRevealing)
                {
                    otherCa.ConuterAttackPointBroken();
                    if(!isFinishMoving)
                    {
                        StartCoroutine(StartFinishMove());
                    }
                }
            }
        }



        yield return new WaitForSeconds(5f);
        CoController.GiveBackAsset(co);
    }


    IEnumerator StartFinishMove()
    {
        isFinishMoving = true;
        yield return new WaitForSeconds(3f);
        isFinishMoving = false;
    }


    void EndShoot()
    {
        gun.SetActive(false);
    }

    IEnumerator StartLookEnemy()
    {
        isLookingEnemy = true;
        Debug.Log("IsLooking");
        Vector3 dir1 = this.transform.forward;
        dir1.y = 0;
        Vector3 dir2 = Game.Singleton.nearbyEnemy.position - this.transform.position;
        dir2.y = 0f;
        lookEnemyTimer = lookEnemyTime;
        while ( Vector3.Angle(dir1, dir2)  > 5f && lookEnemyTimer > 0f) 
        {
            Debug.Log("IsLooking" + Vector3.Angle(this.transform.forward, Game.Singleton.nearbyEnemy.position - this.transform.position)　);
            lookEnemyTimer -= Time.deltaTime;
            Vector3 targetDir = Vector3.zero;
            targetDir = Game.Singleton.nearbyEnemy.position - this.transform.position;
            targetDir = targetDir.normalized;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = this.transform.forward;

            float rs = 5f;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(this.transform.rotation, tr, rs * Time.deltaTime);
            this.transform.rotation = targetRotation;

            dir1 = this.transform.forward;
            dir1.y = 0;
            dir2 = Game.Singleton.nearbyEnemy.position - this.transform.position;
            dir2.y = 0f;


            yield return new WaitForSeconds(Time.deltaTime);
        }
        isLookingEnemy = false;
    }

    IEnumerator StartEat()
    {
        isEating = true;
        CoController.BorrowAsset(CacheObjectController.ObjType.healPotionVFX, transform.position, Quaternion.identity.eulerAngles, transform);
        animator.CrossFadeInFixedTime("UpperBody Layer.Eat", 0.08f);
        yield return new WaitForSeconds(1.4f);
       
        isEating = false;
    }
    void Heal()
    {
        Game.quickItemBarSingle.UsePotion();
        Game.hpBarSingle.ChangeBarValue(new ChangeInfo(50f));
    }

    IEnumerator Spotty()
    {
        animator.speed = 0f;
        yield return new WaitForSeconds(4 * Time.deltaTime);
        animator.speed = 1f;
    }


    public void InvinsibleBuff()
    {
        if (!isInvinsible)
            StartCoroutine(StartInvincible());
    }

    public void InvinsibleBuff(float delta)
    {
        invinsibleTime = delta;
        if (!isInvinsible)
            StartCoroutine(StartInvincible());
    }

    public void EndInvinsibleBuff()
    {
        isInvinsible = false;
    }

    IEnumerator StartInvincible()
    {
        isInvinsible = true;
        Debug.Log("Invincible");
        yield return new WaitForSeconds(invinsibleTime);

        invinsibleTime = invinsibleTime_original;
        isInvinsible = false;
    }

    IEnumerator StartInvincibleInSec(float time)
    {
        isInvinsible = true;
        Debug.Log("Invincible");
        yield return new WaitForSeconds(time);
        isInvinsible = false;
    }


    public float GetAttackDamage()
    {
        //Debug.Log(-(attackDamage + Random.Range(attackDamage / 9, attackDamage / 10)) * animator.GetFloat("AttackPower"));
        float atk = Game.playerAttrSingle.attackDamage;
        atkBuffer = -(atk + Random.Range(-atk / 10, atk / 10)) * animator.GetFloat("motionRate");
        return atkBuffer;
    }
    public float GetWonderPower()
    {
        return Mathf.Abs(atkBuffer) * 0.33f;
    }

    IEnumerator StartJustGuardBuff()
    {
        isJustGuard = true;
        yield return new WaitForSeconds(justGuardTime);
        isJustGuard = false;

    }
}
/*
if ( chargeBtn )
{
    animator.ResetTrigger("ChargeAttack");
    //Debug.Log("rightButton");
    animator.SetBool("chargeBtn", true);
    chargeTimer += Time.deltaTime;
    if(chargeTimer >= chargeTime)
    {
        animator.SetBool("chargeFull", true);
        if ( !chargeFullLightHinting )
            StartCoroutine(StartChargeFullHint());
    }
}


if( Input.GetMouseButtonUp(1))
{
    animator.SetBool("chargeBtn", false);
    animator.SetTrigger("ChargeAttack");
    chargeFullLightHinting = false;
    chargeTimer = 0f;
}
*/
/*
Debug.Log(stateinfo1.IsName("BlockLocomotion") + "BlockLocomotion"); //false
Debug.Log(stateinfo1.IsName("Block_Loop") + "Block_Loop"); //true
Debug.Log(stateinfo1.IsName("BlockLocomotion") + "BlockLocomotion"); //false
Debug.Log(stateinfo1.IsName("BlockStateLayer.BlockLocomotion.Block_Loop") + "BlockStateLayer.BlockLocomotion.Block_Loop"); // true
Debug.Log(stateinfo1.IsName("BlockStateLayer.BlockLocomotion") + "BlockStateLayer.BlockLocomotion"); // false
Debug.Log(stateinfo1.IsName("BlockLocomotion.Block_Loop") + "BlockLocomotion.Block_Loop"); // true
Debug.Log(stateinfo1.fullPathHash + "fullPathHash");
*/