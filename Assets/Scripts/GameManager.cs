using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    public Maze mazeInstance;
    public MazeCell cellPrefab;
    private StoryDisplay storyDisplay;

    private LevelDisplay levelDisplay;

    public GameObject entryRoomPrefab;
    public GameObject exitRoomPrefab;
    public PlayableDirector cutsceneDirector;
    public PlayableDirector cutsceneDirector2;

    public GameObject inSceneToActivate;
    public GameObject outSceneToActivate;

    public Transform playerSpawnPoint;
    public Player playerPrefab;
    private Player playerInstance;
    private Camera playerCamera;
    public Torch torchInstance;
    public GameObject ErrorUI;


    private Zombie zombieInstance;
    public Zombie zombiePrefab;
    
    private ZombieFast zombiefInstance;
    public ZombieFast zombiefPrefab;

    public int numberOfZombies = 5;
    public int numberOfFastZombies = 2;

    public GameObject keyPrefab, healthPrefab, lighterPrefab, matchPrefab;
    private GameObject key;
    private GameObject health;
    private GameObject lighter;
    private GameObject match;
    private GameObject storyItem;
    private GameObject entry, exit;
    private float BurnOutTime;
    private Timer timer;
    private float cutsceneTime = 10f;

    public static int numberOfKeys = 3;
    public static int numberOfHealths = 2;
    public static int numberOfLighters = 3;
    public static int numberOfMatches = 3;

    private bool stopped = false;

    private SaveLevel saveLevelInstance;

    public GameObject storyItemPrefab; // Assign your story item prefab in the Inspector
    private string[] storyFiles;  // Array of story file names

    public int numberOfStories = 0;
    public int level=1;

    private float minSpawnDistanceFromPlayer = 4f; // Minimum distance from the player to spawn zombies

    private void Start()
    {   
        StartCoroutine(BeginGame());
        storyDisplay = FindObjectOfType<StoryDisplay>();
        levelDisplay = FindObjectOfType<LevelDisplay>();
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        ErrorUI.SetActive(false);   
    }
        
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.L))
        {
            SaveGame();
        }

        if (stopped) { ErrorDisplay(); }
        else { ErrorHide(); }

    }

    //[SerializeField]
    private MazeNavMeshBuilder nmBuilder;

    private void Awake()
    {
        nmBuilder = GetComponent<MazeNavMeshBuilder>();
        saveLevelInstance = new SaveLevel();

        timer = FindObjectOfType<Timer>();
        if (timer == null) {
            Debug.LogError("Timer component not found on any GameObject.");
        }
    }


    private void LevelSettings()
    {
        level = saveLevelInstance.LoadLvl();
        Debug.Log($"Level loaded: {level}");
        if(level == 0)
        { 
            level = 1;
            saveLevelInstance.SaveLvl(level);
            saveLevelInstance.SetEndLevelNumber();
        }
       //tutorial levels 
        if (level == 1) // Assuming the first level is represented by 1
        {
            mazeInstance.size = new IntVector2(5, 5);
            numberOfMatches = 0; 
            BurnOutTime = 600f;
            mazeInstance.roomExpansionChance = 0.5f; 
            numberOfKeys = 2; 
            numberOfZombies = 0; 
            numberOfFastZombies = 0; 
            numberOfHealths = 0;
            numberOfStories = 0; 
        }
        else if (level == 2) 
        {
            mazeInstance.size = new IntVector2(6, 6);
            mazeInstance.roomExpansionChance = 0.4f;
            minSpawnDistanceFromPlayer = 3f; 
            numberOfMatches = 4; 
            BurnOutTime = 60f;
            numberOfKeys = 3;
            numberOfZombies = 1;
            numberOfFastZombies = 0;
            numberOfHealths = 2; 
            numberOfStories = 0; 
        }
        else if (level == 3) 
        {
            mazeInstance.size = new IntVector2(7, 7);
            mazeInstance.roomExpansionChance = 0.3f;
            minSpawnDistanceFromPlayer = 3f; 
            numberOfMatches = 7; 
            BurnOutTime = 30f;
            numberOfKeys = 2;
            numberOfZombies = 1;
            numberOfFastZombies = 1;
            numberOfHealths = 5; 
            numberOfStories = 0; 
        }
        else if (level == 4)
        {
            mazeInstance.size = new IntVector2(8, 8);
            mazeInstance.roomExpansionChance = 0.4f;
            numberOfMatches = 8; 
            BurnOutTime = 60f;
            numberOfKeys = 4; 
            numberOfZombies = 3; 
            numberOfFastZombies = 1; 
            numberOfHealths = 4; 
            numberOfStories = 2; 
        }
       //other levels
        else
        {
            // For higher levels, increase maze size and number of zombies slightly
            int sizeIncrease = level % 5;
            //numberOfLighters = 5 + (int)(sizeIncrease / 2);
            int rand = Random.Range(1, 4);
            mazeInstance.size = new IntVector2(7 + rand + (int)(sizeIncrease), 7 + rand + (int)(sizeIncrease));
            numberOfKeys = 1 + (int)(sizeIncrease * 1.5f); 
            numberOfZombies = (int)(sizeIncrease / rand);
            numberOfFastZombies = (int)(sizeIncrease  / rand);
            numberOfHealths = (numberOfZombies * 3) + (numberOfFastZombies * 6) + (int)(sizeIncrease);
            numberOfStories = Random.Range(0, 6);
            numberOfMatches = 5 + rand + (int)(sizeIncrease); 
            BurnOutTime = 60f + (int)(sizeIncrease / rand);
            mazeInstance.roomExpansionChance = 0.4f + 0.0005f * sizeIncrease; // Increase room expansion probability
        }

    }
    private IEnumerator BeginGame()
    {
        
        inSceneToActivate.SetActive(true);
        outSceneToActivate.SetActive(false);
        timer.StartTimer();
        Camera.main.GetComponent<AudioListener> ().enabled  =  true;
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

        // Instantiate and generate the maze.
        mazeInstance = Instantiate(mazePrefab) as Maze;

        // Adjust game settings based on the level
        LevelSettings();
        
        //yield return StartCoroutine(mazeInstance.Generate());
        Coroutine mazeGenerator = StartCoroutine(mazeInstance.Generate());
        //StartCoroutine(mazeInstance.Generate());
        
        //first cutscene 
        if (cutsceneDirector != null)
        {
            cutsceneDirector.Play();
            Debug.Log("Cutscene playing.");
        }
        Debug.Log("Cutscene should now be playing");
        

        //play cutscene while maze is generated
        yield return mazeGenerator;

        if (mazeInstance != null)
        {
            PlaceEntryAndExitRooms();
            // Instantiate the player and set location.
            SetupPlayer();
            Camera.main.GetComponent<AudioListener> ().enabled  =  false;
            //handle baking of navMesh
            nmBuilder.BuildNavMesh();

            //spawn items
            SpawnKeys(numberOfKeys);
            SpawnHealth(numberOfHealths);
            SpawnStoryItems(numberOfStories);
            //SpawnLighter(numberOfLighters);
            SpawnMatches(numberOfMatches);
            //handle baking of navMesh
            nmBuilder.BuildNavMesh();


            SetupCamera();
            Debug.Log("Maze generated!!!");
        }
        //yield return new WaitForSeconds(9);
        yield return new WaitUntil(() => cutsceneDirector.state != PlayState.Playing);

        cutsceneDirector.Stop();
        inSceneToActivate.SetActive(false);
        outSceneToActivate.SetActive(true);
        Debug.Log("First cutscene stopped.");

        // Wait for the second cutscene to finish
        // Begin the second cutscene
        int endLvl = saveLevelInstance.GetEndLevelNumber();

        if (cutsceneDirector2 != null)
        {
            Debug.Log($"EndLvl: {endLvl}");
            
            Debug.Log("Second cutscene playing.");
            cutsceneDirector2.Play();

            //if this is the end switch scenes to the end one
            if(level > endLvl){
                SceneManager.LoadScene("DockThingEnd");
            }
            // Wait for the second cutscene to finish
            yield return new WaitUntil(() => cutsceneDirector2.state != PlayState.Playing);
            //yield return new WaitForSeconds(5);            

            cutsceneDirector2.Stop();
        }
        
        // Instantiate the zombies
        SpawnZombies(numberOfZombies);
        SpawnFastZombies(numberOfFastZombies);
            

        timer.StopTimer();
        cutsceneTime = timer.GetElapsedTime();
        Debug.Log($"cutsceneTime: {cutsceneTime}");
        
        SetupTorch();
        AddTimeTorch(cutsceneTime);
        

        
        //torchInstance = player.GetComponentInChildren<Torch>();


        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.5f);
        Camera.main.depth = 3;

        //disable cutscene view by switching player camera depth to higher one
        if (playerCamera != null) {
            playerCamera.depth = 2; // Reset the depth to make it the active camera
        }

        //storyDisplay.message = "Level: 1, 2, 3, 4, 5, 6, 7, 8, 9 , 0";//+level;
        //levelDisplay.message = level.ToString();
        levelDisplay.SetMessage(level.ToString());
        levelDisplay.SetTime(BurnOutTime.ToString());
        levelDisplay.DisplayMessage();
       
    }

    private void SetupPlayer()
    {
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2(1, 0)));
        playerInstance.transform.Rotate(0f, 0f, 0f);

        //for disable cutscene view in next steps
        GameObject cameraGameObject = GameObject.FindWithTag("PlayerCamera");
        if (cameraGameObject != null) {
            playerCamera = cameraGameObject.GetComponent<Camera>();
        }
        else {
            Debug.LogError("No camera with the tag 'PlayerCamera' was found in the scene.");
        }
    }

    private void SetupTorch() {
        if (playerInstance != null) {
            Torch torch = playerInstance.GetComponentInChildren<Torch>();
            if (torch != null) {
                // Set up the torch (e.g., set default burn out time)
                torch.SetBurnOutTime(BurnOutTime);
            } else {
                Debug.LogError("Torch component not found on the player.");
            }
        } else {
            Debug.LogError("Player instance is null.");
        }
    }
    private void AddTimeTorch(float time) {
        if (playerInstance != null) {
            Torch torch = playerInstance.GetComponentInChildren<Torch>();
            if (torch != null) {
                torch.RestoreLight(time);
            } else {
                Debug.LogError("Torch component not found on the player.");
            }
        } else {
            Debug.LogError("Player instance is null.");
        }
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

    public Maze maze;
    public Player player;    
    
    public void SaveGame()
    {
        if (playerInstance == null)
        {
            Debug.LogError("Player instance is null");
            return;
        }
        
        SaveData saveData = new SaveData();

     //number of level
        saveData.levelNumber = level;
     //player position   
        IntVector2 position = mazeInstance.GetCurrentCell(new Vector3(playerInstance.transform.position.x, playerInstance.transform.position.y, playerInstance.transform.position.z));
        saveData.playerPositionX = position.x;
        saveData.playerPositionZ = position.z; 
     //number of keyes
        saveData.numberOfCollectedKeyes = playerInstance.keysCollected;
        
        PlayerHealth playerHealth = playerInstance.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            saveData.playerHealth = playerHealth.CurrentHealth();
        }
        
        saveLevelInstance.Save(saveData);

    }
    /*
    public void LoadGame()
    {
        SaveData saveData = saveLevelInstance.Load();
        if (saveData != null)
        {
            level = saveData.levelNumber;
            if (playerInstance != null)
            {
                playerInstance.transform.position = new Vector3(saveData.getPositionX(), saveData.getPositionY(), saveData.getPositionZ());
                playerInstance.transform.rotation = Quaternion.identity;
                playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2((int)(saveData.getPositionX()), (int)(saveData.getPositionY()))));
                
                //playerInstance.keysCollected = saveData.getKeyes(); 

                PlayerHealth playerHealth = playerPrefab.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.RestoreHealth(saveData.playerHealth);
                    if (DeathScreen.Instance != null)
                    {
                        DeathScreen.Instance.HideDeathScreen();
                    }
                }  
                torchInstance.SetBurnOutTime(60);  
            }
            else
            {
                Debug.LogError("Player instance is null.");
            }
        }
    }
    */

    public void LoadGame()
    {
        SaveData saveData = saveLevelInstance.Load();
        if (saveData != null)
        {
            level = saveData.levelNumber;
            if (playerInstance != null)
            {
                if(saveData.getPositionX() >= 0 && saveData.getPositionX() < mazeInstance.size.x &&
                    saveData.getPositionZ() >= 0 && saveData.getPositionZ() < mazeInstance.size.z)
                {
                    playerInstance.SetLocation(mazeInstance.GetCell(new IntVector2((int)(saveData.getPositionX()), (int)(saveData.getPositionZ()))));

                    // Update this line to get the PlayerHealth component from playerInstance
                    PlayerHealth playerHealth = playerInstance.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.RestoreHealth(saveData.playerHealth);
                        if (DeathScreen.Instance != null)
                        {
                            DeathScreen.Instance.HideDeathScreen();
                        }
                    }
                    else
                    {
                        Debug.LogError("PlayerHealth component not found on the player instance.");
                    }
                }
                else{
                    ErrorDisplay();
                    Debug.Log("Show message of saved data");
                }
                
            }
            else
            {
                Debug.LogError("Player instance is null.");
            }
        }
    }
    
    public void ErrorDisplay()
    {
        Time.timeScale = 0f;
        ErrorUI.SetActive(true);
        stopped = true;
    }


    public void ErrorHide()
    {
        Time.timeScale = 1f;
        ErrorUI.SetActive(false);
        stopped = false;
    }

    private void PlaceEntryAndExitRooms() {

        //entry room
        Quaternion entryRotation = Quaternion.Euler(0, 90, 0); // Adjust this as needed
        MazeCell entryCell = mazeInstance.GetCell(new IntVector2(0, 0));
        entry = Instantiate(entryRoomPrefab, entryCell.transform.position, entryRotation);

        //exit room
        Quaternion exitRotation = Quaternion.Euler(0, -90, 0); // Adjust this as needed
        MazeCell exitCell = mazeInstance.GetCornerCell(new IntVector2(mazeInstance.size.x - 1, mazeInstance.size.z - 1)); // Top-right corner
        exit = Instantiate(exitRoomPrefab, exitCell.transform.position, exitRotation);

        mazeInstance.RemoveWallsAtCoordinates(new IntVector2(1, 0));
        mazeInstance.RemoveWallsAtCoordinates1( new IntVector2(mazeInstance.size.x - 2, mazeInstance.size.z - 1));

    }
    

    
    private void SpawnZombies(int count) {
        for (int i = 0; i < count; i++) {
            MazeCell cell = null;
            float distance;
            do {
                cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
                distance = Vector3.Distance(playerInstance.transform.position, cell.transform.position);
            } while (distance < minSpawnDistanceFromPlayer);

            Zombie zombieInstance = Instantiate(zombiePrefab, cell.transform.position, Quaternion.identity) as Zombie;
            zombieInstance.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f); // Adjust these values as needed
        }
    }

    private void SpawnFastZombies(int count) {
        for (int i = 0; i < count; i++) {
            MazeCell cell = null;
            float distance;
            do {
                cell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
                distance = Vector3.Distance(playerInstance.transform.position, cell.transform.position);
            } while (distance < minSpawnDistanceFromPlayer);

            zombiefInstance = Instantiate(zombiefPrefab, cell.transform.position, Quaternion.identity) as ZombieFast;
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
    
    /**/
    private void SpawnLighter(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 lighterPosition = randomCell.transform.position;
            float floatHeight = 0.2f; // The height above the ground at which the key will float
            lighterPosition.y += floatHeight;

            
            lighter = Instantiate(lighterPrefab, lighterPosition, Quaternion.identity);
            lighter.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }
    private void SpawnMatches(int count)
    {
        for (int i = 0; i < count; i++)
        {
            MazeCell randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);
            Vector3 matchPosition = randomCell.transform.position;
            float floatHeight = 0.001f; // The height above the ground at which the key will float
            matchPosition.y += floatHeight;

            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            match = Instantiate(matchPrefab, matchPosition, rotation);
            match.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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
            if (storyTrigger != null && i < 6)
            {
                storyTrigger.number = i;
            }
        }
    }

    /**/
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void NewLevel()
    {
        //check if this is The End
        int endLvl = saveLevelInstance.GetEndLevelNumber();
        Debug.Log($"EndLvl: {endLvl}");
        if(level > endLvl) { SceneManager.LoadScene("DockThingEnd"); }
        //save the number of current level
        saveLevelInstance.SaveLvl(level + 1);
        //delete position, health and keyes after level (prevention for smaller maze)
        //saveLevelInstance.DeleteSaveWhenNewLevel();

        RestartGame();

    }




}