using System.Collections;
using UnityEngine;

public class CounterAttackSystem : MonoBehaviour
{
    [HideInInspector]
    public bool isCounterAttackedPointRevealing = false;
    [HideInInspector]
    public bool isCounterAttackedPressing = false;

    Transform giver;
    [HideInInspector]
    public Transform receiver;

    [HideInInspector]
    public Transform myTransform;

    Animator myAnimator;
    Animator giverAnimator;
    Animator receiverAnimator;
    string counterAnimatioName_receiver = "CounterAttacked";
    string counterAnimatioNameSuccess_receiver = "CounterAttacked_Success";

    string counterAnimationName_giver = "StabInChest";

    float giverDistance = 1.7f;
    float revealtime = 0.8f;
    const float fadeTime = .1f;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    private void Start()
    {
        SetupTransform();
    }

    void SetupTransform()
    {
        myTransform = this.transform;
        giver = myTransform;
    }

    public void SetupAnimator()
    {
        receiverAnimator = receiver.GetComponent<Animator>();
        giverAnimator = myAnimator;
    }

    IEnumerator StartCounterAttackPointReveal()
    {
        isCounterAttackedPointRevealing = true;
        yield return new WaitForSeconds(revealtime);
        isCounterAttackedPointRevealing = false;
    }

    public void ConuterAttackPointBroken()
    {
        ResetAnimatorParameter();
        myAnimator.CrossFadeInFixedTime(counterAnimatioName_receiver, fadeTime); //20201117改成crossfade

    }

    IEnumerator StartCounterAttackPress()
    {
        isCounterAttackedPressing = true;
        yield return new WaitForSeconds(2f);
        isCounterAttackedPressing = false;

    }

    public void FinishMovePressSuccess()
    {
        SetupAnimator();
        giver.transform.position = receiver.position + receiver.forward * giverDistance;
        giverAnimator.CrossFadeInFixedTime(counterAnimationName_giver, fadeTime);
        receiverAnimator.CrossFadeInFixedTime(counterAnimatioNameSuccess_receiver, fadeTime);
    }

    public void FinishMovePressSuccess(Transform _receiver)
    {
        this.receiver = _receiver;
        SetupAnimator();
        giver.transform.position = receiver.position + receiver.forward * giverDistance;
        giverAnimator.CrossFadeInFixedTime(counterAnimationName_giver, fadeTime);
        receiverAnimator.CrossFadeInFixedTime(counterAnimatioNameSuccess_receiver, fadeTime);
    }

    public void ResetAnimatorParameter()
    {
        myAnimator.SendMessage("Attack_Off");
    }
}
