/*using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Maze mazePrefab;
    public Player playerPrefab;

    public void LoadLevel(SaveData saveData)
    {
        ClearLevel();

        Maze maze = Instantiate(mazePrefab);
        maze.GenerateFromSaveData(saveData);

        Player player = Instantiate(playerPrefab);
        player.transform.position = new Vector3(saveData.playerData.positionX, saveData.playerData.positionY, saveData.playerData.positionZ);

        // Load other elements like zombies, keys...
    }

    private void ClearLevel()
    {
        // Implement logic to clear the existing level
    }
}*/
