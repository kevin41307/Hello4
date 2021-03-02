using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBehavior : StateMachineBehaviour
{
    public float transhold = 1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.spBarSingle.ChangeBarValue(new ChangeInfo(-20f));
        animator.transform.gameObject.SendMessage("InvinsibleBuff", SendMessageOptions.DontRequireReceiver);
        animator.SendMessage("StartInteractingWithEnv", SendMessageOptions.DontRequireReceiver);
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime <= transhold)
        {
            animator.SetBool("CanCancel", false);
            animator.SetBool("CanDodge", false);
        }
        else
        {
            animator.SetBool("CanCancel", true);
            animator.SetBool("CanMove", true);
            animator.SetBool("CanDodge", true);


        }
    
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanCancel", true);
        //animator.applyRootMotion = false; root motion 開開關關會有不能連續後退的問題
    }
}
