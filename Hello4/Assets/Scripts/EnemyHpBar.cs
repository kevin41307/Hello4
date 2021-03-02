using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    Camera mainCamera;
    EnemyAttribute enemyAttribute;
    public Image hpImage;

    public virtual void Awake()
    {
        enemyAttribute = GetComponentInParent<EnemyAttribute>();
        hpImage = hpImage.GetComponent<Image>();
        mainCamera = Camera.main;
    }

    public void OnEnable()
    {
        hpImage.fillAmount = enemyAttribute.currentHp / enemyAttribute.maxHp;
    }


    public virtual void Start()
    {
        //enemyAttribute.Reset();
        //Debug.Log(enemyAttribute.currentHp + "" + enemyAttribute.maxHp);
        hpImage.fillAmount = enemyAttribute.currentHp / enemyAttribute.maxHp;

    }

    public virtual void LateUpdate()
    {
        Vector3 direction = mainCamera.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(direction.normalized);
        //Debug.Log(rot);
        //rot.y = 0;
        
        this.transform.rotation = rot;
    }


    public virtual void ChangeBarValue(ChangeInfo info)
    {
        enemyAttribute.currentHp = Mathf.Clamp(enemyAttribute.currentHp + info.delta, 0, enemyAttribute.maxHp);
        hpImage.fillAmount = enemyAttribute.currentHp / enemyAttribute.maxHp;
        if (enemyAttribute.currentHp <= 0)
        {
            enemyAttribute.isAlive = false;
            enemyAttribute.firstInDead = true;
        }
        
    }

    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }

    public void Appear()
    {
        this.gameObject.SetActive(true);
    }
}
