using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	private Maze mazeInstance;

	public Player playerPrefab;
	private Player playerInstance;

	//public Zombie zombiePrefab;
	//private Zombie zombieInstance;

	private void Start () {
		StartCoroutine(BeginGame());
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}

	private IEnumerator BeginGame() {
    Camera.main.clearFlags = CameraClearFlags.Skybox;
    Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

    // Instantiate and generate the maze.
    mazeInstance = Instantiate(mazePrefab) as Maze;
    yield return StartCoroutine(mazeInstance.Generate());

    // Instantiate the player and set their location.
    playerInstance = Instantiate(playerPrefab) as Player;
    playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

	// Instantiate the zombie and set their location.
    //zombieInstance = Instantiate(zombiePrefab) as Zombie;
    //zombieInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

    // Here we assume you have a reference to the CameraFollow script component on the camera.
    // Make sure to assign this in the Unity Editor.
    CameraFollow cameraFollowScript = Camera.main.GetComponent<CameraFollow>();
    if (cameraFollowScript != null) {
        cameraFollowScript.StartFollowingPlayer(playerInstance.transform);
    } else {
        Debug.LogError("CameraFollow script not found on the main camera!");
    }

    // Adjust the camera settings as required.
    Camera.main.clearFlags = CameraClearFlags.Depth;
    Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.5f);
}


/*//previous version
	private IEnumerator BeginGame () {
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
		mazeInstance = Instantiate(mazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate());
		playerInstance = Instantiate(playerPrefab) as Player;
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(0f, 0f, 0.3f, 0.4f);
	}*/

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
			//Destroy(zombieInstance.gameObject);
		}
		StartCoroutine(BeginGame());
	}
}