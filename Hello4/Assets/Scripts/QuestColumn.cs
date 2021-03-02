using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestColumn : MonoBehaviour
{
    public Quest quest;
    public Text titleUI;
    public Image processStatus;
    public GameObject prefab_DescriptionPanel;
    Transform descriptionPanel;
    QuestDescription qd;
    bool isOpenDescriptionPanel = false;
    bool isFinished = false;

    public event System.EventHandler<System.EventArgs> questFinished;
    public delegate void CB();
    
    public void Setup()
    {
        
        descriptionPanel = Instantiate(prefab_DescriptionPanel, transform.parent).GetComponent<Transform>();
        qd = descriptionPanel.GetComponent<QuestDescription>();
        descriptionPanel.gameObject.SetActive(false);
        qd.parent = this.transform;

        if (quest.info == null ) return;

        //targetName = quest.info.targetName;
        descriptionPanel.GetComponentInChildren<Text>().text = quest.info.description;
        titleUI.text = quest.info.title + SetupProgressBar();

        if(quest.info.rewardSprite != null )
        {
            qd.SetupRewardImage(quest.info.rewardSprite);
        }
        if (quest.info.targetSprite != null)
        {
            qd.SetupTartgeImage(quest.info.targetSprite);
        }

        qd.SetupRewardNum(quest.info.rewardQuantity);
        qd.SetupTargetNum(quest.info.quantity);

        //註冊事件
        questFinished += qd.GetButton_SetInteractble;

        OnProcessBarChanged();
    }

    string SetupProgressBar() => "(" + quest.currentProgressNum.ToString() + "/" + quest.info.quantity + ")";

    public void UpdateProcessBar(int delta)
    {
        if (quest.currentProgressNum + delta <= 0) return;
        quest.currentProgressNum += delta;
        quest.currentProgressNum = Mathf.Clamp(quest.currentProgressNum, 0, quest.info.quantity);

        OnProcessBarChanged();
    }

    public void UpdateProcessBar_Override(int delta)
    {
        quest.currentProgressNum = Mathf.Clamp(delta, 0, quest.info.quantity);

        OnProcessBarChanged();
    }



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            UpdateProcessBar(1);
        }
    }


    void OnProcessBarChanged()
    {
        titleUI.text = quest.info.title + SetupProgressBar();
        if (quest.currentProgressNum < quest.info.quantity) isFinished = false;
        else
            isFinished = true;

        if (isFinished)
        {
            processStatus.sprite = Database.instance.systemIcon.GetIcon(Marks.types.vMark);
            GetComponent<Image>().color = new Color32(0x8C, 0xE5, 0x80, 0xFF); // 完成的顏色

            //descriptionPanel.GetComponent<QuestDescription>().OnQuestFinished();
            questFinished?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            processStatus.sprite = Database.instance.systemIcon.GetIcon(Marks.types.xMark); //不同方法呼叫嘗試
            GetComponent<Image>().color = new Color32(56, 62, 63, 255);//未完成顏色

            qd.btn.color = new Color32(0xa2, 0xa2, 0xa2, 0xff); //未完成顏色
            qd.getButton.interactable = false;
        }
    }

    public void OnBtnClicked()
    {
        isOpenDescriptionPanel = !isOpenDescriptionPanel;
        descriptionPanel.gameObject.SetActive(isOpenDescriptionPanel);
    }


}
