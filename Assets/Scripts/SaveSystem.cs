using System.IO;
using UnityEngine;

public static class SaveSystem
{
    //later it should be saved outside project folder
    //private static string path = Application.persistentDataPath + "/savefile.json";

    private static string path = Application.dataPath + "/Saves/savefile.json";
    public static void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
    public static SaveData LoadGame()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }
}