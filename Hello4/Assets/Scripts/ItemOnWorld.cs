using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    void AddNewItem()
    {
        if(!playerInventory.itemList.Contains(thisItem))
        {
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
            }

            thisItem.itemHeld += 1;
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        InventoryManager.RefreshItem();

    }


}
