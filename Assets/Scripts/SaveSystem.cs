/*
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/savefile.sav";

    public static void SaveGame(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);
            SaveData saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + saveFilePath);
            return null;
        }
    }
}
*/



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
/**/