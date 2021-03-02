using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class QuestManager : MonoBehaviour
{

    static QuestManager instance;

    public GameObject questRoot;
    public GameObject listsRoot;
    public QuestInventory myQuest;

    public GameObject questColumn;
    public QuestInfomation testInfo;
    List<QuestColumn> columns = new List<QuestColumn>();
    List<string> collectQuestItemsKeys = new List<string>();


    bool isOpen = false;


    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }


    private void OnEnable()
    {
        RefreshList();
    }

    private void Start()
    {
        #region test linq
        /*
        isOpen = questRoot.activeSelf;
        Debug.Log( columns.Count(x => x.quest.info.targetName.Contains("Potion")));

        IEnumerable<QuestColumn> results =
            from column in columns
            where column.quest.info.targetName.Contains("Potion")
            select column;

        foreach (var result in results)
        {
            Debug.Log(result.quest.info.description);
        }

        IEnumerable<QuestColumn> results2 = columns.Where(x => x.quest.info.targetName.Contains("Potion"));
        foreach (var res in results2)
        {
            Debug.Log(res.quest.info.targetName);
        }


        string s = "hello";
        var query = s.Select((x, i) => new { x, i });
        foreach (var item in query) Debug.Log(item);

        int[] scores = { 90, 65, 82, 71, 84, 88, 52, 78, 61, 75, 85, 79 };
        var query2 = scores.OrderBy(x => x).GroupBy(x => x / 10);
        foreach (var g in query2)
        {
            Debug.Log("group key = " + g.Key + " values = ");
            foreach (var item in g) Debug.Log(item);
            Debug.Log("");
        }

        string[] stations = { "北京", "石家莊", "鄭州", "武漢", "衡陽", "廣州"};
        List<string> result2 = new List<string>();
        var query3 = stations.Skip(1).Aggregate(stations[0], (acc, curr) => { result2.Add(acc + "->" + curr); return curr; });
        foreach (var item in result2)
        {
            Debug.Log(item);
        }
        int[] arr = { 1, 2, 3, 4 };
        string[] people = { "kevin", "lala", "john" };
        
        var result4 = arr.Select(x => new List<int>() { x });
        foreach (var item in result4)
        {
            foreach (var i in item)
            {
                Debug.Log(item);
            }
            
        }
        
        var result3 = people.SelectMany(x => arr.Select(y => x + "+" + y));
        foreach (var item in result3)
        {
            Debug.Log(item);
        }
        */

        /*
        string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };

        // Determine whether any string in the array is longer than "banana".
        string longestName =
            fruits.Aggregate("banana",
                            (longest, next) =>
                                next.Length > longest.Length ? next : longest,
                            // Return the final result as an upper case string.
                            fruit => fruit.ToUpper());

        string hm = fruits.Aggregate("",(x, y) => x + y );
        var ss = fruits.Any(a => a.Length < 5);




        var aa = fruits.SelectMany((a) => a +"+-", (c,d) =>  new { V = c + "*", d } );

        foreach (var item in aa)
        {
            Debug.Log("" + item);
        }
        Console.WriteLine(
            "The fruit with the longest name is {0}.",
            longestName);

        
        int[] arr = { 5, 2, 1, 4 };
        var res = arr.TakeWhile(a => a > 3);
        foreach (var r in res)
        {
            Debug.Log(r);
        }
        */



        #endregion
    }


    public static void RefreshList()
    {
        instance.columns.Clear();
        instance.collectQuestItemsKeys.Clear();

        for (int i = 0; i < instance.listsRoot.transform.childCount; i++)
        {
            if (instance.listsRoot.transform.childCount == 0)
                break;
            Destroy(instance.listsRoot.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < instance.myQuest.questList.Count; i++)
        {
            if (instance.myQuest.questList[i].info == null) continue;
            QuestColumn qc = Instantiate(instance.questColumn, instance.listsRoot.transform).GetComponent<QuestColumn>();
            qc.quest = instance.myQuest.questList[i];
            qc.Setup();

            if( qc.quest.info.executionType == QuestInfomation.ExecutionType.CollectItem)
            {
                if (!instance.collectQuestItemsKeys.Contains(qc.quest.info.targetName))
                {
                    instance.collectQuestItemsKeys.Add(qc.quest.info.targetName);
                    Debug.Log(qc.quest.info.targetName);
                }

            }

            instance.columns.Add(qc);
        }
        foreach (var key in instance.collectQuestItemsKeys)
        {
            UpdateCollectQuest(new QuestEventArgs(key));
        }

       
    }


    public static void DeleteQuest(int index)
    {
        instance.myQuest.questList.RemoveAt(index);
        
        RefreshList();
    }
    public static void DeleteQuest(Quest quest)
    {
        instance.myQuest.questList.Remove(quest);

        RefreshList();
    }

    public static void AddQuest(QuestInfomation questInfo)
    {
        Quest quest = new Quest(instance.testInfo);
        bool insert = false;
        for (int i = 0; i < instance.myQuest.questList.Count; i++)
        {
            if (instance.myQuest.questList[i].info != null) continue;
            else
            {
                instance.myQuest.questList[i] = quest;
                insert = true;
                break;
            }
        }
        
        if ( !insert )
            instance.myQuest.questList.Add(quest);
        RefreshList();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            QuestManager.AddQuest(testInfo);
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            isOpen = !isOpen;
            questRoot.SetActive(isOpen);
        }
    }

    public void OnCloseBtnClicked()
    {
        isOpen = false;
        questRoot.SetActive(isOpen);
    }


    public static void UpdateEliminateQuest(QuestEventArgs args)
    {
        string[] words = args.targetName.Split(' ');
        foreach (var column in instance.columns)
        {

            var result = (from word in words
                         where (column.quest.info.targetName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                         select word).FirstOrDefault();
            if (result != null)
            {
                column.UpdateProcessBar(1);
            }

        }
        /*
        for (int i = 0; i < instance.columns.Count; i++)
        {
            Debug.Log(instance.columns[i].quest.info);
            string[] words = args.targetName.Split(' ');
            if (words.Length < 0) break;
            for (int j = 0; j < words.Length; j++)
            {
                if ( instance.columns[i].quest.info.targetName.IndexOf(words[j], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    instance.columns[i].UpdateProcessBar(1);
                    Debug.Log(words[j]);
                    break;
                }
            }
        }
        */
    }

    public static void UpdateCollectQuest(QuestEventArgs args)
    {
        int amount = InventoryManager.AggregateOfItems(args.targetName);
        if (amount == -1) amount = 0; //找不到=沒有=0

        for (int i = 0; i < instance.columns.Count; i++)
        {
            //Debug.Log(instance.columns[i].quest.info);
            string[] words = args.targetName.Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                if (instance.columns[i].quest.info.targetName.IndexOf(words[j], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    instance.columns[i].UpdateProcessBar_Override(amount);
                    break;
                }
            }
        }
    }
}

public class QuestEventArgs : EventArgs
{
    public Quest quest;
    public string targetName;
    
    public QuestEventArgs()
    {

    }
    public QuestEventArgs(string _targetName)
    {
        targetName = _targetName;
    }



}
