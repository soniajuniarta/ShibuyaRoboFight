using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Initializing,
        Gameplay,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.Initializing);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Initializing:
                HandleInitializing();
                break;
            case GameState.Gameplay:
                HandleGameplay();
                break;
            case GameState.Paused:
                HandlePaused();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    private void HandleMainMenu()
    {
        Debug.Log("State: Main Menu, Menunggu input pemain.");
    }

    private void HandleInitializing()
    {
        Debug.Log("State: Initializing. Menyiapkan Arena Shibuya, Deck, dan Dadu...");

        ChangeState(GameState.Gameplay);
    }

    private void HandleGameplay()
    {
        Debug.Log("State: Gameplay Dimulai! Menyerahkan kendali ke TurnManager.");
    }

    private void HandlePaused()
    {
        Debug.Log("State: Paused.");
    }

    private void HandleGameOver()
    {
        Debug.Log("State: Game Over! Kalkulasi pemenang tug of");
    }
}
