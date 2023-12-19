/*
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int mazeWidth;
    public int mazeHeight;
    public List<CellData> cells;
    public PlayerData playerData;
    // Add other elements as needed
}

[System.Serializable]
public class CellData
{
    // Example properties
    public int x, y;
    public bool isWall;
    // Add other cell properties as needed
}

[System.Serializable]
public class PlayerData
{
    // Example properties
    public float positionX, positionY, positionZ;
    // Add other player properties as needed
}

// Add other data classes for zombies, keys, etc., similar to PlayerData
*/


using UnityEngine;
using System.Collections.Generic;

public class SaveData
{
    //public int levelNumber;

    public float playerPositionX, playerPositionY, playerPositionZ;
    public float cameraRotationX, cameraRotationY, cameraRotationZ;

    //public int playerHealth;
    //public int numberOfCollectedKeyes;

}
/**/