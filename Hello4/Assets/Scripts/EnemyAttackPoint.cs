using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPoint : MonoBehaviour
{
    public LayerMask collisionLayer;

    [HideInInspector]
    public float sphereRadius = 0f;
    [HideInInspector]
    public bool sphereDetection;

    void FixedUpdate()
    {
        if (sphereDetection)
        {
            DetectCollisionSphere();
            return;
        }
    }
    void DetectCollisionSphere()
    {
        //Debug.Log("DetectCollisionSphere");
        Collider[] hit = Physics.OverlapSphere(transform.position, sphereRadius, Game.Singleton.playerLayer);
        if (hit.Length > 0)
        {
            Debug.Log(transform.name + " hit " + hit[0].transform.name + " by DetectCollisionSphere.");
            HandleCollision(hit);
            //animator.SendMessage("PlaySwordHitSound", SendMessageOptions.DontRequireReceiver);
            //float delta = Game.playerAttrSingle.playerBattle.GetAttackDamage();

            //GenerateBloodVFX(transform.position, hit[0].transform);

            /*
            ChangeInfo info = new ChangeInfo(Game.bossAttribute.paladinBattle.GetAttackDamage(), animator.GetFloat("motionRate"));
            info.attackDirection = -transform.right;
            info.contactPoint = hitInfo.point;
            hitInfo.transform.gameObject.SendMessage("TakeDamage", info);
            */



            Debug.Log("PlayerDamaged");

            gameObject.SetActive(false);
        }


    }

    public virtual void HandleCollision(Collider[] hit)
    {
        //Debug.Log("DoSomething...");
    }



}
