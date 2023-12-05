using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    /*
    //later it should be saved outside project folder
    //private static string path = Application.persistentDataPath + "/savefile.json";

    private static string path = Application.dataPath + "/Saves/savelevel.json";
    public static void Save(DataLevel data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
    public static SaveData Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return 0;
    }*/
}

public class DataLevel : MonoBehaviour 
{
    public int level;
}