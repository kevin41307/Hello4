
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]

[System.Serializable]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemHeld;
    [TextArea]
    public string itemInfo;

    public bool equipable;
    public bool addInQuickItemList;

}
