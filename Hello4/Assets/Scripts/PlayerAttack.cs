using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    private CharacterAnimationDelegate player_Anim;
    private void Awake()
    {
        player_Anim = GetComponentInChildren<CharacterAnimationDelegate>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //ComboAttacks();
        //ResetComboState();
    }

    void ComboAttacks()
    {

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            //player_Anim.LightAttack();

        } // combo attacks



    }
}// class
































