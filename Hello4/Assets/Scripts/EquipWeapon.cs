using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    public GameObject inventory;
    public GameObject inHand;

    void Equip()
    {
        inventory.SetActive(false);
        inHand.SetActive(true);
    }

    void DisArm()
    {
        inventory.SetActive(true);
        inHand.SetActive(false);
    }
}
