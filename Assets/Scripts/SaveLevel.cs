using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    public void Save(SaveData data)
    {
        //PlayerPrefs.SetFloat("key", value); SetInt SetString
        PlayerPrefs.SetInt("levelNumber", data.getLevel());

        PlayerPrefs.SetFloat("playerPositionX", data.getPositionX());
        PlayerPrefs.SetFloat("playerPositionY", -0.033f);
        PlayerPrefs.SetFloat("playerPositionZ", data.getPositionZ());

        //PlayerPrefs.SetFloat("playerPosition", data.getPosition());

        PlayerPrefs.SetFloat("playerHealth", data.playerHealth);
        PlayerPrefs.SetInt("numberOfCollectedKeyes", data.getKeyes());

    }

    public SaveData Load()
    {
        SaveData data = new SaveData();

        data.setLevel(PlayerPrefs.GetInt("levelNumber"));

        data.setPosition(
            PlayerPrefs.GetFloat("playerPositionX"),
            PlayerPrefs.GetFloat("playerPositionY"),
            PlayerPrefs.GetFloat("playerPositionZ"));

        data.playerHealth = PlayerPrefs.GetFloat("playerHealth");
        data.setKeyes(PlayerPrefs.GetInt("numberOfCollectedKeyes"));
        
        return data;

    }

    public void DeleteSaveWhenNewLevel()
    {
        PlayerPrefs.DeleteKey("playerPositionX");
        PlayerPrefs.DeleteKey("playerPositionY");
        PlayerPrefs.DeleteKey("playerPositionZ");
        PlayerPrefs.DeleteKey("playerHealth");
        PlayerPrefs.DeleteKey("numberOfCollectedKeyes");
    }


  //level number management
    public int LoadLvl()
    {
        return PlayerPrefs.GetInt("levelNumber");
    }
    public void SaveLvl(int level)
    {
        PlayerPrefs.SetInt("levelNumber", level);
    }
/*  
  //lighter number management
    public int loadLighterNumber()
    {
        return PlayerPrefs.GetInt("lightersNumber");
    }
    public void SaveLighterNumber(int lighter)
    {
        PlayerPrefs.SetInt("lightersNumber", lighter);
    }

    public void addLighterToInventory()
    {
        int lighter = PlayerPrefs.GetInt("lightersNumber");
        lighter++;
        SaveLighterNumber(lighter);
    }
    public void removeighterToInventory()
    {
        int lighter = PlayerPrefs.GetInt("lightersNumber");
        lighter--;
        SaveLighterNumber(lighter);
    }
*/

  //matches number management
    public int loadMatchNumber()
    {
        return PlayerPrefs.GetInt("matchesNumber");
    }
    public void SaveMathchNumber(int match)
    {
        PlayerPrefs.SetInt("matchesNumber", match);
    }

    public void addMatchToInventory()
    {
        int match = PlayerPrefs.GetInt("matchesNumber");
        match++;
        SaveMathchNumber(match);
    }
    public void removeMatchToInventory()
    {
        int match = PlayerPrefs.GetInt("matchesNumber");
        match--;
        SaveMathchNumber(match);
    }

  //end level number management
    public void SetEndLevelNumber(){
        //int endLvl = 3;
        int endLvl = Random.Range(100, 10000);
        PlayerPrefs.SetInt("endLevelNumber", endLvl);
    }

    public int GetEndLevelNumber(){
        int endLevelNumber = PlayerPrefs.GetInt("endLevelNumber");
        return endLevelNumber;
    }

    //delets everything from save
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }

    

}
