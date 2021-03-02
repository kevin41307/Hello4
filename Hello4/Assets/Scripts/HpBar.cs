using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider lifePoint;
    public Slider hurt;
    PlayerAttribute pla = Game.playerAttrSingle;
    float recoveryTimer = 3.5f;
    [HideInInspector]
    public bool recoveryBuff = false;
    float delayGetHurtTimer = 10f;
    bool descending = false;


    private void Awake()
    {
        lifePoint = GetComponent<Slider>();
        hurt = hurt.GetComponent<Slider>();
        //lifePoint.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        lifePoint.value = pla.currentHp / pla.maxHp;
        hurt.value = lifePoint.value;
    }


    public void AttackToRecoveryHp()
    {
        if (recoveryBuff && (hurt.value > lifePoint.value))
        {
            float delta = 0.33f * (hurt.value - lifePoint.value) * pla.maxHp;
            
            ChangeInfo info = new ChangeInfo(delta, false);
            ChangeBarValue(info);
        }
    }

    void ResetDelayGetHurtTimer()
    {
        delayGetHurtTimer = 2f;
    }

    public void ChangeBarValue(ChangeInfo info)
    {
        pla.currentHp = Mathf.Clamp( pla.currentHp + info.delta, 0 , pla.maxHp);
        lifePoint.value = pla.currentHp / pla.maxHp;

        if (info.recovery == false)
        {
            hurt.value = lifePoint.value;
        }

        if( hurt.value > lifePoint.value )
        {
            if (info.recovery && !recoveryBuff)
            {
                StartCoroutine(StartRecoveryBuff());
            }


            StartCoroutine(StartDescending());
        }

        //Debug.Log(" pla.currentHp" + pla.currentHp + "info.delta" + info.delta);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            //接受傷害
            ChangeInfo info = new ChangeInfo(-100f);
            info.murderer = Game.playerAttrSingle.animator.transform;
            //info.hitFromPosition = Game.bossAttribute.transform.position;
            Game.playerAttrSingle.playerBattle.TakeDamage(info);

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AttackToRecoveryHp();
        }
    }


    IEnumerator StartRecoveryBuff()
    {
        recoveryBuff = true;
        //Debug.Log("buff");
        yield return new WaitForSeconds(recoveryTimer);
        float t = 0f;
        while (Mathf.Abs(hurt.value - lifePoint.value) >= 0.001f)
        {
            t += 0.5f * Time.deltaTime;
            hurt.value = Mathf.Lerp(hurt.value, lifePoint.value, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        recoveryBuff = false;

    }

    IEnumerator StartDescending()
    {
        descending = true;
        yield return new WaitForSeconds(delayGetHurtTimer);
        float t = 0f;
        while (Mathf.Abs(hurt.value - lifePoint.value) >= 0.001f)
        {
            t += 0.75f * Time.deltaTime;
            hurt.value = Mathf.Lerp(hurt.value, lifePoint.value, t);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        hurt.value = lifePoint.value;
        descending = false;
    }


}
