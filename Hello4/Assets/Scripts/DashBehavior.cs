using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : StateMachineBehaviour
{
    public AnimationCurve curve;
    float acceForceH = 30f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("acceSupport", true);
        animator.SetFloat("acceForceH", acceForceH);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetFloat("acceHorizontalDuration", curve.Evaluate(stateInfo.normalizedTime));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("acceSupport", false);
        animator.SetFloat("acceForceH", 0);
        animator.SetBool("Attack_Dash", false);
    }
}
