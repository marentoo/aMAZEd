using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    public int keysCollected = 0;
    private GameManager gameManager;
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
    //private int keyCount = 0;

    private Animator scavengerAnimator;
    private ElevatorDoorsController elevatorDoorsController;

    private void Start()
    {
        // Find the Scavenger Variant child object
        Transform scavengerVariantTransform = transform.Find("Scavenger Variant");
        if (scavengerVariantTransform != null)
        {
            // Get the Animator component from the Scavenger Variant
            scavengerAnimator = scavengerVariantTransform.GetComponent<Animator>();
        }

        scavengerAnimator = GetComponent<Animator>();

        if (scavengerAnimator == null)
        {
            Debug.LogError("Animator not found on Scavenger Variant");
        }


        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        // Find the ElevatorDoorsController in the scene (assuming there's only one)
        elevatorDoorsController = FindObjectOfType<ElevatorDoorsController>();
        if (elevatorDoorsController == null)
        {
            Debug.LogWarning("ElevatorDoorsController script not found in the scene.");
        }


    }

    private void FixedUpdate()
    {

    }

    private void Move(MazeDirection direction)
    {
        if (isMoving)
        {
            return;
        }

        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            //scavengerAnimator.SetTrigger("Walk");
            targetCell = edge.otherCell;
            targetPosition = targetCell.transform.localPosition;
            isMoving = true;
        }
    }

    private void Animate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            scavengerAnimator.SetTrigger("Walk");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            scavengerAnimator.SetTrigger("Walk");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            scavengerAnimator.SetTrigger("Back");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            scavengerAnimator.SetTrigger("Back");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            scavengerAnimator.SetTrigger("Walk");
        }

        else if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.DownArrow)) ||
                 (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.UpArrow)) ||
                 (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) ||
                 (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)))
        {
            // Stop walking immediately
            scavengerAnimator.SetTrigger("StopWalking");
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
        Animate();

        if (IsNearElevator() && HasCollectedRequiredKeys())
        {
            Debug.Log("Collected half of keyes!");
            OpenElevatorDoor();
        }

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
        if (Mathf.Abs(mouseY) > 0)
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
            //scavengerAnimator.SetTrigger("Walk");
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
            //scavengerAnimator.SetTrigger("StopWalking");
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

    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        currentCell.OnPlayerEntered();
    }

    private bool IsNearElevator()
    {
        // Assuming the elevator is at cell (1,0)
        MazeCell elevatorCell = gameManager.mazeInstance.GetCell(
            new IntVector2(gameManager.mazeInstance.size.x - 1, gameManager.mazeInstance.size.z - 1));
        float distanceToElevator = Vector3.Distance(transform.position, elevatorCell.transform.position);

        // Define a suitable threshold distance
        return distanceToElevator <= 2.0f;

        Debug.Log("Player near doors.");
    }

    private bool HasCollectedRequiredKeys()
    {
        return keysCollected >= GameManager.numberOfKeys / 2;
    }


    private void OpenElevatorDoor()
    {
        Debug.Log("Doors are opening.");
        if (elevatorDoorsController != null)
        {
            elevatorDoorsController.OpenDoors();
        }
    }



    

}

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