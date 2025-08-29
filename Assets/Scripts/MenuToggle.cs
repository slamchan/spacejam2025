using UnityEngine;
using UnityEngine.UI; // For Toggle

public class MenuToggle : MonoBehaviour
{
    public Toggle toggleButton;        // Reference to the Toggle UI element
    private const string TogglePref = "ToggleState"; // Key for PlayerPrefs

    private void Start()
    {
        // Load the saved toggle state (default to false if not set)
        bool savedState = PlayerPrefs.GetInt(TogglePref, 0) == 1;
        toggleButton.isOn = savedState; // Set the toggle's state

        // Set the toggle's state and update the global value
        toggleButton.isOn = savedState;

        // Add listener to handle changes to the toggle state
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool value)
    {
        // Save the state to PlayerPrefs
        PlayerPrefs.SetInt(TogglePref, value ? 1 : 0);
        PlayerPrefs.Save(); // Ensure it's written to disk
    }
}
