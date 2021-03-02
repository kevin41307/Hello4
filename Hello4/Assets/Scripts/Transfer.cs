using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transfer : MonoBehaviour
{
    SkeletonBattle skeletonBattle;
    
    private void Awake()
    {
        skeletonBattle = GetComponentInParent<SkeletonBattle>();
    }
    public void TakeDamage(ChangeInfo info)
    {
        if(skeletonBattle != null )
        {
            skeletonBattle.TakeDamage(info);
        }
        else
        {
            Debug.Log("skeletonBattle is null! Transfer msg failed.");
        }
    }
}
