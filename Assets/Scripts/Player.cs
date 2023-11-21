using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
/*
    private MazeCell currentCell;
    private MazeCell targetCell;
    private MazeDirection currentDirection;
    private Quaternion targetRotation;

    private float moveSpeed = 1f;
    //private float rotationSpeed = 120f;
    //private bool useMouseRotation = true;
    private Vector3 targetPosition;
    private bool isMoving;
    private bool isRotating;

    //private float moveThreshold = 0.1f;
    public int keysCollected = 0;
    private HUDManager hudManager;


    [Range(10f, 100f)]
    public float mouseSensitivity = 70f;
    private float pitch = 0f; // Add this variable at the class level
    public float verticalMouseSensitivity = 2.0f;
    public float maxSpeed = 3f;
    public float speed = 1.5f;

    public float doorOpenDistance = 2.0f; // Max distance to open doors
    public LayerMask doorLayer; // Layer mask to detect doors

*/

    public int keysCollected = 0;
    private HUDManager hudManager;
    private MazeCell currentCell;
    private MazeCell targetCell;
    private MazeDirection currentDirection;
    private Quaternion targetRotation;

    private float moveSpeed = 1f;
    private float rotationSpeed = 120f;
    private bool useMouseRotation = true;
    private Vector3 targetPosition;
    private bool isMoving;
    private bool isRotating;

    private float moveThreshold = 0.1f;
    private int keyCount = 0;


    private void Move(MazeDirection direction)
    {
        if (isMoving)
        {
            return;
        }

        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            targetCell = edge.otherCell;
            targetPosition = targetCell.transform.localPosition;
            isMoving = true;
        }
    }


    private void Look(MazeDirection direction)
    {
        if (isRotating)
        {
            return;
        }

        currentDirection = direction;
        targetRotation = direction.ToRotation();
        isRotating = true;
    }

    private void Update()
    {
        if (!isMoving)
        {
            CheckInput();
        }
        if (useMouseRotation)
        {
            CheckMouseRotation();
        }

        UpdateTargetPositionBasedOnInput();
        MoveTowardsTargetSmooth();
        if (isRotating)
        {
            RotateTowardsTarget();
        }
    }

    private void CheckMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (Mathf.Abs(mouseX) > 0)
        {
            transform.Rotate(0f, mouseX * rotationSpeed * Time.deltaTime, 0f);
            UpdateCurrentDirection();
        }
        if (Mathf.Abs(mouseY)> 0)
        {
            transform.Rotate(0f, mouseX * rotationSpeed * Time.deltaTime, 0f);
            UpdateCurrentDirection();
        }
    }

    private void UpdateCurrentDirection()
    {

        Vector3 forward = transform.forward;
        forward.y = 0;
        if (Vector3.Angle(forward, Vector3.forward) <= 45.0f)
        {
            currentDirection = MazeDirection.North;
        }
        else if (Vector3.Angle(forward, Vector3.right) <= 45.0f)
        {
            currentDirection = MazeDirection.East;
        }
        else if (Vector3.Angle(forward, Vector3.back) <= 45.0f)
        {
            currentDirection = MazeDirection.South;
        }
        else if (Vector3.Angle(forward, Vector3.left) <= 45.0f)
        {
            currentDirection = MazeDirection.West;
        }
    }



    private void RotateTowardsTarget()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    private void UpdateTargetPositionBasedOnInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            UpdateMovement(currentDirection);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            UpdateMovement(currentDirection.GetNextClockwise());
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            UpdateMovement(currentDirection.GetOpposite());
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            UpdateMovement(currentDirection.GetNextCounterclockwise());
        }
    }
    private void UpdateMovement(MazeDirection direction)
    {
        if (isMoving)
        {
            return;
        }

        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            targetCell = edge.otherCell;
            targetPosition = targetCell.transform.localPosition;
            isMoving = true; // Now we're ready to move towards the target.
        }
    }

    public void AddKey(Key key)
    {
        hudManager = FindObjectOfType<HUDManager>();
        keysCollected++;
        hudManager.UpdateKeyDisplay();

    }

    private void MoveTowardsTargetSmooth()
    {
        if (!isMoving)
        {
            return;
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

        
        float distanceToTarget = Vector3.Distance(transform.localPosition, targetPosition);
        //Debug.Log($"Distance to target: {distanceToTarget}");

        if (distanceToTarget <= moveThreshold)
        {
            SetLocation(targetCell);
            isMoving = false;
        }
    }



    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(currentDirection);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(currentDirection.GetNextClockwise());
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(currentDirection.GetOpposite());
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(currentDirection.GetNextCounterclockwise());
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Look(currentDirection.GetNextCounterclockwise());
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Look(currentDirection.GetNextClockwise());
        }
    }

    //for doors
    //private MazeCell currentCell;

	public void SetLocation (MazeCell cell) {
		if (currentCell != null) {
			currentCell.OnPlayerExited();
		}
		currentCell = cell;
		transform.localPosition = cell.transform.localPosition;
		currentCell.OnPlayerEntered();
	}
}

/*
    float horizontal;  //left to righ controlls (A & D keys)
    float vertical;    //up and down controllers (W & S keys)
    float mouseX;
    float mouseY;
    Quaternion deltaRotation;
    Vector3 deltaPosition;

    Rigidbody rbody;
    //Transform _cameraTransform;


    private void Start(){
        rbody = GetComponent<Rigidbody>();
        //_cameraTransform = Camera.main.transform;
    }

    private void Update(){
        GetInputs();

        //CheckForDoors();
    }

    private void FixedUpdate(){ //update in real time, consistently
        deltaRotation = Quaternion.Euler(Vector3.up * mouseX * mouseSensitivity * Time.fixedDeltaTime);
        rbody.MoveRotation(rbody.rotation * deltaRotation); 

        deltaPosition = ((transform.forward * vertical) + (transform.right * horizontal)) * Time.fixedDeltaTime;
        rbody.MovePosition(rbody.position + deltaPosition);

        // Check for running input (e.g., holding down Left Shift key)
        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = maxSpeed;
        }
        else {
            moveSpeed = speed; // Or your default move speed
        }

        // Calculate position change and apply it
        deltaPosition = ((transform.forward * vertical) + (transform.right * horizontal)) * moveSpeed * Time.fixedDeltaTime;
        rbody.MovePosition(rbody.position + deltaPosition);
    }

    void GetInputs(){
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void CheckForDoors() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, doorOpenDistance, doorLayer)) {
            MazeDoor door = hit.collider.GetComponent<MazeDoor>();
            if (door != null && Input.GetKeyDown(KeyCode.E)) { // Press E to open the door
                door.OpenDoor(); // This should no longer cause an error
            }
        }
    }


    /*
    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        currentCell.OnPlayerEntered();
        targetCell = null;
        isMoving = false;
    }

    public void AddKey(Key key)
    {
        keysCollected++;
        // Update the HUD using the reference to HUDManager
        if (hudManager != null)
        {
            hudManager.UpdateKeyDisplay();
        }
    }
}*/

public class PlayerSpawn : MonoBehaviour
{
    public float groundClearance = 1.0f; // Space between player and ground

    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            // Place the player slightly above the ground hit point
            transform.position = new Vector3(transform.position.x, hit.point.y + groundClearance, transform.position.z);
        }
    }
}