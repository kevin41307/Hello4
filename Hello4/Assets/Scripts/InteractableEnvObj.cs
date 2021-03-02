using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEnvObj : MonoBehaviour
{
    public float maxDurability = 1;
    float currentDurability = 0;
    
    bool isOpen = false;
    public string animationClipName = "";
    public string talkSomething;

    public enum InteractableType
    {
        Destoryable,
        Openable
    }

    public InteractableType interactableType;

    private void Start()
    {
        currentDurability = maxDurability;
        this.gameObject.layer = LayerMask.NameToLayer("InteractableEnvObj");
        //Debug.Log("InteractableEnvObj: " + transform.name +" currentDurability: " + currentDurability);
    }

    void ChangeDurability(ChangeInfo info)
    {
        currentDurability += info.delta;
        if(currentDurability <= 0f)
        {
            SpawnFracture spawnFracture = this.GetComponent<SpawnFracture>();   
            if(spawnFracture != null)
            {
                spawnFracture.SpawnFractureObject();
            }
        }
    }

    void PlayAnimation(string name)
    {
        this.GetComponent<Animation>().Play(name);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E) && !isOpen)
        {
            PlayAnimation(animationClipName);
            isOpen = true;
            this.SendMessage("CloseTips", SendMessageOptions.DontRequireReceiver);
            TipsManager.instance.ShowTips(talkSomething);
        }
    }

}
