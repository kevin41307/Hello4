using UnityEngine;
using System.Collections.Generic;

public class BloodprintVFX : MonoBehaviour
{
    int count = 0;
    const int maxCount = 10;
    float rate = 0.2f;
    [HideInInspector]
    public List<ParticleCollisionEvent> collisionEvents;
    ParticleSystem ps;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();

        var collision = ps.collision;
        int layermask =~ (LayerMask.GetMask("Enemy") | LayerMask.GetMask("EnemyButNoPlayer"));

        collision.collidesWith = layermask;
    }
    private void OnEnable()
    {
        count = 0;
        //Debug.Log(transform.name);
    }

    public void OnParticleCollision(GameObject other)
    {
        //Debug.Log("collsion!" + other.name);
        if (count >= maxCount) return;


        int i = 0;
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
        while (i < numCollisionEvents && count < maxCount)
        {
            if(Random.Range(0, 1f) > rate)
            {
                Vector3 pos = collisionEvents[i].intersection;
                pos.y = 0;
                Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.bloodprint, pos, Quaternion.identity.eulerAngles);
                count++;
            }
            i++;   
        }
        //collisionEvents.Clear();
    }
}
