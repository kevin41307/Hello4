using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Quest/QuestInventory", fileName = "QuestInventory")]
[System.Serializable]
public class QuestInventory : ScriptableObject
{ 
    public List<Quest> questList = new List<Quest>();
}
