using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    public Maze mazeInstance;

    public MazeCell cellPrefab;
    //public LiftRoom LiftPrefab;

    //new
    public GameObject entryRoomPrefab;
    public GameObject exitRoomPrefab;

    public Transform playerSpawnPoint;
    public Player playerPrefab;
    private Player playerInstance;

    private Zombie zombieInstance;
    public Zombie zombiePrefab;
    
    private ZombieFast zombiefInstance;
    public ZombieFast zombiefPrefab;

    public int numberOfZombies = 5;
    public int numberOfFastZombies = 2;

    public GameObject keyPrefab, healthPrefab;
    public static int numberOfKeys = 3;
    public static int numberOfHealths = 2;

    public GameObject storyItemPrefab; // Assign your story item prefab in the Inspector
    public string[] storyFiles; // Array of story file names

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.Return))
        {
            //mazeInstance.ToggleDoorsInRoom(false); // Close all doors in the current room
        }
    }

    //[SerializeField]
    private MazeNavMeshBuilder nmBuilder;

    private void Awake()
    {
        nmBuilder = GetComponent<MazeNavMeshBuilder>();
    }

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

        // Instantiate and generate the maze.
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        PlaceEntryAndExitRooms();
        //mazeInstance.RemoveWallsAtCoordinates(new IntVector2(0, 0), new IntVector2(1, 0), new IntVector2(2, 0));
        mazeInstance.RemoveWallsAtCoordinates(new IntVector2(1, 0));
        //mazeInstance.RemoveWallsAtCoordinates1(new IntVector2(mazeInstance.size.x - 2, mazeInstance.size.z - 1), new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1), new IntVector2(mazeInstance.size.x - 3, mazeInstance.size.z - 1));
        mazeInstance.RemoveWallsAtCoordinates1( new IntVector2(mazeInstance.size.x - 2, mazeInstance.size.z - 1));

        //MazeRoom liftRoomInstance = Instantiate(liftRoomPrefab) as MazeRoom;
        //liftRoomInstance.transform.position = CalculateLiftRoomPosition();
        //ConnectLiftRoomToMaze(liftRoomInstance);

        // Instantiate the player and set location.
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
        // Assuming playerPrefab is your player GameObject prefab

        //handle baking of navMesh
        nmBuilder.BuildNavMesh();

        // Instantiate the zombies and keys
        SpawnZombies(numberOfZombies);
        SpawnFastZombies(numberOfFastZombies);
        SpawnKeys(numberOfKeys);
        SpawnHealth(numberOfHealths);
        SpawnStoryItems(storyFiles.Length);


        CameraFollow cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollowScript != null)
        {
            cameraFollowScript.StartFollowingPlayer(playerInstance.transform);
        }
        else
        {
            Debug.LogError("CameraFollow script not found on the main camera!");
        }

        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.5f);
    }

    /*
    private MazeCell CreateOutofBoundCell(IntVector2 coordinates) {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        newCell.coordinates = coordinates;
        newCell.name = "Out of Bound Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        // Position the cell outside of the maze boundaries
        newCell.transform.localPosition = new Vector3(coordinates.x, 0f, coordinates.z);
        return newCell;
    }*/

    private Vector3 CalculateLiftRoomPosition()
    {
        // Calculate where the lift room should be instantiated
        // This is just an example and would need to be adjusted based on your game's logic
        return new Vector3(mazeInstance.size.x, 0, mazeInstance.size.z);
    }

    private void ConnectLiftRoomToMaze(MazeRoom liftRoomInstance)
    {
        // Create passages or doors that connect the lift room to the maze
        // This method will need to be customized based on how you want to connect the rooms
    }
    public void SaveGame()
    {
        if (playerInstance == null)
        {
            Debug.LogError("Player instance is null");
            return;
        }


        SaveData saveData = new SaveData();
        saveData.playerPositionX = playerInstance.transform.position.x;
        saveData.playerPositionY = playerInstance.transform.position.y;
        saveData.playerPositionZ = playerInstance.transform.position.z;

        Transform cameraTransform = Camera.main.transform;
        saveData.cameraRotationX = cameraTransform.eulerAngles.x;
        saveData.cameraRotationY = cameraTransform.eulerAngles.y;
        saveData.cameraRotationZ = cameraTransform.eulerAngles.z;

        SaveSystem.SaveGame(saveData);

    }
    public void LoadGame()
    {
        SaveData saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            if (playerInstance != null)
            {
                playerInstance.transform.position = new Vector3(saveData.playerPositionX, saveData.playerPositionY, saveData.playerPositionZ);
            }
            else
            {
                Debug.LogError("Player instance is null.");
            }

            // Load other game state elements here, like enemies' positions, collected items, etc.
        }
        if (Camera.main != null)
        {
            Camera.main.transform.eulerAngles = new Vector3(
                saveData.cameraRotationX,
                saveData.cameraRotationY,
                saveData.cameraRotationZ
            );
        }
        else
        {
            Debug.LogError("No save data found.");
        }
    }

    private void PlaceEntryAndExitRooms() {

        //entry room
        /**/
        Quaternion entryRotation = Quaternion.Euler(0, 90, 0); // Adjust this as needed
        MazeCell entryCell = mazeInstance.GetCell(new IntVector2(0, 0));
        Instantiate(entryRoomPrefab, entryCell.transform.position, entryRotation);
        

        //exit room
        Quaternion exitRotation = Quaternion.Euler(0, -90, 0); // Adjust this as needed
        MazeCell exitCell = mazeInstance.GetCornerCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)); // Top-right corner
        Instantiate(exitRoomPrefab, exitCell.transform.position, exitRotation);
    }
    

    
    // Adjustments for spawning and door handling
    public float minSpawnDistanceFromPlayer = 10f; // Minimum distance from the player to spawn zombies

    private void SpawnZombies(int count)
    {

        for (int i = 0; i < count; i++)
        {
            MazeCell cell = null;
            float distance;
            do
            {
                cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
                distance = Vector3.Distance(playerInstance.transform.position, cell.transform.position);
            }
            while (distance < minSpawnDistanceFromPlayer); // Keep looking for a cell that is far enough from the player

            zombieInstance = Instantiate(zombiePrefab) as Zombie;
            zombieInstance.SetLocation(cell);
            zombieInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }

    private void SpawnFastZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell cell = null;
            float distance;
            do
            {
                cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
                distance = Vector3.Distance(playerInstance.transform.position, cell.transform.position);
            }
            while (distance < minSpawnDistanceFromPlayer); // Keep looking for a cell that is far enough from the player

            zombiefInstance = Instantiate(zombiefPrefab) as ZombieFast;
            zombiefInstance.SetLocation(cell);
            zombiefInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }

    private void SpawnKeys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 keyPosition = randomCell.transform.position;
            float floatHeight = 0.04f; // The height above the ground at which the key will float
            keyPosition.y += floatHeight;

            GameObject key = Instantiate(keyPrefab, keyPosition, Quaternion.identity);
            key.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
    }

    private void SpawnHealth(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 healthPosition = randomCell.transform.position;
            float floatHeight = 0.04f; // The height above the ground at which the key will float
            healthPosition.y += floatHeight;

            GameObject health = Instantiate(healthPrefab, healthPosition, Quaternion.identity);
            health.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }



    private void SpawnStoryItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 itemPosition = randomCell.transform.position;
            float floatHeight = 0.1f;
            itemPosition.y += floatHeight;

            GameObject storyItem = Instantiate(storyItemPrefab, itemPosition, Quaternion.identity);
            storyItem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            StoryTrigger storyTrigger = storyItem.GetComponent<StoryTrigger>();
            if (storyTrigger != null && i < storyFiles.Length)
            {
                storyTrigger.storyFileName = storyFiles[i];
            }
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    /*
    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
            Destroy(zombieInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }*/
}