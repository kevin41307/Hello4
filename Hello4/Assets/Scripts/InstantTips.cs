using UnityEngine;



[CreateAssetMenu(menuName = "MyTips/NewTips", fileName = "NewTips")]
public class InstantTips : ScriptableObject
{
    public string speakerName;
    [TextArea]
    public string description;
}
