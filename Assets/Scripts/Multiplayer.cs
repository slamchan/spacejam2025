using UnityEngine;

public class Multiplayer : MonoBehaviour
{
    public bool P2;
    public bool loadedToggleState;

    // Array of assets (GameObjects) that can be enabled/disabled
    public GameObject[] assetsToEnable;
    public GameObject[] assetsToDisable;

    private void Start()
    {
        loadedToggleState = PlayerPrefs.GetInt("ToggleState", 0) == 1;
        loadedToggleState = true;
        P2 = loadedToggleState;
    }

    void Update()
    {

        if (P2 == true)
        {
            foreach (GameObject asset in assetsToEnable)
            {
                asset.SetActive(true); // Enables each asset in the list
            }
            foreach (GameObject asset in assetsToDisable)
            {
            asset.SetActive(false); // Enables each asset in the list
            }
        }
    }
}
