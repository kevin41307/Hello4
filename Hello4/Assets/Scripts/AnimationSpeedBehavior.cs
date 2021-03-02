using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedBehavior : StateMachineBehaviour
{
    public AnimationCurve curve;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.speed = 1f;
        animator.SetFloat("animationSpeed", 1);
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.speed = curve.Evaluate(stateInfo.normalizedTime);
        animator.SetFloat("animationSpeed", curve.Evaluate(stateInfo.normalizedTime));
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.speed = 1;
        animator.SetFloat("animationSpeed", 1);
    }
}
