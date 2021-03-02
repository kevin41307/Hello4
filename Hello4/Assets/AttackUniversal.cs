using UnityEngine;
using System.Collections.Generic;
//Player專用
public class AttackUniversal : MonoBehaviour {

    public LayerMask collisionLayer;

    Vector3 swordPoint;
    float radius = 0.09f;
    float length = 1.65f;
    bool detectedHitPoint = false;
    Animator animator;
    PlayerBattle playerBattle;
    CounterAttackSystem myCounterAttackSystem;
    CounterAttackSystem otherCA;
    CacheObjectController CoController;
    RaycastHit hitInfo;

    List<Transform> attackedEnemies;

    float sphereRadius = 1f;
    public bool sphereDetection;

    private void Awake()
    {
        animator = Game.playerAttrSingle.animator;
        playerBattle = Game.playerAttrSingle.playerBattle;
        myCounterAttackSystem = animator.gameObject.GetComponent<CounterAttackSystem>();
        CoController = Game.cacheObjectControllerSingle;
        attackedEnemies = Game.Singleton.attackedEnemies;
    }

    private void Start()
    {

        swordPoint = this.transform.position - this.transform.forward * 1f;
    }

    private void LateUpdate()
    {
        swordPoint = this.transform.position - this.transform.forward * 1f;
    }

    void FixedUpdate()
    {
        if(sphereDetection)
        {
            DetectCollisionSphere();
            return;
        }
        DetectCollision(); 
    }
    void DetectCollision()
    {     
        if (Physics.SphereCast(swordPoint, radius, this.transform.forward, out hitInfo, length, Game.Singleton.enemyLayer))
        {
            if (!attackedEnemies.Exists(x => x == hitInfo.transform ))
            {
                attackedEnemies.Add(hitInfo.transform);
            } 
            else
            {
                //Debug.Log("Pass Attacked Enemy");
                return;
            }
            
            //Debug.Log("hitpoint" + hitInfo.point);
            if (playerBattle.isFinishMoving)
            {
                otherCA = null;
                otherCA = hitInfo.transform.GetComponent<CounterAttackSystem>();
                if( otherCA != null )
                {
                    if( otherCA.isCounterAttackedPressing)
                    {
                        myCounterAttackSystem.FinishMovePressSuccess(otherCA.myTransform);
                        otherCA.isCounterAttackedPressing = false;
                        playerBattle.isFinishMoving = false;
                        gameObject.SetActive(false);
                        return;
                    }
                }
            }

            animator.SendMessage("PlaySwordHitSound", SendMessageOptions.DontRequireReceiver);

            GenerateBloodVFX(hitInfo);

            ChangeInfo changeInfo = new ChangeInfo(Game.playerAttrSingle.playerBattle.GetAttackDamage(), animator.GetFloat("motionRate"), Game.playerAttrSingle.playerBattle.GetWonderPower());
            changeInfo.hitFromRightSide = HitFromRightSide();
            hitInfo.transform.gameObject.SendMessage("TakeDamage", changeInfo);
            Game.playerAttrSingle.player.SendMessage("Spotty", SendMessageOptions.DontRequireReceiver);
            if (Game.hpBarSingle.recoveryBuff)
                Game.hpBarSingle.AttackToRecoveryHp();

            //gameObject.SetActive(false);
        } // detect collision
    }

    void DetectCollisionSphere()
    {
        //Debug.Log("DetectCollisionSphere");
        Collider[] hit = Physics.OverlapSphere(transform.position, sphereRadius, Game.Singleton.enemyLayer);
        if (hit.Length > 0)
        {
            Debug.Log("DetectCollisionSphere" + hit[0].transform.name);
            animator.SendMessage("PlaySwordHitSound", SendMessageOptions.DontRequireReceiver);
            float delta = Game.playerAttrSingle.playerBattle.GetAttackDamage();

            GenerateBloodVFX(transform.position, hit[0].transform);

            ChangeInfo changeInfo = new ChangeInfo(delta, animator.GetFloat("motionRate"), Game.playerAttrSingle.playerBattle.GetWonderPower());
            if (animator.GetBool("directDamage"))
                changeInfo.directDamage = true;
            hit[0].transform.gameObject.SendMessage("TakeDamage", changeInfo);

            Game.playerAttrSingle.player.SendMessage("Spotty", SendMessageOptions.DontRequireReceiver);
            if (Game.hpBarSingle.recoveryBuff)
                Game.hpBarSingle.AttackToRecoveryHp();
            if (animator.GetBool("chargeFull"))
            {
                Game.playerAttrSingle.player.SendMessage("GenerateImpulse", Vector3.down);
                //Debug.Log("ShakeIt");
            }
            //Debug.Log("hit");

            gameObject.SetActive(false);
        }
        

    }

    void GenerateBloodVFX(RaycastHit hitInfo)
    {
        if (animator.GetFloat("motionRate") >= 3f)
        {
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseBig");
        }

        //CachedObject co;
        Quaternion rot = Quaternion.LookRotation(-transform.right);

        if (animator.GetFloat("motionRate") >= 2f)
        {
            Vector3 place = new Vector3(hitInfo.transform.position.x, 0.001f, hitInfo.transform.position.z);
            Game.plastererSingle.Construction(place);
            CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_more, hitInfo.point, rot.eulerAngles, hitInfo.transform);
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseSmall");
        }
        else
        {
            if (Random.Range(0, 3f) > 2)
            {
                Vector3 place = new Vector3(hitInfo.transform.position.x, 0.001f, hitInfo.transform.position.z);
                //Game.plastererSingle.Construction(place);
            }
            
            //CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_less, hitInfo.point, rot.eulerAngles, hitInfo.transform);
            CoController.BorrowAsset(CacheObjectController.ObjType.bloodDirection, hitInfo.point, rot.eulerAngles, hitInfo.transform);
        }
    }


    void GenerateBloodVFX(Vector3 hitpoint, Transform hitTransform)
    {
        if (animator.GetFloat("motionRate") >= 3f)
        {
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseBig");
        }

        Quaternion rot = Quaternion.LookRotation(-transform.forward);

        if (animator.GetFloat("motionRate") >= 2f)
        {
            Vector3 place = new Vector3(hitTransform.position.x, 0.001f, hitTransform.position.z);
            Game.plastererSingle.Construction(place);
            CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_more, hitpoint, rot.eulerAngles, hitTransform);
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseSmall");
        }
        else
        {
            if (Random.Range(0, 3f) > 2)
            {
                Vector3 place = new Vector3(hitInfo.transform.position.x, 0.001f, hitInfo.transform.position.z);
                Game.plastererSingle.Construction(place);
            }
            CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_less, hitpoint, rot.eulerAngles, hitTransform);
        }
    }

    float HitFromRightSide()
    {
        float hitFromRight = 0;
        Vector3 dir1 = Game.playerAttrSingle.player.transform.right;
        dir1.y = 0;
        Vector3 dir2 = this.transform.right;
        dir2.y = 0;

        if ( Vector3.Angle(dir1, dir2) < 45f)
        {
            hitFromRight = 1;
        }
        else
        {
            hitFromRight = -1;
        }
        return hitFromRight;
    }

    void OnDrawGizmos()
    {

        Gizmos.color = new Color(0, 1, 0, 1f);
        //Gizmos.DrawSphere(transform.position, radius);
        //Gizmos.DrawLine(this.transform.position - this.transform.up * 1f, this.transform.position + this.transform.up * 0.7f);
        //Gizmos.DrawLine(this.transform.position + this.transform.right * 0.2f, this.transform.position - this.transform.right * 0.2f);

        Gizmos.DrawLine(swordPoint, swordPoint + this.transform.forward * length);
        Gizmos.color = new Color(0, 1, 0, 0.3f);

        Gizmos.DrawSphere(swordPoint, radius);
        Gizmos.DrawSphere(swordPoint + this.transform.forward * length, radius);

        //Gizmos.DrawSphere(transform.position, sphereRadius);



    }


} // class








