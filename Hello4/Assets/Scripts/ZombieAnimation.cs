using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    public State currentState { get; set; }
    Transform player;
    public enum State
    {
        Idle,
        Walk,
        Run,
        Chase,
        Attack,
        Damaged,
        Dead

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        currentState = State.Idle;
    }

    void Play_Damaged()
    {
        animator.Play("Z_Damaged");
    }

    public void ChangeAnimation( State nextState)
    {
        if (currentState == nextState) return;
        switch(currentState)
        {
            case State.Attack:
                animator.SetBool("Attack", false);
                break;
            case State.Chase:
                animator.SetBool("Chase", false);
                break;
            case State.Run:
                animator.SetBool("Run", false);
                break;
            case State.Walk:
                animator.SetBool("Walk", false);
                break;
            case State.Idle:
                animator.SetBool("Idle", false);
                break;
            case State.Damaged:
                animator.SetBool("Damaged", false);
                break;

            case State.Dead:
                animator.SetBool("Dead", false);
                break;
            default:
                Debug.Log("no this state in zombieAnimation");
                break;
        }

        currentState = nextState;
        switch (currentState)
        {
            case State.Idle:
                animator.SetBool("Idle", false);
                break;
            case State.Attack:
                animator.SetBool("Attack", true);
                break;
            case State.Chase:
                animator.SetBool("Chase", true);
                break;
            case State.Run:
                animator.SetBool("Run", true);
                break;
            case State.Walk:
                animator.SetBool("Walk", true);
                break;
            case State.Damaged:
                animator.SetBool("Damaged", true);
                Play_Damaged();
                break;
            case State.Dead:
                animator.SetBool("Dead", true);
                break;

            default:
                Debug.Log("no this state in zombieAnimation");
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
