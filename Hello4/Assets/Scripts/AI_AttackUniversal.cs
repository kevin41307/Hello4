using UnityEngine;

public class AI_AttackUniversal : MonoBehaviour
{
    Animator animator;
    Vector3 swordPoint;
    float radius = 0.14f;
    float length = 1.95f;
    CacheObjectController CoController;
    private void Awake()
    {
        CoController = Game.cacheObjectControllerSingle;
    }

    private void Start()
    {
        swordPoint = this.transform.position - this.transform.up * 1f;
        animator = Game.bossAttribute.animator;
    }

    private void LateUpdate()
    {
        swordPoint = this.transform.position - this.transform.up * 1f;
    }

    void FixedUpdate()
    {
        DetectCollision();
    }

    void DetectCollision()
    {
        RaycastHit hitInfo;

        if (Physics.SphereCast(swordPoint, radius, this.transform.up, out hitInfo, length, Game.Singleton.playerLayer))
        {
            Debug.Log("hitpointPlayer" + hitInfo.point);
            if (Game.playerAttrSingle.playerBattle.isInvinsible)
            {
                gameObject.SetActive(false);
                return;
            }
            //GenerateBloodVFX(hitInfo);
            //hitInfo.transform.gameObject.GetComponent<EnemyBattle>().enemyHpBar.ChangeBarValue(new ChangeInfo(Game.playerAttrSingle.GetAttackDamage()));
            ChangeInfo info = new ChangeInfo(Game.bossAttribute.paladinBattle.GetAttackDamage(), animator.GetFloat("motionRate"));
            info.attackDirection = -transform.right;
            info.contactPoint = hitInfo.point;
            info.murderer = this.transform;
            hitInfo.transform.gameObject.SendMessage("TakeDamage", info);


            Debug.Log("PlayerDamaged");
            
            gameObject.SetActive(false);


        } // detect collision
    }


    void GenerateBloodVFX(RaycastHit hitInfo)
    {
        
        if (animator.GetFloat("motionRate") >= 3f)
        {
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseBig");
        }

        CachedObject co;
        
        Quaternion rot = Quaternion.LookRotation(-transform.right);

        if (animator.GetFloat("motionRate") >= 2f)
        {
            Vector3 place = new Vector3(hitInfo.transform.position.x, 0.001f, hitInfo.transform.position.z);
            Game.plastererSingle.Construction(place);
            co = CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_more, hitInfo.point, rot.eulerAngles, hitInfo.transform);
            Game.playerAttrSingle.player.SendMessage("GenerateImpulseSmall");
        }
        else
        {
            if (Random.Range(0, 3f) > 2)
            {
                Vector3 place = new Vector3(hitInfo.transform.position.x, 0.001f, hitInfo.transform.position.z);
                Game.plastererSingle.Construction(place);
            }
            co = CoController.BorrowAsset(CacheObjectController.ObjType.hit_blood_less, hitInfo.point, rot.eulerAngles, hitInfo.transform);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        //Gizmos.DrawSphere(transform.position, radius);
        //Gizmos.DrawLine(this.transform.position - this.transform.up * 1f, this.transform.position + this.transform.up * 0.7f);
        //Gizmos.DrawLine(this.transform.position + this.transform.right * 0.2f, this.transform.position - this.transform.right * 0.2f);

        Gizmos.DrawLine(swordPoint, swordPoint + this.transform.up * length);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        
        Gizmos.DrawSphere(swordPoint, radius);
        Gizmos.DrawSphere(swordPoint + this.transform.up * length, radius);


    }


}
