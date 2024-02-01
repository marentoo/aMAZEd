using UnityEngine;
using System.Collections.Generic;

public class SaveData
{
    public int levelNumber;

    public float playerPositionX, playerPositionY, playerPositionZ;

    public float playerHealth;
    public int numberOfCollectedKeyes;

    public int GetLevelNumber() 
    {
        return this.levelNumber; // Assuming 'levelNumber' is the property storing the level number.
    }


   //getters
    public int getLevel(){
        return this.levelNumber;
    }

    public float getPositionX(){
        return this.playerPositionX;
    }
    
    public float getPositionY(){
        return this.playerPositionY;
    }
    public float getPositionZ(){
        return this.playerPositionZ;
    }
    
    public float getHealth(){
        return this.playerHealth;
    }
    public int getKeyes(){
        return this.numberOfCollectedKeyes;
    }

   //setters
    public void setLevel(int level){
        this.levelNumber = level;
    }

    public void setPosition(float x, float y, float z){
        this.playerPositionX = x;
        this.playerPositionY = y;
        this.playerPositionZ = z;
    }

    public void setKeyes(int keyes){
        this.numberOfCollectedKeyes = keyes;
    }
    /**/
    public void setHealth(float health){
        this.playerHealth = health;
    }


}
