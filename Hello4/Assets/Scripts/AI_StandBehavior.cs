using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StandBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("motionRate", 1f);
        animator.SetBool("aimSupport", false);
        animator.SetBool("isInteracting", false);


    }
}
