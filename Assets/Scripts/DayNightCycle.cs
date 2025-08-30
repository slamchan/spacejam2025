using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Camera mainCamera;         // Reference to the main Camera
    public Camera camera2;            // Reference to second Camera
    public Camera camera3;            // Reference to third Camera

    public Color dayColor = Color.blue;  // Color for daytime
    public Color nightColor = Color.black; // Color for nighttime
    public float cycleDuration = 60f; // Duration for one full day-night cycle in seconds
    private float timeOfDay = 0f;      // Tracks time in the cycle
    private float lerpTime = 0f;       // Used to lerp the color smoothly

    void Start()
    {
        // Set the cameras if not manually assigned
        if (mainCamera == null)
            mainCamera = Camera.main;
        // Optionally auto-assign camera2 and camera3 if needed
        // camera2 = ...;
        // camera3 = ...;
    }

    void Update()
    {
        // Update time of day based on real-time or custom logic
        timeOfDay += Time.deltaTime / cycleDuration;

        if (timeOfDay > 1f)
            timeOfDay = 0f; // Reset to 0 when a full cycle is completed

        // Divide the timeOfDay into two halves (night and day)
        if (timeOfDay < 0.5f) 
        {
            // Night Phase: Transition from day to night
            lerpTime = Mathf.Lerp(0f, 1f, timeOfDay * 2); // Smooth transition from day to night
        }
        else
        {
            // Day Phase: Transition from night to day
            lerpTime = Mathf.Lerp(1f, 0f, (timeOfDay - 0.5f) * 2); // Smooth transition from night to day
        }

        // Lerp between day and night colors based on lerpTime
        Color currentColor = Color.Lerp(nightColor, dayColor, lerpTime);

        if (mainCamera != null)
            mainCamera.backgroundColor = currentColor;
        if (camera2 != null)
            camera2.backgroundColor = currentColor;
        if (camera3 != null)
            camera3.backgroundColor = currentColor;
    }
}
