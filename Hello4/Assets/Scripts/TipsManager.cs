using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public GameObject tipsPrefab;
    public RectTransform parentCanvas;
    InstantTipsOnWorld tips;
    public static TipsManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

    }

    private void Start()
    {
        tips = Instantiate(tipsPrefab, parentCanvas).GetComponent<InstantTipsOnWorld>();
        tips.Hide();
    }

    public void ShowTips(string content)
    {
        if (!tips.gameObject.activeSelf)
            instance.tips.Show(content);
        else
            Debug.Log("InstantTips占用中...內容:" + content);
    }
    public void CloseTips()
    {
        instance.tips.Hide();
    }    

}
