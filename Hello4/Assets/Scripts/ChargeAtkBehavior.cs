using UnityEngine;

public class ChargeAtkBehavior : StateMachineBehaviour
{
    public float transhold = 1;
    public float countHold = 1;
    public float sp = 0f;
    float motionRate = 1f;
    public int order = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Game.spBarSingle.ChangeBarValue(new ChangeInfo(-sp));
        if (animator.GetBool("chargeFull"))
        {
            motionRate = 6f;
        }
        else
            motionRate = 2f;
        animator.SetFloat("motionRate", motionRate);
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


        if(stateInfo.normalizedTime <= countHold)
        {
            animator.SetInteger("strongAttack", order);
        }
        
       
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("CanDodge", true);
        if (animator.GetFloat("motionRate") != 1f)
        {
            animator.SetFloat("motionRate", 1f);
        }
        animator.SetBool("chargeFull", false);
    }

}
