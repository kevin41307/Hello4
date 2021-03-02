using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Marks 
{
    public enum types
    {
        vMark,
        xMark,
        questionMark,
        ExclamationMark

    }
    public types type;
    public Sprite sprite;

	
}