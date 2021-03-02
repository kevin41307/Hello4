using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsOnWorld : MonoBehaviour
{
    public GameObject tipsPrefab;
    public RectTransform rootCanvas;
    RectTransform rect;
    GameObject tips;
    bool isClose = false;

    private void Awake()
    {
        tips = Instantiate(tipsPrefab, transform.position, Quaternion.identity, rootCanvas);
        tips.GetComponent<RectTransform>().localPosition = Vector3.zero;
        rect = tips.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (!isClose)
        {
            if (Game.IsInFrontOfCamera(transform.position))
            {
                tips.SetActive(true);

                rect.anchoredPosition = UIPosition.WorldToUI(transform.position, rootCanvas);

            }
            else
            {
                tips.SetActive(false);
            }

        }

        
    }
    
    public void CloseTips()
    {
        isClose = true;
        tips.SetActive(false);
    }


}
