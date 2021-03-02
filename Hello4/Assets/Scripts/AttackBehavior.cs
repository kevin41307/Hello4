using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    public float transhold = 1;
    public float dodgeHold = 1;
    float countHold = 0.2f;
    public int order = 0;
    public float sp = 0f;
    public float motionRate = 1f;
    public bool hasNextCombo = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.spBarSingle.ChangeBarValue(new ChangeInfo(-sp));
        animator.SetFloat("motionRate", motionRate);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime <= transhold)
        {
            animator.SetBool("CanMove", false);

        }
        else
        {
            animator.SetBool("CanMove", true);
        }

        if (stateInfo.normalizedTime <= dodgeHold)
        {
            animator.SetBool("CanDodge", false);
            animator.SetBool("CanCancel", false);
        }
        else
        {
            animator.SetBool("CanCancel", true);
            animator.SetBool("CanDodge", true);
        }

        if (stateInfo.normalizedTime <= countHold )
        {
            animator.SetInteger("LightAttack", order);
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("LightAttack", 0);
        animator.SetBool("CanDodge", true);
        if(!hasNextCombo)
        {
            if (animator.GetFloat("motionRate") != 1f)
            {
                animator.SetFloat("motionRate", 1f);
            }
        }

    }

}
