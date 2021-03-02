using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_HitBehavior : StateMachineBehaviour
{

    public bool hasNextCombo = false;
    public string exitName = "";
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInteracting", true);
        
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!hasNextCombo)
        {
            animator.SetBool("isInteracting", false);
            animator.ResetTrigger(exitName);
        }
    }

}
