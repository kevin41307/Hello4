using UnityEngine;

public class EnemyAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public float maxHp;
    [HideInInspector]
    public float currentHp;
    [HideInInspector]
    public bool isAlive = true;
    [HideInInspector]
    public bool firstInDead = false;
    public AudioClip attackAudio;
    [HideInInspector]
    public string enemyName;


    [HideInInspector]
    public float attackDamage;

    public virtual void Start()
    {
        Reset();
    }
    public virtual void Reset()
    {
        attackDamage = 5f;
        maxHp = 555f;
        currentHp = maxHp;
        isAlive = true;
    }

}
