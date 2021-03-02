using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class QuickItemBar : MonoBehaviour
{
    [Header("QuickItemBar")]
    public Image quickItemUI;
    public Text quiclItemText;
    public Text quickItemNameText;
    [Header("Potion")]
    public Text potionText;
    [Header("Bullet")]
    public Text bulletText;

    public Inventory playerInventory;
    List<Item> quickItemList = new List<Item>();
    public Item healPotion;
    public Item silverBullet;
    int quickItemBarIndex;

    public Item_old[] item;

    [Serializable]
    public class Item_old
    {
        public ItemType itemType;
        public Sprite sprite;
        public int num;
    }

    Image imageUI;
    Text numTextUI;
    //int potionAmount;
    //int bulletAmount;

    ItemType currentItem;

    public enum ItemType
    {
        MolotovCocktail,
        FirePaper
        
    }
    private void Awake()
    {
        imageUI = quickItemUI.GetComponent<Image>();
        numTextUI = quiclItemText.GetComponent<Text>();
    }

    private void OnEnable()
    {
        quickItemBarIndex = 0;
        RefreshQuickItemBar();
    }

    private void Start()
    {
        //Init();   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            SwitchState();
        }
    }


    void SwitchState()
    {
        if (quickItemList.Count == 0)
            return;
        
        quickItemBarIndex = (quickItemBarIndex + 1) % quickItemList.Count;
        
        imageUI.sprite = quickItemList[quickItemBarIndex].itemImage;
        numTextUI.text = quickItemList[quickItemBarIndex].itemHeld.ToString();
        quickItemNameText.text = quickItemList[quickItemBarIndex].itemName;
        imageUI.enabled = true;
    }

    public bool IsPotionEnough()
    {
        return (healPotion.itemHeld - 1 >= 0) ? true : false;
    }
    public bool IsBulletEnough()
    {
        return (silverBullet.itemHeld - 1 >= 0) ? true : false;
    }

    public void UsePotion()
    {
        healPotion.itemHeld -= 1;
        potionText.text = healPotion.itemHeld.ToString();
    }

    public void UseBullet()
    {
        silverBullet.itemHeld -= 1;
        bulletText.text = silverBullet.itemHeld.ToString();
    }

    public void RefreshQuickItemBar()
    {
        //quickItemList.Clear();
        for (int i = 0; i < playerInventory.itemList.Count; i++)
        {
            if( playerInventory.itemList[i] != null 
                && playerInventory.itemList[i].itemHeld > 0 
                && playerInventory.itemList[i].addInQuickItemList 
                && !quickItemList.Contains(playerInventory.itemList[i]))
            {
                quickItemList.Add(playerInventory.itemList[i]);
            }
        }
        potionText.text = healPotion.itemHeld.ToString();
        bulletText.text = silverBullet.itemHeld.ToString();
    }

}
