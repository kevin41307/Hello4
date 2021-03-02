using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    /*
    public void LightAttack()
    {
        anim.SetInteger("LightAttack", anim.GetInteger("LightAttack") +1 );
    }

    public void StrongAttack()
    {
        anim.SetInteger("strongAttack", anim.GetInteger("strongAttack") + 1);
    }
    */

} // class











































