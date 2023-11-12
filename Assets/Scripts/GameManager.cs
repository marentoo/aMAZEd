using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public Maze mazePrefab;
    private Maze mazeInstance;

    public Player playerPrefab;
    private Player playerInstance;

    public Zombie zombiePrefab;
    public int numberOfZombies = 5;
    
    public GameObject keyPrefab;
    public int numberOfKeys = 3;

    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

        // Instantiate and generate the maze.
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());

        // Instantiate the player and set location.
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

        ////NavMash handling
        //var navMeshSurface = mazeInstance.GetComponent<NavMeshSurface>();
        //if (navMeshSurface == null)
        //{
        //    navMeshSurface = mazeInstance.gameObject.AddComponent<NavMeshSurface>();
        //}

        //navMeshSurface.collectObjects = CollectObjects.Children;
        //navMeshSurface.layerMask = LayerMask.GetMask("Walkable");
        //navMeshSurface.BuildNavMesh();

        // Instantiate the zombies and keys
        SpawnZombies(numberOfZombies);
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


    private void SpawnZombies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Zombie zombieInstance = Instantiate(zombiePrefab) as Zombie;
            zombieInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
            zombieInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }
    private void SpawnKeys(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 keyPosition = randomCell.transform.position;
            float floatHeight = 0.1f; // The height above the ground at which the key will float
            keyPosition.y += floatHeight;

            GameObject key = Instantiate(keyPrefab, keyPosition, Quaternion.identity);
            key.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

        }
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
            //Destroy(zombieInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }
}