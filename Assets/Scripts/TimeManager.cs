using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayDuration = 120f; // Seconds for a full 24-hour cycle
    [Range(0,1)] public float currentTime = 0f; // 0 = midnight, 0.5 = noon

    [Header("Sun & Lighting")]
    public Light directionalLight; // Assign your "Sun" light
    public Gradient lightColor;    // Color of the sun over time
    public AnimationCurve lightIntensity; // Intensity curve over time

    private void Update()
    {
        // Advance time
        currentTime += Time.deltaTime / dayDuration;
        if (currentTime > 1f) currentTime -= 1f;

        UpdateSun();
    }

    private void UpdateSun()
    {
        // Rotate sun: 0 = midnight, 180 = noon
        float angle = currentTime * 360f - 90f; // shift so 0 = midnight
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 170f, 0));

        // Update color and intensity
        directionalLight.color = lightColor.Evaluate(currentTime);
        directionalLight.intensity = lightIntensity.Evaluate(currentTime);
    }
}
