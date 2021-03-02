using System.Collections;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    EnemyAttribute enemyAttribute;
    [HideInInspector]
    public bool getDamaged = false;
    [HideInInspector]
    public bool hitRecover = false;

    [HideInInspector]
    public Vector3 damagedDirection;
    [HideInInspector]
    public EnemyHpBar enemyHpBar;


    

    public virtual void Awake()
    {
        enemyAttribute = this.GetComponent<EnemyAttribute>();
        enemyHpBar = this.GetComponentInChildren<EnemyHpBar>();
    }

    private void Start()
    {
        
    }
    public virtual void TakeDamage(ChangeInfo info)
    {
        enemyHpBar.ChangeBarValue(info);
        if (enemyAttribute.isAlive)
        {
            getDamaged = true;

            damagedDirection = Game.playerAttrSingle.player.transform.position - this.transform.position;
            damagedDirection = damagedDirection.normalized;
        }
    }

    /*
    public virtual void TakeDamage(ChangeInfo info, Vector3 shootPosition)
    {
        enemyHpBar.ChangeBarValue(info);
        if (enemyAttribute.isAlive)
        {
            getDamaged = true;         
            damagedDirection = shootPosition - this.transform.position;
            damagedDirection = damagedDirection.normalized;
        }
    }

    */

    public void OnHit()
    {
        Debug.Log("OnHit");

    }
    IEnumerator BackToNormal()
    {

        for (float t = 0; t <= 1f; t += 0.1f)
        {
            //mr.material.color = Color.Lerp(Color.red, Color.white, t);
            yield return new WaitForSeconds(0.1f);
        }

    }

}
