using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegate : MonoBehaviour
{
    public GameObject attackPointObj;
    public virtual void Attack_On()
    {
        attackPointObj.SetActive(true);
    }

    public virtual void Attack_Off()
    {
        attackPointObj.SetActive(false);
    }

}
