using System;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private static Game singleton;
    public static Game Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = new Game();
            }
            return singleton;
        }
    }

    private static SpBar _spBar;
    public static SpBar spBarSingle
    {
        get
        {
            if (_spBar == null)
            {
                _spBar = GameObject.Find("/Canvas/Panel/SpBar").gameObject.GetComponent<SpBar>();            
            }
            return _spBar;
        }
    }

    private static MpBar _mpBar;
    public static MpBar mpBarSingle
    {
        get
        {
            if (_mpBar == null)
            {
                _mpBar = GameObject.Find("/Canvas/Panel/MpBar").gameObject.GetComponent<MpBar>();
            }
            return _mpBar;
        }
    }

    private static HpBar _hpBar;
    public static HpBar hpBarSingle
    {
        get
        {
            if (_hpBar == null)
            {
                _hpBar = GameObject.Find("/Canvas/Panel/HpBar").gameObject.GetComponent<HpBar>();
            }
            return _hpBar;
        }
    }

    private static PlayerAttribute _playerAttr;
    public static PlayerAttribute playerAttrSingle
    {
        get
        {
            if (_playerAttr == null)
            {
                _playerAttr = new PlayerAttribute();
                Debug.Log("new player");
            }
            return _playerAttr;
        }
    }

    private static Plasterer _plasterer;
    public static Plasterer plastererSingle
    {
        get
        {
            if (_plasterer == null)
            {
                _plasterer = GameObject.Find("/Controllers/Plasterer").gameObject.GetComponent<Plasterer>();
            }
            return _plasterer;
        }
    }
    
    private static CacheObjectController _cacheObjectController;
    public static CacheObjectController cacheObjectControllerSingle
    {
        get
        {
            if (_cacheObjectController == null)
            {
                _cacheObjectController = GameObject.Find("/Controllers/CacheObjectController").gameObject.GetComponent<CacheObjectController>();
            }
            return _cacheObjectController;
        }
    }

    private static QuickItemBar _quickItemBar;
    public static QuickItemBar quickItemBarSingle
    {
        get
        {
            if (_quickItemBar == null)
            {
                _quickItemBar = GameObject.Find("/Controllers/QuickItemBar").gameObject.GetComponent<QuickItemBar>();
            }
            return _quickItemBar;
        }
    }

    private static BossAttribute _bossAttribute = null;
    public static BossAttribute bossAttribute
    {
        get
        {
            if (_bossAttribute == null)
            {
                _bossAttribute = GameObject.FindGameObjectWithTag("Boss").gameObject.GetComponent<BossAttribute>();
            }
            return _bossAttribute;
        }
    }

    private static Camera _mainCamera = null;
    public static Camera mainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();
            }
            return _mainCamera;
        }
    }



    public Transform nearbyEnemy;
    public bool isLockOn;
    public List<Transform> attackedEnemies = new List<Transform>(10);
    /*
    float gameMaxHp = 1000f;
    float gameMaxMp = 500f;
    float gameMaxSp = 200f;
    */

    public int enemyLayer = 1 << 9 | 1 << 10;
    public int playerLayer = 1 << 8;
    public int dafultLayer = 1 << 0;
    public int interactableEnvObjLayer = 1 << 18;

    public static readonly Vector3 vectorTenThousands = new Vector3(10000, 10000, 10000);

    public static bool IsPlayerAttacking()
    {
        string[] names = new string[11] { "LightAttack1", "LightAttack2", "LightAttack3", "LightAttack4", "LightAttack5", "LightAttack6", 
                                            "Charge", "ChargeAttack", "ChargeAttack2", "StabInChest", "StabInChest2" };
        bool result = false;

        AnimatorStateInfo stateInfo = Game.playerAttrSingle.animator.GetCurrentAnimatorStateInfo(0);
        for( int i = 0; i < names.Length; i++)
        {
            result = stateInfo.IsName(names[i]);
            if (result) break;
        }
        return result;
    }
    public static bool IsInFrontOfCamera(Vector3 otherPosition)
    {
        Vector3 direction = otherPosition - mainCamera.transform.position;
        if(Vector3.Angle(mainCamera.transform.forward, direction) < 60f)
        {
            return true;
        }
        else return false;
    }


}

/*
private static PotionContainer _potionContainer;
public static PotionContainer potionContainerSingle
{
    get
    {
        if (_potionContainer == null)
        {
            _potionContainer = GameObject.Find("/Controllers/PotionContainer").gameObject.GetComponent<PotionContainer>();
        }
        return _potionContainer;
    }
}
*/