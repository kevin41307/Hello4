using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionContainer : MonoBehaviour
{
    [HideInInspector]
    public int amount { set; get; }
    public Text amountText;

    private void Awake()
    {
        amountText = amountText.GetComponent<Text>();
    }
    private void Start()
    {
        amount = 10;
        amountText.text = amount.ToString();
    }

    public void UsePotion()
    {
        amount -= 1;
        amountText.text = amount.ToString();
    }


}
