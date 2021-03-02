using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy000 : MonoBehaviour
{
    // Start is called before the first frame update

    MeshRenderer mr;     
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnHit()
    {
        Debug.Log("OnHit");
        mr.material.color = Color.red;
        StartCoroutine(BackToNormal());
    }
    IEnumerator BackToNormal()
    {

        for (float t = 0; t <= 1f; t+=0.1f)
        {
            mr.material.color = Color.Lerp(Color.red, Color.white, t);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
