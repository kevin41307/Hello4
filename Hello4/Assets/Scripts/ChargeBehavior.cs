using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBehavior : StateMachineBehaviour
{
    public float transhold = 1f;
    float chargeTimer = 0f;
    const float chargeTime = 0.9f;
    bool chargeAbsorbHinting = false;
    bool chargeAbsorb_p2Hinting = false;
    bool chargeFullLightHinting = false;
 
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("ChargeAttack");
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

        if( animator.GetBool("chargeBtn"))
        {
            chargeTimer += Time.deltaTime;

            if (chargeTimer >= chargeTime/3 )
            {

                if (!chargeAbsorbHinting)
                {
                    animator.SendMessage("PlayChargeAbsorbVFX", SendMessageOptions.DontRequireReceiver);
                    chargeAbsorbHinting = true;
                }

            }

            if (chargeTimer >= chargeTime * 0.65f)
            {

                if (!chargeAbsorb_p2Hinting)
                {
                    animator.SendMessage("PlayChargeAbsorb_p2VFX", SendMessageOptions.DontRequireReceiver);
                    chargeAbsorb_p2Hinting = true;
                }

            }

            if (chargeTimer >= chargeTime)
            {
                animator.SetBool("chargeFull", true);
                
                if (!chargeFullLightHinting)
                {
                    animator.SendMessage("PlayChargeFullVFX", SendMessageOptions.DontRequireReceiver);
                    chargeFullLightHinting = true;
                    animator.SendMessage("StartFadeOutRimColor", SendMessageOptions.DontRequireReceiver);
                }
                    
            }
        }
        if ( !animator.GetBool("chargeBtn") )
        {
            animator.SetTrigger("ChargeAttack");
            chargeFullLightHinting = false;
            chargeTimer = 0f;
        }
        
        //Debug.Log("curve.Evaluate(stateInfo.normalizedTime)" + curve.Evaluate(stateInfo.normalizedTime));
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("chargeBtn", false);
        chargeFullLightHinting = false;
        chargeAbsorbHinting = false;
        chargeAbsorb_p2Hinting = false;
        chargeTimer = 0f;
    }
}
