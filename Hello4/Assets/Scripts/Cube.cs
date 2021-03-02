using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void GenerateColor()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.white;
    }

}
