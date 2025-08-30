using UnityEngine;
using UnityEngine.UI;

public class GameEndType : MonoBehaviour
{
    [Header("Outcome Sprites")]
    public Sprite player1WinSprite;
    public Sprite player2WinSprite;
    public Sprite drawSprite;
    public Sprite timeOutSprite;

    [Header("UI Reference")]
    public Image endImage; // assign in inspector

    void Start()
    {
        switch (GameManager.endState)
        {
            case EndGameType.Player1Win:
                endImage.sprite = player1WinSprite;
                break;
            case EndGameType.Player2Win:
                endImage.sprite = player2WinSprite;
                break;
            case EndGameType.Draw:
                endImage.sprite = drawSprite;
                break;
            case EndGameType.TimeOut:
                endImage.sprite = timeOutSprite;
                break;
        }
    }
}
