using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprint : MonoBehaviour
{
    bool footUp = false;
    bool footDown = false;
    Animator animator;
    BoxCollider boxCollider;
    AnimatorStateInfo stateInfo;
    private void Awake()
    {
        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.24f, 0.24f, 0.24f);
        boxCollider.isTrigger = true;
    }
    public virtual void Start()
    {
        animator = Game.playerAttrSingle.animator;
    }


    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log();
        if (footUp && other.gameObject.layer ==LayerMask.NameToLayer("Player"))
        {
            footUp = false;
            //stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if(true)
            {
                Game.playerAttrSingle.transform.SendMessage("PlayFootsteps", SendMessageOptions.DontRequireReceiver);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            footUp = true;
        }
    }
}
