using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory myBag;
    public GameObject slotGrid;
    //public Slot slotPrefab;
    public GameObject emptySlot;
    public Text itemInformation;
    public GameObject BagPanel;
    bool isOpen = false;
    public List<GameObject> slots = new List<GameObject>();
    void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInformation.text = "";
        
    }
    private void Start()
    {
        isOpen = BagPanel.activeSelf;
       
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchOpen();
        }
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInformation.text = itemDescription;
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            if (instance.myBag.itemList[i] != null && instance.myBag.itemList[i].itemHeld <= 0 )
            {
                instance.myBag.itemList[i] = null;
            }
            instance.slots[i].GetComponent<Slot>().slotID = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.myBag.itemList[i]);
        }

        Game.quickItemBarSingle.RefreshQuickItemBar();
        
    }

    public static bool IsItemInBag(string theItemName)
    {
        bool isExist = false;
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            if (instance.myBag.itemList[i] == null) break;
            if (instance.myBag.itemList[i].itemName.IndexOf(theItemName) >= 0)
            {
                isExist = true;
                break;
            }
        }
        return isExist;
    }


    public static int AggregateOfItems(string theItemName)
    {
        if (!IsItemInBag(theItemName)) return -1;
        int amount = 0;
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            if (instance.myBag.itemList[i].itemName.IndexOf(theItemName, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                amount = instance.myBag.itemList[i].itemHeld;
                break;
            }
        }
        return amount;
    }

    public void SwitchOpen()
    {
        isOpen = !isOpen;
        BagPanel.SetActive(isOpen);
        RefreshItem();
        if(isOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void CloseBtn()
    {
        isOpen = false;
        BagPanel.SetActive(isOpen);
        Time.timeScale = 1f;
    }




}
