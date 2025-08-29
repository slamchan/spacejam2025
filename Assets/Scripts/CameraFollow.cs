using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // Reference to the player's Transform
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public Vector3 offset;            // Offset from the player (to make the camera follow at a specific distance)
    public float zoomOutAmount = 5f;  // Amount to zoom out (larger values = more zoomed out)
    public float fixedYPosition = 3f; // The fixed Y position for the camera

    private Camera cam;               // Camera component reference

    void Start()
    {
        // Get the Camera component on the same GameObject
        cam = GetComponent<Camera>();
        
        // Set the initial camera position further away on the Y-axis (or adjust as needed)
        Vector3 initialPosition = transform.position;
        initialPosition.y = fixedYPosition; // Set the Y position to the fixed value
        transform.position = initialPosition;
    }

    void LateUpdate()
    {
        // Check if player is assigned
        if (player != null)
        {
            // Desired position of the camera (player position + offset)
            Vector3 desiredPosition = player.position + offset;
            
            // Lock the camera's Y position (keep it at fixedYPosition)
            desiredPosition.y = fixedYPosition; 

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Update the camera's position
            transform.position = smoothedPosition;

            // Zoom out the camera (adjust orthographic size)
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomOutAmount, smoothSpeed);
        }
    }
}
