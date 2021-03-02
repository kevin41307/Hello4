using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    CacheObjectController CoController;
    Transform[] spawnPoint;
    public float frequency = 5f;
    public int totalAmount = 10;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }


    // Start is called before the first frame update
    void Start()
    {
        CoController = Game.cacheObjectControllerSingle;
        /*
        for(int i = 1; i < spawnPoint.Length; i++)
        {
            Debug.Log("spawnPoint" + spawnPoint[i].position);
        }
        */
        StartCoroutine(Spwan());
    }


    IEnumerator Spwan()
    {
        //yield return new WaitForSeconds(.1f);
        int count = 0;
        while( spawnPoint.Length > 1)
        {
            int place = Random.Range(1, spawnPoint.Length);
            if( CoController.IsCachedRemained(CacheObjectController.ObjType.skeleton))
            {
                CoController.BorrowAsset(CacheObjectController.ObjType.summon, spawnPoint[place].position, Quaternion.identity.eulerAngles);
                yield return new WaitForSeconds(.4f);

                CachedObject co = CoController.BorrowAsset(CacheObjectController.ObjType.skeleton, spawnPoint[place].position, Quaternion.identity.eulerAngles);     
                if(co != null)
                {

                    AIController_Base aIController_Skeleton = co._asset.GetComponent<AIController_Base>();
                    aIController_Skeleton.co = co;
                }
            }

            count += 1;
            yield return new WaitForSeconds(frequency);
        }

    }
}

