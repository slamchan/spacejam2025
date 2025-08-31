using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this line

public class GameEndType : MonoBehaviour
{
    [Header("Outcome Sprites")]
    public Sprite player1WinSprite;
    public Sprite player2WinSprite;
    public Sprite baseDeadSprite;

    [Header("UI Reference")]
    public Image endImage; // assign in inspector

    void Start()
    {
        Debug.Log("End state: " + GameManager.endState);
        switch (GameManager.endState)
        {
            case EndGameType.Player1Win:
                endImage.sprite = player1WinSprite;
                break;
            case EndGameType.Player2Win:
                endImage.sprite = player2WinSprite;
                break;
            case EndGameType.BaseDied:
                endImage.sprite = baseDeadSprite;
                break;
        }
    }

    // Call this method to load the GameEnd scene
    public void LoadGameEndScene()
    {
        GameManager.endState = EndGameType.Player1Win; // or Player2Win, BaseDied
        SceneManager.LoadScene("GameEnd");
    }
}
