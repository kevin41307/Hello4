using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantTipsOnWorld : MonoBehaviour
{
    public Text m_Text;
    [HideInInspector]
    float displayTime = 2f;
    
    public void SetupTips(string content)
    {
        m_Text.text = content;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Show(string content)
    {
        SetupTips(content);
        this.gameObject.SetActive(true);
        float time = displayTime + content.Length / 20;
        StartCoroutine(StartFadeOutTips(time));
    }

    public void Hide()
    {
        m_Text.text = "";
        
        this.gameObject.SetActive(false);
    }

    IEnumerator StartFadeOutTips(float time)
    {

        yield return new WaitForSeconds(time);
        Hide();
    }
    
}
