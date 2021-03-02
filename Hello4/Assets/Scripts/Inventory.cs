using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Inventory", menuName ="Inventory/New Inventory")]
[System.Serializable]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
    public int money = 0;
}
