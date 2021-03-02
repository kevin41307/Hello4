using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTut : MonoBehaviour
{
    public float minForce;
    public float MaxForce;
    public float radius;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        Explode();
    }
    
    public void Explode()
    {
        foreach( Transform t in transform )
        {

            if( t != null)
            {
                Rigidbody rb = t.GetComponent<Rigidbody>();
                if( rb != null )
                {
                    rb.AddExplosionForce(Random.Range(minForce, MaxForce), transform.position, radius);
                }
            }


        }
        Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.smoke1, transform.position, Quaternion.identity.eulerAngles);
    }
}
