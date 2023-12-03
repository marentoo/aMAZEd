using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow (usually the player)
    public Vector3 offset; // The offset distance between the camera and the player
    public float smoothSpeed = 0.125f; // The smoothing factor
    public float height = 10f;

    void FixedUpdate()
    {

        // First, check if the 'target' is not null
        if(target == null)
        {
            // Optionally, you can log an error or a warning here if needed
            //Debug.Log("CameraFollow target is not set.");
            return; // Exit the function if there is no target to follow
        }

        // Define a position the camera should move towards (target position with an offset)
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        //transform.position = smoothedPosition;
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        desiredPosition.y = height;

        // Optionally, you can make the camera look at the target always
        transform.LookAt(target);
    }

    // Call this method to initiate camera following with a new target
    public void StartFollowingPlayer(Transform newTarget)
    {
        target = newTarget; // Assign the new target
        // You could also add additional logic here if needed
    }

    // Call this method to stop the camera from following the target (e.g., when the player is destroyed)
    public void StopFollowingPlayer()
    {
        target = null; // Remove the target
    }
}
