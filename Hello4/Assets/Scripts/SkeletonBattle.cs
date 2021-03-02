using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattle : EnemyBattle
{
    SkeletonAnimations skeletonAnimations;
    AIController_Skeleton aIController_Skeleton;

    public override void Awake()
    {
        base.Awake();
        skeletonAnimations = GetComponent<SkeletonAnimations>();
        aIController_Skeleton = GetComponent<AIController_Skeleton>();

    }


    public override void TakeDamage(ChangeInfo info)
    {
        base.TakeDamage(info);

        skeletonAnimations.ChangeAnimation(SkeletonAnimations.AniState.Damaged);
        aIController_Skeleton.StateChange(AIController_Skeleton.FSMState.Damaged);
    }


}
