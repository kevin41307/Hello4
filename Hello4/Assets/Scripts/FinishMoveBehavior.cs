using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMoveBehavior : StateMachineBehaviour
{
    public float transhold = 1;
    public float motionRate = 10f;
    public bool hasNextCombo = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("motionRate", motionRate);
        animator.SetBool("directDamage", true);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime <= transhold)
        {
            animator.SetBool("CanDodge", false);
            animator.SetBool("CanCancel", false);
            animator.SetBool("CanMove", false);

        }
        else
        {
            animator.SetBool("CanCancel", true);
            animator.SetBool("CanDodge", true);
            animator.SetBool("CanMove", true);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanDodge", true);
        if( !hasNextCombo )
        {
            animator.SetBool("directDamage", false);
            if (animator.GetFloat("motionRate") != 1f)
            {
                animator.SetFloat("motionRate", 1f);
            }

        }
            
    }


}

