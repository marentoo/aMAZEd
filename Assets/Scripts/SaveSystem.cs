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

    public static bool CheckSaveFileExists()
    {
        return File.Exists(path);
    }


//only for level
    public static void SaveLevel(int levelNumber)
    {
        var levelData = new { LevelNumber = levelNumber };
        string json = JsonUtility.ToJson(levelData);
        File.WriteAllText(path, json); // 'path' should be the file path where you want to save the JSON.
    }
    public static int LoadLevel()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);
            return levelData.LevelNumber;
        }
        else
        {
            Debug.LogError("Level file not found in " + path);
            return -1; // Or any default value indicating an error
        }
    }



}
/**/

[System.Serializable]
public class LevelData
{
    public int LevelNumber;
}
