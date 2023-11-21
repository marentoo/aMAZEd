using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

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

    /**/
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