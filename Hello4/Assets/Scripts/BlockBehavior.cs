using UnityEngine;

public class BlockBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SendMessage("StartJustGuardBuff", SendMessageOptions.DontRequireReceiver);
    }
}
