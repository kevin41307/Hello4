using System.Collections;
using UnityEngine;

public class PaladinBattle : MonoBehaviour
{
    [HideInInspector]
    public bool hitRecover = false;
    float woundValue;
    BossHpBar bossHpBar;
    AIController_Paladin aIController_Paladin;
    BossAttribute bossAttribute;
    Animator animator;
    PaladinAnimation paladinAnimation;
    string[] hitExceptionAniNames = new string[] { "KnockDown", "FrontUp", "CounterAttacked", "KnockDown2", "CounterAttacked_Success" };
    AnimatorStateInfo stateInfo;

    float AtkBuffer;
    float playerInBack;
    Vector3 direction;


    private void Start()
    {
        woundValue = 0f;
        
    }

    private void Update()
    {
        if (woundValue >= 0f)
            woundValue -= 1f* Time.deltaTime;
        //Vector3 direction = Game.playerAttrSingle.player.transform.position - this.transform.position;
        //Debug.Log("angle" + Vector3.Angle(direction, this.transform.forward));
    }

    private void Awake()
    {
        bossHpBar = this.GetComponent<BossHpBar>();
        aIController_Paladin = this.GetComponent<AIController_Paladin>();
        bossAttribute = this.GetComponent<BossAttribute>();
        animator = this.GetComponent<Animator>();
        paladinAnimation = this.GetComponent<PaladinAnimation>();
    }

    

    public void TakeDamage(ChangeInfo info)
    {
        bossHpBar.ChangeBarValue(info);
        //Debug.Log(info.delta);
        if(!HitAniException()) //  !info.directDamage
        {
            playerInBack = 0f;
            direction = Game.playerAttrSingle.player.transform.position - this.transform.position;
            playerInBack = (Vector3.Angle(direction, -this.transform.forward) < 90f) ? 1 : -1;

            if( playerInBack == -1 && !info.hitByGun)
            {
                if (playerInBack * info.hitFromRightSide > 0)
                {
                    animator.Play("HitLeft");
                }
                else
                {
                    animator.Play("HitRight");
                }
            }
            else
            {
                animator.Play("Hit");
            }
            aIController_Paladin.OnHitted();


            /*
            woundValue += info.woundPower;
            if (woundValue >= 0f && woundValue < 100f)
                paladinAnimation.PlayHit();
            if (woundValue >= 100f && woundValue < 200f)
            {
                if (Random.Range(0, 2f) > 1f)
                {
                    paladinAnimation.PlayHit2();
                }
                else
                {
                    paladinAnimation.PlayHit();
                }
            }
            */
        }
    }

    public bool HitAniException()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool except = false;
        for(int i = 0; i < hitExceptionAniNames.Length; i++)
        {
            if( stateInfo.IsName(hitExceptionAniNames[i]))
            {
                except = true;
            }
        }
        return except;
    }


    public void BattleStart()
    {
        BossAttribute bossAttribute = this.GetComponent<BossAttribute>();
        if (bossAttribute != null)
        {
            //Game.bossAttribute = bossAttribute;
            bossAttribute.Reset();
        }
        else
        {
            Debug.Log("Cannot get bossAttr");
        }
    }

    public float GetAttackDamage()
    {
        //Debug.Log(-(attackDamage + Random.Range(attackDamage / 9, attackDamage / 10)) * animator.GetFloat("AttackPower"));
        float atk = bossAttribute.atk;
        AtkBuffer = -(atk + Random.Range(atk / 9, atk / 10)) * animator.GetFloat("motionRate");
        return AtkBuffer;
    }


}
