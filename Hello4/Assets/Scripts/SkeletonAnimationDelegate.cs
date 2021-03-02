using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationDelegate : AnimationDelegate
{
    public GameObject crazyEye1;
    public Transform swordTrail;
    CachedObject burstVFX;
    

    public override void Attack_On()
    {
        base.Attack_On();
        swordTrail.gameObject.SetActive(true);
        //Debug.Log("skeleton attack!");
    }

    public override void Attack_Off()
    {
        base.Attack_Off();
        swordTrail.gameObject.SetActive(false);
        //Debug.Log("skeleton attack2!");
    }

    public void CrazyEye1_On()
    {
        crazyEye1.SetActive(true);
    }

    public void CrazyEye1_Off()
    {
        crazyEye1.SetActive(false);
    }
    public void BurstVFX_On()
    {
        CrazyEye1_On();
        burstVFX = Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.burst, this.transform.position, Quaternion.identity.eulerAngles, this.transform);
    }
    public void BurstVFX_Off()
    {
        CrazyEye1_Off();
        if (burstVFX != null) Game.cacheObjectControllerSingle.GiveBackAsset(burstVFX);
    }

}
