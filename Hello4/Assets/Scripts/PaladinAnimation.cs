using UnityEngine;


public class PaladinAnimation : MonoBehaviour
{
    Animator animator;
    Transform player;
    public State currentState { get; set; }

    public enum State
    {
        Idle,
        Walk,
        Run,
        Chase,
        Observe,
        Attack,
        Damaged,
        Dead
    }

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        player = Game.playerAttrSingle.player.transform;
    }

    void Play_Walk()
    {
        animator.SetFloat("speedPercent", 1f);
    }
    void Stop_Walk()
    {
        animator.SetFloat("speedPercent", 0f);
    }

    void Play_Idle()
    {
        animator.SetFloat("speedPercent", 0f);
        animator.SetFloat("lockOn", 0f);
    }
    void Stop_Idle()
    {
        animator.SetFloat("speedPercent", 0f);

    }

    void Play_Chase()
    {
        if (animator.GetFloat("lockOn") == 0f)
            animator.Play("Equip");

        animator.SetFloat("lockOn", 1f);

    }

    void Stop_Chase()
    {
        Debug.Log("Stop_Chase");
    }

    void Play_Attack()
    {
        //Debug.Log("Play_Attack");
        if (animator.GetFloat("lockOn") == 0f)
            animator.Play("Equip");

        animator.SetFloat("lockOn", 1f);
        //animator.SetBool("isInteracting", true);
    }

    void Stop_Attack()
    {
        //Debug.Log("Stop_Attack");
        //animator.SetBool("isInteracting", false);
    }

    public void Play_Skill(string name)
    {
        animator.SetBool(name, true);
    }

    void Play_Observe()
    {
        //Debug.Log("Play_Observe");
        if (animator.GetFloat("lockOn") == 0f)
            animator.Play("Equip");
        animator.SetFloat("lockOn", 1f);
    }

    void Stop_Observe()
    {
        //Debug.Log("Stop_Observe");
    }

    public void PlayHit()
    {
        if( !animator.GetBool("isInteracting") )
        {
            animator.SetTrigger("Hit");
        }
            
    }

    public void PlayHit2()
    {
        if (!animator.GetBool("isInteracting"))
        {
            animator.SetTrigger("Hit2");
        }

    }

    public void PlayDodge()
    {
        animator.SetBool("Dodge", true);
    }


    private void Start()
    {
        currentState = State.Idle;
    }

    public void ChangeAnimation(State nextState)
    {
        if (currentState == nextState) return;
        switch (currentState)
        {
            case State.Idle:
                Stop_Idle();
                break;
            case State.Walk:
                Stop_Walk();
                break;
            case State.Chase:
                Stop_Chase();
                break;
            case State.Observe:
                Stop_Observe();
                break;

            case State.Attack:
                Stop_Attack();
                break;
            case State.Run:
                animator.SetBool("Run", false);
                break;
            case State.Damaged:
                animator.SetBool("Damaged", false);
                break;

            case State.Dead:
                animator.SetBool("Dead", false);
                break;
            default:
                Debug.Log("no this state in paladinAnimation");
                break;
        }

        currentState = nextState;
        switch (currentState)
        {
            case State.Idle:
                Play_Idle();
                break;
            case State.Walk:
                Play_Walk();
                break;
            case State.Chase:
                Play_Chase();
                break;
            case State.Observe:
                Play_Observe();
                break;
            case State.Attack:
                Play_Attack();
                break;
            case State.Run:
                animator.SetBool("Run", true);
                break;

            case State.Damaged:
                animator.SetBool("Damaged", true);
                break;
            case State.Dead:
                animator.SetBool("Dead", true);
                break;

            default:
                Debug.Log("no this state in paladinAnimation");
                break;
        }
    }


}
