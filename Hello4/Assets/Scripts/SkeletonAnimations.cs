using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    public AniState currentState { get; set; }
    Transform player;
    public enum AniState
    {
        Stand,
        Idle,
        Walk,
        Run,
        Chase,
        Attack,
        Damaged,
        Skill,
        Dead

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = Game.playerAttrSingle.player.transform;
    }
    public void Reset()
    {
        ChangeAnimation(AniState.Stand);
    }

    private void Start()
    {
        ChangeAnimation(AniState.Stand);
    }

    void Play_Damaged()
    {
        animator.Play("Damage");
        ChangeAnimation(AniState.Damaged);
    }

    void Play_Stand()
    {
        animator.Play("Stand");
        ChangeAnimation(AniState.Stand);

    }

    public void ChangeAnimation(AniState nextState)
    {
        if (currentState == nextState) return;
        switch (currentState)
        {
            case AniState.Attack:
                animator.SetBool("Attack", false);
                break;
            case AniState.Chase:
                animator.SetBool("Chase", false);
                break;
            case AniState.Run:
                animator.SetBool("Run", false);
                break;
            case AniState.Walk:
                animator.SetBool("Walk", false);
                break;
            case AniState.Idle:
                animator.SetBool("Idle", false);
                break;
            case AniState.Damaged:
                animator.SetBool("Damaged", false);
                break;
            case AniState.Stand:
                animator.SetBool("Stand", false);
                break;
            case AniState.Skill:
                animator.SetBool("Skill", false);
                break;
            case AniState.Dead:
                animator.SetBool("Dead", false);
                break;
            default:
                Debug.Log("no " + nextState + " state in skeletonAnimation");
                break;
        }

        currentState = nextState;
        switch (currentState)
        {
            case AniState.Idle:
                animator.SetBool("Idle", true);
                break;
            case AniState.Attack:
                animator.SetBool("Attack", true);
                break;
            case AniState.Chase:
                animator.SetBool("Chase", true);
                break;
            case AniState.Run:
                animator.SetBool("Run", true);
                break;
            case AniState.Walk:
                animator.SetBool("Walk", true);
                break;
            case AniState.Damaged:
                animator.SetBool("Damaged", true);
                Play_Damaged();
                break;
            case AniState.Dead:
                animator.SetBool("Dead", false); //use ragdoll death animation instead;
                break;
            case AniState.Stand:
                animator.SetBool("Stand", true);
                break;
            case AniState.Skill:
                animator.SetBool("Skill", true);
                break;
            default:
                Debug.Log("no " + nextState +  " state in skeletonAnimation");
                break;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 pos = player.transform.position;
        pos.y += 1.6f;
        animator.SetLookAtPosition(pos);
        animator.SetLookAtWeight(1);
    }



}
