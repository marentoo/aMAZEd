using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{


    public Maze mazePrefab;
    public Maze mazeInstance;

    public MazeCell cellPrefab;
    //public LiftRoom LiftPrefab;

    //new
    public GameObject entryRoomPrefab;
    public GameObject exitRoomPrefab;
    public PlayableDirector cutsceneDirector;
    public PlayableDirector cutsceneDirector2;

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
    private GameObject key;
    private GameObject health;
    private GameObject storyItem;
    private GameObject entry, exit;

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

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
			RestartGame();
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
        //yield return StartCoroutine(mazeInstance.Generate());
        Coroutine mazeGenerator = StartCoroutine(mazeInstance.Generate());
        //StartCoroutine(mazeInstance.Generate());
        


        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            Debug.Log("Cutscene playing.");
        }
        Debug.Log("Cutscene should now be playing");
        

        // Ensure the maze generation coroutine has finished as well.
        yield return mazeGenerator;

        if (mazeInstance != null)
        {
            PlaceEntryAndExitRooms();

            mazeInstance.RemoveWallsAtCoordinates(new IntVector2(1, 0));
            mazeInstance.RemoveWallsAtCoordinates1( new IntVector2(mazeInstance.size.x - 2, mazeInstance.size.z - 1));

            // Instantiate the player and set location.
            playerInstance = Instantiate(playerPrefab) as Player;
            playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
            
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
        }

        

        cutsceneDirector.Stop();
        //yield return null;

        if (cutsceneDirector2 != null)
        {
            cutsceneDirector2.Play();
            Debug.Log("Cutscene playing.");
        }
        

        Debug.Log("Maze generated!!!");
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.5f);

        // After maze generation is done, stop the cutscene if it is still playing
        cutsceneDirector2.Stop();
        
        //yield return null;

       
    }

    private void SetupPlayer()
    {
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
        // ... other player setup code.
    }

    private void SetupCamera()
    {
        CameraFollow cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollowScript != null)
        {
            cameraFollowScript.StartFollowingPlayer(playerInstance.transform);
        }
        else
        {
            Debug.LogError("CameraFollow script not found on the main camera!");
        }
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
        entry = Instantiate(entryRoomPrefab, entryCell.transform.position, entryRotation);
        // Assume 'size' is the size of your maze, and 'elevatorCellPrefab' is assigned.
		//mazeInstance.PlaceElevatorCell(new IntVector2(0, -1), false); //, MazeDirection.South

        //exit room
        Quaternion exitRotation = Quaternion.Euler(0, -90, 0); // Adjust this as needed
        MazeCell exitCell = mazeInstance.GetCornerCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)); // Top-right corner
        exit = Instantiate(exitRoomPrefab, exitCell.transform.position, exitRotation);
		// Place the elevator at (size.x, size.z - 1), which might be (instance+1, instance-1)
		//mazeInstance.PlaceElevatorCell(new IntVector2(mazeInstance.size.x-1, mazeInstance.size.z), true);//, MazeDirection.North
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

            key = Instantiate(keyPrefab, keyPosition, Quaternion.identity);
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

            health = Instantiate(healthPrefab, healthPosition, Quaternion.identity);
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

            storyItem = Instantiate(storyItemPrefab, itemPosition, Quaternion.identity);
            storyItem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            StoryTrigger storyTrigger = storyItem.GetComponent<StoryTrigger>();
            if (storyTrigger != null && i < storyFiles.Length)
            {
                storyTrigger.storyFileName = storyFiles[i];
            }
        }
    }

    /*
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }*/
    
    private void RestartGame()
    {
        // Stop all coroutines to prevent them from affecting the new game instance
        StopAllCoroutines();

        // Destroy the maze instance and all its children (including walls, cells, etc.)
        if (mazeInstance != null)
        { Destroy(mazeInstance.gameObject); }

        // Stop the camera from following the destroyed player
        Camera.main.GetComponent<CameraFollow>().StopFollowingPlayer();

        // Destroy the player instance
        if (playerInstance != null)
        { Destroy(playerInstance.gameObject); }

        // Destroy all zombies in the scene
        foreach (var zombie in FindObjectsOfType<Zombie>())
        { Destroy(zombie.gameObject); }
        
        // Destroy all fast zombies in the scene
        foreach (var ZombieFast in FindObjectsOfType<ZombieFast>())
        { Destroy(ZombieFast.gameObject); }

        // Destroy all keys in the scene
        foreach (var key in GameObject.FindGameObjectsWithTag("Key"))
        { Destroy(key.gameObject); }
        
        // Destroy all keys in the scene
        foreach (var storyItem in GameObject.FindGameObjectsWithTag("Story"))
        { Destroy(storyItem.gameObject); }
        
        // Destroy all hearts in the scene
        foreach (var health in GameObject.FindGameObjectsWithTag("Health"))
        { Destroy(health.gameObject); }

        // Destroy all entry in the scene
        if (entry != null)
        { Destroy(entry.gameObject);  entry = null; }

        // Destroy all exit in the scene
        if (exit != null)
        { Destroy(exit.gameObject); exit = null;}

        


        // Start a new game
        StartCoroutine(BeginGame());
    }

   


}