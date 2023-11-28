using UnityEngine;

public class ElevatorDoorsController : MonoBehaviour
{
    public Transform doorLeft; // Assign in inspector, drag the left door GameObject
    public Transform doorRight; // Assign in inspector, drag the right door GameObject
    public Transform door2Left; // Assign in inspector, drag the left door GameObject
    public Transform door2Right; // Assign in inspector, drag the right door GameObject

    public float openSpeed = 0.5f; // The speed at which the doors open
    public float openDistance = 0.4f; // How far the doors should slide open
    public float openDistance2 = 0.3f; // How far the doors should slide open

    private Vector3 leftDoorClosedPosition;
    private Vector3 rightDoorClosedPosition;
    private Vector3 leftDoor2ClosedPosition;
    private Vector3 rightDoor2ClosedPosition;
    
    private Vector3 leftDoorOpenPosition;
    private Vector3 rightDoorOpenPosition;
    
    private Vector3 leftDoor2OpenPosition;
    private Vector3 rightDoor2OpenPosition;

    private bool shouldOpen = false;

    private void Start()
    {
        // Store the initial positions
        leftDoorClosedPosition = doorLeft.localPosition;
        rightDoorClosedPosition = doorRight.localPosition;
        
        leftDoor2ClosedPosition = doorLeft.localPosition;
        rightDoor2ClosedPosition = doorRight.localPosition;

        // Calculate the open positions based on the open distance
        leftDoorOpenPosition = leftDoorClosedPosition + new Vector3(0, 0, openDistance);
        rightDoorOpenPosition = rightDoorClosedPosition + new Vector3(0, 0, -openDistance);
        leftDoor2OpenPosition = leftDoor2ClosedPosition + new Vector3(0, 0, openDistance);
        rightDoor2OpenPosition = rightDoor2ClosedPosition + new Vector3(0, 0, -openDistance);
    }

    private void Update()
    {
        if (shouldOpen)
        {
            // Move the doors to their open positions
            doorLeft.localPosition = Vector3.MoveTowards(doorLeft.localPosition, leftDoorOpenPosition, openSpeed * Time.deltaTime);
            doorRight.localPosition = Vector3.MoveTowards(doorRight.localPosition, rightDoorOpenPosition, openSpeed * Time.deltaTime);
            
            door2Left.localPosition = Vector3.MoveTowards(doorLeft.localPosition, leftDoor2OpenPosition, openSpeed * Time.deltaTime);
            door2Right.localPosition = Vector3.MoveTowards(doorRight.localPosition, rightDoor2OpenPosition, openSpeed * Time.deltaTime);
        }
    }

    // Call this method from your Player script when the conditions are met
    public void OpenDoors()
    {
        shouldOpen = true;
    }
}
