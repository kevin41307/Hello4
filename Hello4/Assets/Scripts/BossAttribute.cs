using UnityEngine;

public class BossAttribute : MonoBehaviour
{
    [HideInInspector]
    public string bossName = "";
    [HideInInspector]
    public float maxHp;
    [HideInInspector]
    public float currentHp { set; get; }
    [HideInInspector]
    public bool isAlive = true;
    [HideInInspector]
    public bool firstInDead = false;
    public AudioClip attackAudio;

    [HideInInspector]
    public float atk;

    private GameObject _boss;
    [HideInInspector]
    public GameObject boss
    {
        get
        {
            if (_boss == null) _boss = this.GetComponent<GameObject>();
            return _boss;
        }
    }

    private Animator _animator;
    [HideInInspector]
    public Animator animator
    {
        get
        {
            if (_animator == null) _animator = this.GetComponent<Animator>();
            return _animator;
        }
    }

    private PaladinBattle _paladinBattle;

    [HideInInspector]
    public PaladinBattle paladinBattle
    {
        get
        {
            if (_paladinBattle == null) _paladinBattle = this.GetComponent<PaladinBattle>();
            return _paladinBattle;
        }
    }

    private void OnEnable()
    {
        Reset();
    }


    public void Reset()
    {
        bossName = "Paladin";
        atk = 20f;
        maxHp = 2031f;
        currentHp = maxHp;
        isAlive = true;

    }
}
