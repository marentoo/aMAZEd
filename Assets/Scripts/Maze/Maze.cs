using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class Maze : MonoBehaviour {

	public IntVector2 size; // This holds the dimensions of the maze

	public MazeCell cellPrefab;

	public float generationStepDelay;

	public MazePassage passagePrefab;

	public MazeDoor doorPrefab;

	[Range(0f, 1f)]
	public float doorProbability;

	public MazeWall[] wallPrefabs;

	public MazeRoomSettings[] roomSettings;

	private MazeCell[,] cells;

	private List<MazeRoom> rooms = new List<MazeRoom>();

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	//new
	public MazeCell GetCornerCell(IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	
	public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			yield return delay;
			DoNextGenerationStep(activeCells);
		}
		//for hiding rooms
		/*
		for (int i = 0; i < rooms.Count; i++) {
			rooms[i].Hide();
		}*/
		
	}
	

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		MazeCell newCell = CreateCell(RandomCoordinates);
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) { //for creating expanded room, without additional walls
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	[Range(0f, 1f)]
	public float roomExpansionChance;	// Chance to expand the room by removing walls between cells

	/**/
	private void DoNextGenerationStep(List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				// Create a passage with a random chance of expanding the room.
				if (Random.value < roomExpansionChance) { // roomExpansionChance determines how often rooms are expanded.
					CreatePassageInSameRoom(currentCell, neighbor, direction);
					neighbor.Initialize(currentCell.room); // Initialize with the current room
				} else {
					CreatePassage(currentCell, neighbor, direction);
				}
				activeCells.Add(neighbor);
			} else {
				if (currentCell.room == neighbor.room) {
					// Random chance to expand the room instead of creating a wall.
					if (Random.value < roomExpansionChance) {
						CreatePassageInSameRoom(currentCell, neighbor, direction);
					} else {
						CreateWall(currentCell, neighbor, direction);
					}
				} else {
					CreateWall(currentCell, neighbor, direction);
				}
			}
		} else {
			CreateWall(currentCell, null, direction);
		}
	}

	public void RemoveWallsAtCoordinates(params IntVector2[] coordinatesArray) {
		foreach (IntVector2 coordinates in coordinatesArray) {
			MazeCell cell = GetCell(coordinates);
			if (cell != null) {
				// Assuming each cell has a method to remove walls or an array/list of walls
				cell.RemoveWalls();
			}
		}
	}
	public void RemoveWallsAtCoordinates1(params IntVector2[] coordinatesArray) {
		foreach (IntVector2 coordinates in coordinatesArray) {
			MazeCell cell = GetCell(coordinates);
			if (cell != null) {
				// Assuming each cell has a method to remove walls or an array/list of walls
				cell.RemoveWalls1();
			}
		}
	}



	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}
	/*
	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;
		if (passage is MazeDoor) {
			otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
		}
		else {
			otherCell.Initialize(cell.room);
		}
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}
	*/
	
	private List<MazeDoor> doorsInRoom = new List<MazeDoor>(); // Track doors in the current room
	
	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
        MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MazePassage;
        if (passage is MazeDoor) {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
            doorsInRoom.Add(passage as MazeDoor); // Add door to the list
        }
        else {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }



	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) {
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		}
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

	//for adding the lift to maze map
	//public MazeCell elevatorCellPrefabEnter; // Assign the prefab for the elevator cell in the inspector
	//public MazeCell elevatorCellPrefabExit; // Assign the prefab for the elevator cell in the inspector

	// This method is called after the standard maze generation is complete
	/*
	public void AddElevatorCells() {
		// Assume 'size' is the size of your maze, and 'elevatorCellPrefab' is assigned.
		PlaceElevatorCell(new IntVector2(-1, 0));

		// Place the elevator at (size.x, size.z - 1), which might be (instance+1, instance-1)
		PlaceElevatorCell(new IntVector2(size.x, size.z - 1));
	}*/
	/*
	public void PlaceElevatorCell(IntVector2 coordinates, bool isExit) { //, MazeDirection directionToMaze
		// Instantiate the elevator cell at the given coordinates
		MazeCell elevatorCellInstance = Instantiate(elevatorCellPrefabEnter) as MazeCell;
		if(isExit)
		{
			elevatorCellInstance = Instantiate(elevatorCellPrefabExit) as MazeCell;
		}
		//MazeCell elevatorCellInstance = Instantiate(elevatorCellPrefabExit) as MazeCell;
		elevatorCellInstance.transform.parent = transform; // Set the parent to the maze transform
		elevatorCellInstance.transform.localPosition = new Vector3(coordinates.x, 0, coordinates.z);

		// Optionally, if you need to connect the elevator cell to the maze, you can call ConnectElevatorToMaze here
		ConnectElevatorToMaze(coordinates); //, directionToMaze
	}

	// Call this method after the maze generation is complete
	public void ConnectElevatorToMaze(IntVector2 elevatorCoordinates) { //, MazeDirection directionToMaze
		// Determine the direction of the maze cell that should connect to the elevator
		// For example, if the elevator is at (-1,0), the direction would be East to connect to (0,0)
		MazeDirection directionToMaze = MazeDirection.North; // This is an example; you'll need to determine this based on your elevator's location

		// The coordinates of the maze cell adjacent to the elevator cell
		IntVector2 adjacentCoordinates = elevatorCoordinates + MazeDirections.ToIntVector2(directionToMaze);

		// Check if the adjacent coordinates are within the maze bounds
		if (ContainsCoordinates(adjacentCoordinates)) {
			// Get the maze cell adjacent to the elevator
			MazeCell adjacentCell = GetCell(adjacentCoordinates);

			// Create a passage between the adjacent cell and the elevator
			// This will depend on your MazeCell implementation; here's a conceptual example:
			//CreatePassage(adjacentCell, elevatorCellPrefabEnter, directionToMaze.GetOpposite());
			//CreatePassage(adjacentCell, elevatorCellPrefabExit, directionToMaze.GetOpposite());
		}
	}*/
}

