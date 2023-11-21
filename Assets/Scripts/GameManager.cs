using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    private Maze mazeInstance;

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

    public GameObject keyPrefab;
    public static int numberOfKeys = 20;

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) { RestartGame(); }

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
        mazeInstance.RemoveWallsAtCoordinates(new IntVector2(0, 0), new IntVector2(1, 0), new IntVector2(2, 0));


        // Instantiate the player and set location.
        //playerInstance = Instantiate(playerPrefab) as Player;
        //playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
        // Assuming playerPrefab is your player GameObject prefab
        Vector3 spawnPosition = new Vector3(-3.4f, 0, -7.4f); // Replace x, y, z with your specific coordinates
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity) as Player;
        /*
        // Assuming playerPrefab is your player GameObject prefab
        if (playerSpawnPoint != null) {
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation) as Player;
        } else {
            Debug.LogError("Player spawn point not set!");
        }*/

        //playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        //playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(0, 0)));

        //one door at (mazeInstance.size.x - 2, mazeInstance.size.z - 1)
        //second at (1,0)

        //handle baking of navMesh
        nmBuilder.BuildNavMesh();

        // Instantiate the zombies and keys
        SpawnZombies(numberOfZombies);
        SpawnFastZombies(numberOfFastZombies);
        SpawnKeys(numberOfKeys);

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