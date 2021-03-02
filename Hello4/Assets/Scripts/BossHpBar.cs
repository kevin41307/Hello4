using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    BossAttribute bossAttribute;
    PaladinBattle paladinBattle;
    public Transform bossHpBar;
    public Image hpImage;
    public Text damageValue;

    // Start is called before the first frame update
    public void Awake()
    {
        hpImage = hpImage.GetComponent<Image>();
        bossAttribute = this.GetComponent<BossAttribute>();
        damageValue = damageValue.GetComponent<Text>();
        paladinBattle = this.GetComponent<PaladinBattle>();
    }

    public void Start()
    {
        hpImage.fillAmount = bossAttribute.currentHp / bossAttribute.maxHp;
        Debug.Log("bossHp" + bossAttribute.currentHp);
        Debug.Log("bossMaxHp" + bossAttribute.maxHp);

        //bossHpBar.gameObject.SetActive(false);
    }

    public void ChangeBarValue(ChangeInfo info)
    {
        //Debug.Log("info.delta " + info.delta + "bossAttribute.currentHp" + bossAttribute.currentHp);
        bossAttribute.currentHp = Mathf.Clamp(bossAttribute.currentHp + info.delta, 0, bossAttribute.maxHp);
        hpImage.fillAmount = bossAttribute.currentHp / bossAttribute.maxHp;
        string s = Mathf.Abs( (int) info.delta).ToString();
        damageValue.text = s;

        if (bossAttribute.currentHp <= 0)
        {
            bossAttribute.isAlive = false;
            bossAttribute.firstInDead = true;
            bossAttribute.isAlive = false;
        }
    }


}
