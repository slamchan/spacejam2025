using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static EndGameType endState;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // persists across scenes
    }

    public static void SetEndGame(EndGameType type)
    {
        endState = type;
    }
}

/*
GameManager.SetEndGame(EndGameType.Player2Win);
                SceneManager.LoadScene("GameEnd");
                */