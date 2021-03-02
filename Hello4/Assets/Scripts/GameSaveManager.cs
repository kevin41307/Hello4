using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class GameSaveManager : MonoBehaviour
{
    public Inventory myInventory;
    public Item healPotion;
    public Item Bullet;
    public Item MolotovCocktail;
    public Item FirePaper;
    


    string persistentDataPath;
    string selfPath = "/game_SaveData";
    string selfFullPath; 
    string filename;



    private void Awake()
    {
        persistentDataPath = Application.persistentDataPath;
        selfFullPath = persistentDataPath + selfPath;
        //Debug.Log(selfFullPath);

    }
    public void SaveGame()
    {
        Debug.Log(Application.persistentDataPath);

        if(!Directory.Exists(selfFullPath))
        {
            Directory.CreateDirectory(selfFullPath);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(selfFullPath + "/inventory.txt");
        var json = JsonUtility.ToJson(myInventory);
        formatter.Serialize(file, json);
        file.Close();

        file = File.Create(selfFullPath + "/healPotion.txt");
        json = JsonUtility.ToJson(healPotion);
        formatter.Serialize(file, json);
        file.Close();

        file = File.Create(selfFullPath + "/Bullet.txt");
        json = JsonUtility.ToJson(Bullet);
        formatter.Serialize(file, json);
        file.Close();

        file = File.Create(selfFullPath + "/MolotovCocktail.txt");
        json = JsonUtility.ToJson(MolotovCocktail);
        formatter.Serialize(file, json);
        file.Close();

        file = File.Create(selfFullPath + "/FirePaper.txt");
        json = JsonUtility.ToJson(FirePaper);
        formatter.Serialize(file, json);
        file.Close();
        //EditorUtility.SetDirty(myInventory);


    }

    public void LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(selfFullPath + "/inventory.txt"))
        {
            file = File.Open(selfFullPath + "/inventory.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), myInventory);
            file.Close();
        }

        if (File.Exists(selfFullPath + "/healPotion.txt"))
        {
            file = File.Open(selfFullPath + "/healPotion.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), healPotion);
            file.Close();
        }

        if (File.Exists(selfFullPath + "/Bullet.txt"))
        {
            file = File.Open(selfFullPath + "/Bullet.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), Bullet);
            file.Close();
        }

        if (File.Exists(selfFullPath + "/MolotovCocktail.txt"))
        {
            file = File.Open(selfFullPath + "/MolotovCocktail.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), MolotovCocktail);
            file.Close();
        }

        if (File.Exists(selfFullPath + "/FirePaper.txt"))
        {
            file = File.Open(selfFullPath + "/FirePaper.txt", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), FirePaper);
            
            file.Close();
        }

        InventoryManager.RefreshItem();
    }

}
