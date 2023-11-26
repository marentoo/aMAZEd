using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MazeDoor : MazePassage {

	public Transform hinge;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}
	
	private static Quaternion
		normalRotation = Quaternion.Euler(0f, -90f, 0f),
		mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

	private bool isMirrored;

	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor != null) {
			isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}

	public override void OnPlayerEntered () {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation =
			isMirrored ? mirroredRotation : normalRotation;
		//OtherSideOfDoor.cell.room.Show(); //for hiding rooms
	}


	public override void OnPlayerExited () {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
		//OtherSideOfDoor.cell.room.Hide(); //for hiding rooms
	}

	public bool isOpen;

/*
	// This method will change the state of isOpen and rotate the hinge to open the door
    public void OpenDoor() {
        SetOpen(true); // Set the door state to open
        hinge.localRotation = Quaternion.Euler(0f, -90f, 0f); // Rotate the hinge to open the door
		Debug.Log("Door has been opened!"); // Placeholder action
    } 

	public void OpenDoor() {
    // Assuming the door has an Animator component to play the opening animation
    Animator animator = GetComponent<Animator>();
    if (animator != null) {
        animator.SetTrigger("Open");
    }
    // Disable the collider so the player can walk through
    Collider collider = GetComponent<Collider>();
    if (collider != null) {
        collider.enabled = false;
    }
}*/

    // Call this method to open or close the door
    public void SetOpen(bool open) {
        isOpen = open;
        // You can add animations or other visual changes here when the door state changes
    }
	
	void Start() {
		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.isTrigger = true; // Set the collider as a trigger
	}

	
	

	
}

