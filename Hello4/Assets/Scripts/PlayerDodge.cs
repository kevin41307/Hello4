using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    CharacterAnimation player_Anim;
    Animator anim;
    AnimatorStateInfo aniStateInfo;
    bool interactingWithEnv = false;
    const float sphereRadius = .6f;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        aniStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(Input.GetKeyDown(KeyCode.X) && CanCancel() )
        {
            if (!anim.GetBool("CanDodge")) return;
            anim.applyRootMotion = true;
            anim.Play("Default", 1);

            if (anim.GetFloat("LockOn") == 0f)
            {
                anim.SetFloat("DodgeForce", anim.GetFloat("direction"));

                if (anim.GetFloat("direction") == 0)
                {
                    //anim.SetBool("CanMove", false);
                    anim.SetBool("CanDodge", false);
                    //anim.Play("StepBack");
                    //anim.Play("Roll");
                    anim.CrossFadeInFixedTime(PlayerAnimationTags.BaseLayer + ".Roll", 0.08f);
                }
                else
                {
                    anim.SetBool("CanDodge", false);

                    //anim.Play("Roll");
                    anim.CrossFadeInFixedTime(PlayerAnimationTags.BaseLayer + ".Roll", 0.08f);
                }
                
            }
            else if (anim.GetFloat("LockOn") == 1f )
            {
                anim.SetFloat("DodgeX", anim.GetFloat("X"));
                anim.SetFloat("DodgeY", anim.GetFloat("Y"));
                anim.SetBool("CanMove", false);
                anim.SetBool("CanDodge", false);
                anim.CrossFadeInFixedTime( PlayerAnimationTags.BaseLayer + ".LockAndRoll", 0.08f);
            }
            anim.SetBool("CanCancel", false);
        }
        
    }

    private void FixedUpdate()
    {
        if (interactingWithEnv)
        {
            DetectInteractableEnvObj();
        }
    }

    void DetectInteractableEnvObj()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, sphereRadius, Game.Singleton.interactableEnvObjLayer);
        if (hit.Length > 0)
        {
            Debug.Log("DetectInteractableEnvObj success");
            for( int i = 0; i < hit.Length; i++)
            {
                InteractableEnvObj interactableEnvObj = hit[i].transform.GetComponent<InteractableEnvObj>();
                if( interactableEnvObj != null )
                {
                    if(interactableEnvObj.interactableType == InteractableEnvObj.InteractableType.Destoryable)
                    {
                        hit[i].SendMessage("ChangeDurability", new ChangeInfo(-10f));
                    }
                }
            }
        }

    }

    bool CanCancel()
    {
        return anim.GetBool("CanCancel");
    }

    private void Reset()
    {
        anim.SetFloat("DodgeForce", 0);
        anim.SetFloat("DodgeX", 0);
        anim.SetFloat("DodgeY", 0);
        anim.SetBool("CanCancel", true);
    }


    IEnumerator StartInteractingWithEnv()
    {
        interactingWithEnv = true;
        yield return new WaitForSeconds(1f);
        interactingWithEnv = false;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(this.transform.position, sphereRadius);
    }
}
