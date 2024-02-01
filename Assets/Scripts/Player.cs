using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{

    public int keysCollected = 0;
    private GameManager gameManager;
    public Camera playerCamera;

    private StoryDisplay storyDisplay;
    private HUDManager hudManager;
    private MazeCell currentCell;
    private MazeCell targetCell;
    private MazeDirection currentDirection;
    private Quaternion targetRotation;
    private TextManager textManager;

    private float moveSpeed = 1f;
    private float rotationSpeed = 120f;
    private bool useMouseRotation = true;
    private Vector3 targetPosition;
    private bool isMoving;
    private bool isRotating;

    private float distanceToElevator;

    public int lightersCollected = 0;
    public int matchesCollected = 0;
    private Torch torch;
    private SaveLevel saveLevelInstance;
    private Lighter lighter;
    private Match match;

    private float moveThreshold = 0.1f;
    //private int keyCount = 0;

    private Animator scavengerAnimator;
    private ElevatorDoorsController elevatorDoorsController;

    private void Start()
    {
        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor from view
        Cursor.visible = false;

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

        storyDisplay = FindObjectOfType<StoryDisplay>();

        GameObject cameraGameObject = GameObject.FindWithTag("PlayerCamera");
        if (cameraGameObject != null) {
            playerCamera = cameraGameObject.GetComponent<Camera>();
        }

        torch = FindObjectOfType<Torch>();
        lighter = FindObjectOfType<Lighter>();
        match = FindObjectOfType<Match>();
        
        saveLevelInstance = new SaveLevel();
        //RestoreLighters();
        RestoreMatches();

        
        rbody = GetComponent<Rigidbody>();
        //Rigidbody rb = GetComponent<Rigidbody>();
        rbody.constraints = RigidbodyConstraints.FreezePositionY | rbody.constraints; // This line keeps the existing constraints and adds a constraint on the Y position.


    }

    Quaternion deltaRotation;
    Vector3 deltaPosition;
    Rigidbody rbody;
    float horizontal;  //left to righ controlls (A & D keys)
    float vertical;    //up and down controllers (W & S keys)
    float mouseX;
    float mouseY;

    [Range(10f, 100f)]
    public float mouseSensitivity = 70f;
    //private float pitch = 0f; // Add this variable at the class level
    public float verticalMouseSensitivity = 2.0f;
    public float maxSpeed = 3f;
    public float speed = 1.5f;


    private void FixedUpdate()
    {
        rbody.constraints = RigidbodyConstraints.FreezePositionY | rbody.constraints;
        rbody.constraints = RigidbodyConstraints.FreezeRotationY | rbody.constraints;
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



/*    private void Animate()
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
*/

    private void Look(MazeDirection direction)
    {
        float mouseX = Input.GetAxis("Mouse X"); // Horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse movement
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
        //Animate();
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
         if (Input.GetMouseButtonDown(1)) //left click = 0; right clisk = 1
        {
            //Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            // Hide the cursor from view
            Cursor.visible = false;
        }

        if(IsNearElevator() && !HasCollectedRequiredKeys()){
            storyDisplay.message = "Collect ALL keyes!";
            storyDisplay.DisplayMessage();
        }

        if (IsNearElevator() && HasCollectedRequiredKeys())
        {
            Debug.Log("Collected half of keyes!");
            OpenElevatorDoor();
            if (distanceToElevator < 1){
                storyDisplay.message = "Press ENTER to continue";
                storyDisplay.DisplayMessage();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameManager.NewLevel(); // Call the NextLevel method when Enter is pressed
                }
                else
                {
                    //textManager.HideText();
                }
            }
        }
        
        if (!isMoving)
        {
            CheckInput();
        }/**/
        //GetInputs();
        
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

        //Use lighter
        /*
        if (Input.GetKeyDown(KeyCode.L) && lightersCollected > 0) {
            UseLighter(torch);
        }*/
        //Use match
        if (Input.GetKeyDown(KeyCode.L) && matchesCollected > 0) {
            UseMatch(torch);
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
    }



    public bool IsNearElevator()
    {
        // Assuming the elevator is at cell (1,0)
        MazeCell elevatorCell = gameManager.mazeInstance.GetCell(
            new IntVector2(gameManager.mazeInstance.size.x - 2, gameManager.mazeInstance.size.z - 1));
        distanceToElevator = Vector3.Distance(transform.position, elevatorCell.transform.position);

        return distanceToElevator <= 2.0f; 

    }

    private bool HasCollectedRequiredKeys()
    {
        return keysCollected >= GameManager.numberOfKeys;
    }


    private void OpenElevatorDoor()
    {
        Debug.Log("Doors are opening.");
        if (elevatorDoorsController != null)
        {
            elevatorDoorsController.OpenDoors();
        }
    }


    public void AddKey(Key key)
    {
        hudManager = FindObjectOfType<HUDManager>();
        keysCollected++;
        hudManager.UpdateKeyDisplay();

    }

    public float fallDuration = 2.0f; // Time in seconds over which the fall occurs
    public float nearGroundYPosition = 0.2f; // The Y position near the ground

    public void Death(){
        scavengerAnimator.SetTrigger("Death");

        torch.SetBurnOutTime(0.2f);
        
        //useMouseRotation = false;
        /*
        float elapsedTime = 0;
        Vector3 startPosition = transform.position; // Camera's current position
        Quaternion startRotation = transform.rotation; // Camera's current rotation

        // Keep the same X and Z positions but change Y to near-ground level
        Vector3 endPosition = new Vector3(startPosition.x, nearGroundYPosition, startPosition.z);
        // Rotate to look straight up
        Quaternion endRotation = Quaternion.Euler(90f, 0f, 0f);

        while (elapsedTime < fallDuration) {
            float ratio = elapsedTime / fallDuration;

            // Interpolate position and rotation over time
            transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, ratio);

            //elapsedTime += Time.deltaTime;
        }

        // Ensure final position and rotation are set
        transform.position = endPosition;
        transform.rotation = endRotation;
        */
    }

 //Lighters
 /*   public void AddLighter(Lighter lighter) {
        
        Debug.Log($"Lighters: {lightersCollected}");
        lightersCollected++;
        saveLevelInstance.addLighterToInventory();
        
        hudManager = FindObjectOfType<HUDManager>();
        hudManager.UpdateLighterDisplay();
        // Update UI or other game elements if necessary
    }

    public void UseLighter(Torch torch) {
        if (lightersCollected > 0) {
            lighter.UseLighter(torch);
            lightersCollected--;
            saveLevelInstance.removeighterToInventory();
            
            hudManager = FindObjectOfType<HUDManager>();
            hudManager.UpdateLighterDisplay();
            // Update UI or other game elements if necessary
        }
    }

    public void RestoreLighters(){        
        lightersCollected = saveLevelInstance.loadLighterNumber();
        
        hudManager = FindObjectOfType<HUDManager>();
        hudManager.UpdateLighterDisplay();
    }
    */
    public void AddMatch(Match match) {
        
        Debug.Log($"Matches: {matchesCollected}");
        matchesCollected++;
        saveLevelInstance.addMatchToInventory();
        
        hudManager = FindObjectOfType<HUDManager>();
        hudManager.UpdateMatchesDisplay();
        // Update UI or other game elements if necessary
    }

    public void UseMatch(Torch torch) {
        if (matchesCollected > 0) {
            match.UseMatch(torch);
            matchesCollected--;
            saveLevelInstance.removeMatchToInventory();
            
            hudManager = FindObjectOfType<HUDManager>();
            hudManager.UpdateMatchesDisplay();
            // Update UI or other game elements if necessary
        }
    }

    public void RestoreMatches(){        
        matchesCollected = saveLevelInstance.loadMatchNumber();
        
        hudManager = FindObjectOfType<HUDManager>();
        hudManager.UpdateMatchesDisplay();
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