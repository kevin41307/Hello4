using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public Database_SystemIcon systemIcon;

    private static Database _instance;
    public static Database instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            _instance = FindObjectOfType<Database>();

            if (_instance != null)
                return _instance;
            CreateInstance();
            return _instance;


        }
    }

    static void CreateInstance()
    {
        Database data = Resources.Load<Database>("Assets/Resources/Database");
        _instance = Instantiate(data);
        
    }
}
