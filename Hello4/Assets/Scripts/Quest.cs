using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Quest
{
    public QuestInfomation info;
    
    public int currentProgressNum = 0;

    public int aa { set; get; }
    public int bb;

    public List<TestInpector> cc = new List<TestInpector>();
    public Quest()
    {

    }
    public Quest(QuestInfomation _info) => info = _info;

    public void hello()
    {
        Quest qq = new Quest()
        {
            bb = 0,
            cc = new List<TestInpector>()
            {
                new TestInpector(),
                new TestInpector()
            }
        };
    }

}
