using System.Collections;
using UnityEngine;

public class CharacterAnimationDelegate : MonoBehaviour
{
    Animator animator;

    public GameObject playerAttackPoint;
    public GameObject playerAttackPointWithEnv;
    public GameObject playerAttackPoint_LeftFoot;
    public GameObject playerAttackTrail;
    public SkinnedMeshRenderer playerRenderer1;
    public SkinnedMeshRenderer playerRenderer2;

    public Material rimColorMat_Default;
    Material[] playerClothMaterial;
    Material[] playerClothMaterial2;
    Material rimColorMat;
    Material rimColorMat2;
    Color rimColor;
    bool isFadeOutRimColoring = false;

    [Header("EquipWeapon")]
    public GameObject inventory;
    public GameObject inHand;

    [Header("Light")]
    public Light whiteLight;
    public Light warmLight;
    public Light alarmLight;
    public Light characterLight;
    Coroutine co;
    Coroutine co_StartFadeOutRimColor;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rimColor = rimColorMat_Default.GetColor("mColor");

        rimColorMat = playerRenderer1.materials[1];
        rimColorMat2 = playerRenderer2.materials[1];

        Reset();
    }

    private void Reset()
    {
        rimColorMat.SetColor("mColor", Color.black);
        rimColorMat2.SetColor("mColor", Color.black);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            if (isFadeOutRimColoring)
            {
                if(co_StartFadeOutRimColor != null)
                    StopCoroutine(co_StartFadeOutRimColor);
                isFadeOutRimColoring = false;
            }
                
            co_StartFadeOutRimColor = StartCoroutine(StartFadeOutRimColor());
        }
    }
    IEnumerator StartFadeOutRimColor()
    {
        isFadeOutRimColoring = true;
        rimColorMat.SetColor("mColor", rimColor);
        rimColorMat2.SetColor("mColor", rimColor);
        Color color = rimColor;
        while (color.r > 1f)
        {
            rimColorMat.SetColor("mColor", rimColorMat.GetColor("mColor") * 0.9f);
            rimColorMat2.SetColor("mColor", rimColorMat.GetColor("mColor") * 0.9f);

            yield return new WaitForSeconds(0.08f);
            color = rimColorMat.GetColor("mColor");
        }
        rimColorMat.SetColor("mColor", Color.black);
        rimColorMat2.SetColor("mColor", Color.black);
        isFadeOutRimColoring = false;
    }

    public void Attack_On()
    {
        playerAttackPoint.SetActive(true);
        playerAttackPointWithEnv.SetActive(true);
        AttackTrail_On();
        CharacterLightOn();
        Game.Singleton.attackedEnemies.Clear();

    }

    public void Attack_Off()
    {
        playerAttackPoint.SetActive(false);
        playerAttackPointWithEnv.SetActive(false);
        AttackTrail_Off();
        Game.Singleton.attackedEnemies.Clear();
    }

    public void AttackTrail_On()
    {
        if (playerAttackTrail != null)
        {
            playerAttackTrail.SetActive(true);
        }
    }

    public void AttackTrail_Off()
    {
        if(playerAttackTrail != null )
        {
            playerAttackTrail.SetActive(false);
        }   
    }

    public void CharacterLightOn()
    {


        if(characterLight != null)
        {
            if(co != null )
                StopCoroutine(co);       
            co = StartCoroutine(StartCharacterLight());
        }


    }
    public void CharacterLightOff()
    {
        if (characterLight != null)
        {
            characterLight.gameObject.SetActive(false);
        }

    }


    public void LeftFoot_Attack_On()
    {
        playerAttackPoint_LeftFoot.SetActive(true);
    }

    public void LeftFoot_Attack_Off()
    {
        playerAttackPoint_LeftFoot.SetActive(false);
    }

    public void Equip()
    {
        inventory.SetActive(false);
        inHand.SetActive(true);
    }

    public void DisArm()
    {
        inventory.SetActive(true);
        inHand.SetActive(false);
    }
    
    public void Alarm()
    {
        StartCoroutine(StartAlarmLight());
    }

    IEnumerator StartAlarmLight()
    {
        alarmLight.gameObject.SetActive(true);
        float intensity = 5f;
        yield return new WaitForSeconds(0.8f);
        alarmLight.intensity = intensity;
        alarmLight.gameObject.SetActive(false);
    }

    void PlayChargeAbsorbVFX()
    {
        Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.chargeAbsorbVFX, playerAttackPoint.transform.position, Quaternion.identity.eulerAngles);

    }
    void PlayChargeAbsorb_p2VFX()
    {

        Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.chargeAbsorb_p2VFX, transform.position, Quaternion.identity.eulerAngles);
    }


    void PlayChargeFullVFX()
    {
        animator.Play("ClothLayer.Knight@ClothWave");
        Game.cacheObjectControllerSingle.BorrowAsset(CacheObjectController.ObjType.chargeFullVFX, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up).eulerAngles);
    }

    /*
    IEnumerator WhiteLight()
    {

        whiteLight.gameObject.SetActive(true);
        float intensity = 4f;
        while (whiteLight.intensity >= 0.5f)
        {
            whiteLight.intensity = Mathf.Lerp(whiteLight.intensity, 0, 3f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);

        }
        whiteLight.intensity = intensity;
        whiteLight.gameObject.SetActive(false);
    }
    */


    IEnumerator WarmLight()
    {
        warmLight.gameObject.SetActive(true);
        float intensity = 6f;
        while (warmLight.intensity >= 0.5f)
        {
            warmLight.intensity = Mathf.Lerp(warmLight.intensity, 0, 5f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);

        }
        warmLight.intensity = intensity;
        warmLight.gameObject.SetActive(false);
    }

    IEnumerator StartCharacterLight()
    {
        characterLight.gameObject.SetActive(true);
        //Debug.Log("aaa");
        float intensity = 1f;
        characterLight.intensity = intensity;
        while (characterLight.intensity >= 0.05f)
        {
            characterLight.intensity = Mathf.Lerp(characterLight.intensity, 0,  0.75f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);

        }
        characterLight.intensity = intensity;
        characterLight.gameObject.SetActive(false);
    }

}
