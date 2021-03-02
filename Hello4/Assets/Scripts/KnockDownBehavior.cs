using UnityEngine;

public class KnockDownBehavior : StateMachineBehaviour
{
    public float transhold = 1;
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
        animator.SetBool("CanCancel", true);
        animator.SetBool("CanDodge", true);
        animator.SetBool("CanMove", true);
        animator.transform.gameObject.SendMessage("EndInvinsibleBuff", SendMessageOptions.DontRequireReceiver);
    }
}
