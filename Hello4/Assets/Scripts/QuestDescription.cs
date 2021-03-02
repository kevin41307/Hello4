using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestDescription : MonoBehaviour
{
    public Transform parent;
    public Image btn;
    public Button getButton;
    public Image targetImage;
    public Image rewardImage;
    public Text targetNum;
    public Text rewardNum;


    [HideInInspector]
    public Color32 finishColor = new Color32(0x8C, 0xE5, 0x80, 0xFF);


    public event EventHandler<EventArgs> OnClicked;

    private void Awake()
    {

    }

    private void Start()
    {
        this.OnClicked += this.print1;
        bomb();
        //QuestColumn.QuestFinished += GetButton_SetInteractble;
        
        getButton.onClick.AddListener(OnGetBtnClicked);
    }
    public void bomb()
    {
        if(OnClicked != null )
        {
            this.OnClicked(this, EventArgs.Empty);
        }
    }    

    public void print1(object sender, EventArgs args)
    {
        //Debug.Log("test");
        //Debug.Log((sender as QuestDescription).parent);
        
    }

    public void SetupImage(Sprite target, Sprite reward)
    {
        targetImage.sprite = target;
        rewardImage.sprite = reward;
    }
    public void SetupTartgeImage(Sprite target)
    {
        targetImage.sprite = target;
    }
    public void SetupRewardImage(Sprite reward)
    {
        rewardImage.sprite = reward;
    }

    public void SetupTargetNum(int num )
    {
        targetNum.text = num.ToString();
    }
    public void SetupRewardNum(int num)
    {
        rewardNum.text = num.ToString();
    }

    public void OnGetBtnClicked() => QuestManager.DeleteQuest(parent.GetComponent<QuestColumn>().quest);


    public void GetButton_SetInteractble(object sendor, EventArgs e)
    {
        getButton.interactable = true;
        btn.color = finishColor;
    }

    


    private void OnDestroy()
    {
        getButton.onClick.RemoveAllListeners();
    }
}
