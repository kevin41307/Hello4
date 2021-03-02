using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Database/Icon/SystemIcon", fileName = "SystemIcon")]
public class Database_SystemIcon : ScriptableObject
{
    public Sprite emptySprite;
    public List<Marks> marks = new List<Marks>();


    public Sprite GetIcon(Marks.types theType)
    {
        //Debug.Log(theType);
        Sprite theSprite = null;
        for (int i = 0; i < marks.Count; i++)
        {
            if (theType == marks[i].type)
            {
                theSprite = marks[i].sprite;
                break;
            }
        }

        if (theSprite == null)
            return emptySprite;

        return theSprite;
    }
}
