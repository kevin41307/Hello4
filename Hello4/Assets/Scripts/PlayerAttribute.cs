using UnityEngine;

public class PlayerAttribute 
{
    public PlayerAttribute()
    {
        Reset();
    }
    [HideInInspector]
    public float maxHp = 380f;
    [HideInInspector]
    public float maxMp = 125f;
    [HideInInspector]
    public float maxSp = 150f;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public float currentHp { set; get; }
    [HideInInspector]
    public float currentMp { set; get; }
    [HideInInspector]
    public float currentSp { set; get; }
    [HideInInspector]
    public float autoRecoverySpeed = 30f;

    [HideInInspector]
    public float attackDamage = 30f;

    public Inventory myBag;

    private GameObject _player;
    public GameObject player
    {
        get
        {
            if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
            return _player;
        }
    }

    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null) _animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
            return _animator;
        }
    }
    private PlayerBattle _playerBattle;

    [HideInInspector]
    public PlayerBattle playerBattle
    {
        get
        {
            if (_playerBattle == null) _playerBattle = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBattle>();
            return _playerBattle;
        }
    }

    private Transform _tranform;

    [HideInInspector]
    public Transform transform
    {
        get
        {
            if (_tranform == null) _tranform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            return _tranform;
        }
    }



    public void Reset()
    {
        currentHp = maxHp;
        currentMp = maxMp;
        currentSp = maxSp;
        attackDamage = 30f;
        isDead = false;
    }



}
