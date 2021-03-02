using System.Collections;
using UnityEngine;


public class AI_AttackBehavior : StateMachineBehaviour
{
    public bool applyRootMotion;
    public string exitName = "";
    public bool hasNextCombo = false;
    public bool aimSupport = false;
    public float motionRate = 1f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInteracting", true);
        if (applyRootMotion)
            animator.applyRootMotion = true;      
        if (aimSupport)
            animator.SetBool("aimSupport", true);

        animator.SetFloat("motionRate", motionRate);
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        if (applyRootMotion)
            animator.applyRootMotion = false;
        */
        
        if(!hasNextCombo)
        {
            animator.SetBool("isInteracting", false);
            animator.SetBool(exitName, false);
            animator.applyRootMotion = true;
            animator.SetFloat("motionRate", 1f);
        }

        if (aimSupport)
            animator.SetBool("aimSupport", false);
    }

}
