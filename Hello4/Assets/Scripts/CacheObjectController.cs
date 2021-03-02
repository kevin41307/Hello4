using System;
using UnityEngine;

public class CacheObjectController : MonoBehaviour
{
    public GameObject prefab_hit_blood_less;
    public GameObject prefab_hit_blood_more;
    public GameObject prefab_ShootExplosion;
    public GameObject prefab_GuardGlow_big;
    public GameObject prefab_GuardGlow_small;
    public GameObject prefab_SmokeSheet1;
    public GameObject prefab_HitSparks;
    public GameObject prefab_Smoke1;
    public GameObject prefab_Skeleton;
    public GameObject prefab_Burst;
    public GameObject prefab_Summon;
    public GameObject prefab_HealPotionVFX;
    public GameObject prefab_ChargeFullVFX;
    public GameObject prefab_ChargeAbsorbVFX;
    public GameObject prefab_ChargeAbsorb_p2VFX;
    public GameObject prefab_Bloodprint;
    public GameObject prefab_BloodDirection;




    public GameObject[] prefab_BloodCarpet;



    CachedObject[] hit_blood_less_cache = new CachedObject[10];
    CachedObject[] hit_blood_more_cache = new CachedObject[8];
    CachedObject[] shoot_explosion_cache = new CachedObject[6];
    CachedObject[] guardGlow_big_cache = new CachedObject[8];
    CachedObject[] guardGlow_small_cache = new CachedObject[8];
    CachedObject[] smokeSheet1_cache = new CachedObject[8];
    CachedObject[] hitSparks_cache = new CachedObject[8];
    CachedObject[] smoke1_cache = new CachedObject[8];
    CachedObject[] skeleton_cache = new CachedObject[2];
    CachedObject[] burst_cache = new CachedObject[2];
    CachedObject[] summon_cache = new CachedObject[10];
    CachedObject[] healPotionVFX_cache = new CachedObject[3];
    CachedObject[] chargeFullVFX_cache = new CachedObject[3];
    CachedObject[] chargeAbsorbVFX_cache = new CachedObject[3];
    CachedObject[] chargeAbsorb_p2VFX_cache = new CachedObject[3];
    CachedObject[] bloodprint_cache = new CachedObject[50];
    CachedObject[] bloodDirection_cache = new CachedObject[20];




    CachedObject[] bloodCarpet_cache = new CachedObject[35];


    public enum ObjType
    {
        hit_blood_less,
        hit_blood_more,
        shoot_explosion,
        guardGlow_big,
        guardGlow_small,
        smokeSheet1,
        smoke1,
        hitSparks,
        blood_carpet,
        skeleton,
        burst,
        summon,
        healPotionVFX,
        chargeFullVFX,
        chargeAbsorbVFX,
        chargeAbsorb_p2VFX,
        bloodprint,
        bloodDirection

    }

    private void Awake()
    {
        CreatePool();
    }

    private void OnEnable()
    {
       
    }

    private void Start()
    {
        
    }

    void CreatePool()
    {
        for (int i = 0; i < shoot_explosion_cache.Length; i++)
        {
            shoot_explosion_cache[i] = CachedObject.Create( Enum.GetName(typeof(ObjType), ObjType.shoot_explosion) ,prefab_ShootExplosion);
        }
        for (int i = 0; i < hit_blood_less_cache.Length; i++)
        {
            hit_blood_less_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.hit_blood_less), prefab_hit_blood_less);
        }
        for (int i = 0; i < hit_blood_more_cache.Length; i++)
        {
            hit_blood_more_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.hit_blood_more), prefab_hit_blood_more);
        }
        for (int i = 0; i < guardGlow_big_cache.Length; i++)
        {
            guardGlow_big_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.guardGlow_big), prefab_GuardGlow_big);
        }
        for (int i = 0; i < guardGlow_small_cache.Length; i++)
        {
            guardGlow_small_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.guardGlow_small), prefab_GuardGlow_small);
        }
        for (int i = 0; i < smokeSheet1_cache.Length; i++)
        {
            smokeSheet1_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.smokeSheet1), prefab_SmokeSheet1);
        }

        for (int i = 0; i < smokeSheet1_cache.Length; i++)
        {
            smoke1_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.smoke1), prefab_Smoke1);
        }

        for (int i = 0; i < hitSparks_cache.Length; i++)
        {
            hitSparks_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.hitSparks), prefab_HitSparks);
        }

        for (int i = 0; i < skeleton_cache.Length; i++)
        {
            skeleton_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.skeleton), prefab_Skeleton);
        }
        for (int i = 0; i < burst_cache.Length; i++)
        {
            burst_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.burst), prefab_Burst, ObjType.burst, i);
        }
        for (int i = 0; i < summon_cache.Length; i++)
        {
            summon_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.summon), prefab_Summon);
        }

        for (int i = 0; i < healPotionVFX_cache.Length; i++)
        {
            healPotionVFX_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.healPotionVFX), prefab_HealPotionVFX);
        }

        for (int i = 0; i < chargeFullVFX_cache.Length; i++)
        {
            chargeFullVFX_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.chargeFullVFX), prefab_ChargeFullVFX);
        }

        for (int i = 0; i < chargeAbsorbVFX_cache.Length; i++)
        {
            chargeAbsorbVFX_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.chargeAbsorbVFX), prefab_ChargeAbsorbVFX);
        }
        for (int i = 0; i < chargeAbsorb_p2VFX_cache.Length; i++)
        {
            chargeAbsorb_p2VFX_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.chargeAbsorb_p2VFX), prefab_ChargeAbsorb_p2VFX);
        }
        for (int i = 0; i < bloodprint_cache.Length; i++)
        {
            bloodprint_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.bloodprint), prefab_Bloodprint);
        }
        for (int i = 0; i < bloodDirection_cache.Length; i++)
        {
            bloodDirection_cache[i] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.bloodDirection), prefab_BloodDirection);
        }

        int x = 0;
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                x = i + j * 7;
                bloodCarpet_cache[x] = CachedObject.Create(Enum.GetName(typeof(ObjType), ObjType.blood_carpet), prefab_BloodCarpet[i]);
            }
        }


    }

    public CachedObject BorrowAsset(ObjType cachedObj, Vector3 pos, Vector3 euler)
    {
        CachedObject co = null;
        switch (cachedObj)
        {
            case ObjType.shoot_explosion:
                for (int i = 0; i < shoot_explosion_cache.Length; i++)
                {
                    if (shoot_explosion_cache[i].inUse == false)
                    {
                        shoot_explosion_cache[i].CreateObject(pos, euler);
                        co = shoot_explosion_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.hit_blood_less:
                for (int i = 0; i < hit_blood_less_cache.Length; i++)
                {
                    if (hit_blood_less_cache[i].inUse == false)
                    {
                        hit_blood_less_cache[i].CreateObject(pos, euler, 0.7f);
                        co = hit_blood_less_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.hit_blood_more:
                for (int i = 0; i < hit_blood_more_cache.Length; i++)
                {
                    if (hit_blood_more_cache[i].inUse == false)
                    {
                        hit_blood_more_cache[i].CreateObject(pos, euler, 2.1f);
                        co = hit_blood_more_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.blood_carpet:
                for (int i = 0; i < bloodCarpet_cache.Length; i++)
                {
                    if (bloodCarpet_cache[i].inUse == false)
                    {
                        bloodCarpet_cache[i].CreateObject(pos, euler, 60f);
                        co = bloodCarpet_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.guardGlow_big:
                for (int i = 0; i < guardGlow_big_cache.Length; i++)
                {
                    if (guardGlow_big_cache[i].inUse == false)
                    {
                        guardGlow_big_cache[i].CreateObject(pos, euler, 2f);
                        co = guardGlow_big_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.guardGlow_small:
                for (int i = 0; i < guardGlow_small_cache.Length; i++)
                {
                    if (guardGlow_small_cache[i].inUse == false)
                    {
                        guardGlow_small_cache[i].CreateObject(pos, euler, 1.5f);
                        co = guardGlow_small_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.smokeSheet1:
                for (int i = 0; i < smokeSheet1_cache.Length; i++)
                {
                    if (smokeSheet1_cache[i].inUse == false)
                    {
                        smokeSheet1_cache[i].CreateObject(pos, euler, 2.5f);
                        co = smokeSheet1_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.smoke1:
                for (int i = 0; i < smoke1_cache.Length; i++)
                {
                    if (smoke1_cache[i].inUse == false)
                    {
                        smoke1_cache[i].CreateObject(pos, euler, 1f);
                        co = smoke1_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.hitSparks:
                for (int i = 0; i < hitSparks_cache.Length; i++)
                {
                    if (hitSparks_cache[i].inUse == false)
                    {
                        hitSparks_cache[i].CreateObject(pos, euler, 2f);
                        co = hitSparks_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.skeleton:
                for (int i = 0; i < skeleton_cache.Length; i++)
                {
                    if (skeleton_cache[i].inUse == false)
                    {
                        skeleton_cache[i].CreateObject(pos, euler);
                        co = skeleton_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.burst:
                for (int i = 0; i < burst_cache.Length; i++)
                {
                    if (burst_cache[i].inUse == false)
                    {
                        burst_cache[i].CreateObject(pos, euler);
                        co = burst_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.summon:
                for (int i = 0; i < summon_cache.Length; i++)
                {
                    if (summon_cache[i].inUse == false)
                    {
                        summon_cache[i].CreateObject(pos, euler, 2f);
                        co = summon_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.healPotionVFX:
                for (int i = 0; i < healPotionVFX_cache.Length; i++)
                {
                    if (healPotionVFX_cache[i].inUse == false)
                    {
                        healPotionVFX_cache[i].CreateObject(pos, euler, 2f);
                        co = healPotionVFX_cache[i];
                        break;
                    }
                }
                break;
            case ObjType.chargeFullVFX:
                for (int i = 0; i < chargeFullVFX_cache.Length; i++)
                {
                    if (chargeFullVFX_cache[i].inUse == false)
                    {
                        chargeFullVFX_cache[i].CreateObject(pos, euler, 2f);
                        co = chargeFullVFX_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.chargeAbsorbVFX:
                for (int i = 0; i < chargeAbsorbVFX_cache.Length; i++)
                {
                    if (chargeAbsorbVFX_cache[i].inUse == false)
                    {
                        chargeAbsorbVFX_cache[i].CreateObject(pos, euler, 1f);
                        co = chargeAbsorbVFX_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.chargeAbsorb_p2VFX:
                for (int i = 0; i < chargeAbsorb_p2VFX_cache.Length; i++)
                {
                    if (chargeAbsorb_p2VFX_cache[i].inUse == false)
                    {
                        chargeAbsorb_p2VFX_cache[i].CreateObject(pos, euler, 1f);
                        co = chargeAbsorb_p2VFX_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.bloodprint:
                for (int i = 0; i < bloodprint_cache.Length; i++)
                {
                    if (bloodprint_cache[i].inUse == false)
                    {
                        bloodprint_cache[i].CreateObject(pos, euler, 30f);
                        co = bloodprint_cache[i];
                        break;
                    }
                }
                break;

            case ObjType.bloodDirection:
                for (int i = 0; i < bloodDirection_cache.Length; i++)
                {
                    if (bloodDirection_cache[i].inUse == false)
                    {
                        bloodDirection_cache[i].CreateObject(pos, euler, 1.7f);
                        co = bloodDirection_cache[i];
                        break;
                    }
                }
                break;


            default:
                Debug.Log("找不到這個cached obj type");
                break;

        }

        if(co == null)
        {
            Debug.Log("沒有剩餘的" + cachedObj.ToString() + " Cached Obj可以使用.");
        }

        return co;
    }
    public CachedObject BorrowAsset(ObjType cachedObj, Vector3 pos, Vector3 euler, Transform parent)
    {
        CachedObject co = BorrowAsset(cachedObj, pos, euler);
        co.MyParent(parent);
        return co;
    }

    public bool IsCachedRemained(ObjType objType)
    {
        bool exists = false;
        switch (objType)
        {
            case ObjType.skeleton:
                for (int i = 0; i < skeleton_cache.Length; i++)
                {
                    if (skeleton_cache[i].inUse == false)
                    {
                        exists = true;
                    }
                }
                break;
            default:
                Debug.Log("找不到這個cached obj type");
                break;
        }
        return exists;
    }

    public void GiveBackAsset(CachedObject cachedObject)
    {
        cachedObject.MyParent(null);
        cachedObject.DestroyAsset();
    }

    private void Update()
    {
        if(Time.frameCount % 30 == 0)
        {
            //Debug.Log("ColletGarbage");
            System.GC.Collect();
        }
    }

}
