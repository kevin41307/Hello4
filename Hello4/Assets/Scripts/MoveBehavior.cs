using UnityEngine;

public class MoveBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanDodge", true);
        animator.SetBool("CanCancel", true);
        animator.SetBool("CanMove", true);
        animator.SetFloat("motionRate", 1f);
        animator.SetInteger("LightAttack", 0);
        animator.SetInteger("strongAttack", 0);
        animator.SetBool("chargeFull", false);
        animator.speed = 1f;
    }


}
