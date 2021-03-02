using UnityEngine;

public class JumpAttackBehavior : StateMachineBehaviour
{
    public AnimationCurve curveH;
    public AnimationCurve curveV;
    float acceForceH = 16f;
    [Range(0,20)] public float acceForceV = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("acceSupport", true);
        animator.SetFloat("acceForceH", acceForceH);
        animator.SetFloat("acceForceV", acceForceV);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetFloat("acceHorizontalDuration", curveH.Evaluate(stateInfo.normalizedTime));
        animator.SetFloat("acceVerticalDuration", curveV.Evaluate(stateInfo.normalizedTime));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("acceSupport", false);
        animator.SetFloat("acceForceH", 0);
        animator.SetFloat("acceForceV", 0);
        animator.SetBool("Attack_Jump", false);
        
    }
}
