using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform mapImage;     // The minimap background
    public RectTransform player1Icon;  // Red dot

    [Header("Players")]
    public Transform player1;          // Reference to player 1

    [Header("World Size")]
    public float halfWorldWidth = 10f;   // same as LoopingWorld script
    public float halfWorldHeight = 5f;   // for vertical space

    void Update()
    {
        UpdateIcon(player1, player1Icon);
    }

    void UpdateIcon(Transform player, RectTransform icon)
    {
        // Normalize player position (-1 .. +1)
        float normX = Mathf.Clamp(player.position.x / halfWorldWidth, -1f, 1f);
        float normY = Mathf.Clamp(player.position.y / halfWorldHeight, -1f, 1f);

        // Convert normalized coords → minimap rect
        float mapHalfW = mapImage.rect.width * 0.5f;
        float mapHalfH = mapImage.rect.height * 0.5f;

        float posX = normX * mapHalfW;
        float posY = normY * mapHalfH;

        icon.localPosition = new Vector3(posX, posY, 0f);
    }
}
