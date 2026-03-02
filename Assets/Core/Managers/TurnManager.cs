using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public enum TurnPhase
    {
        TurnStart,
        CardDrifting,
        FirstRoll,
        RerollPhase,
        Resolution,
        TugOfWarUpdate,
        TurnEnd
    }

    public TurnPhase CurrentPhase { get; private set; }

    public int CurrentPlayerIndex { get; private set; }

    private int currentRerollCount = 0;
    private const int MAX_REROLLS = 0;

    public static event Action<TurnPhase> OnPhaseChanged;
    public static event Action<int> OnPlayerTurnChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() //Start()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        DiceManager.OnAllDiceStopped += HandleAllDiceStopped;
    }

    private void OnDisable() //OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;

        DiceManager.OnAllDiceStopped -= HandleAllDiceStopped;

    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Gameplay)
        {
            StartNewGameLoop();
        }
    }

    private void StartNewGameLoop()
    {
        CurrentPlayerIndex = 0;
        OnPlayerTurnChanged?.Invoke(CurrentPlayerIndex);
        ChangePhase(TurnPhase.TurnStart);
    }

    private void ChangePhase(TurnPhase newPhase)
    {
        CurrentPhase = newPhase;
        OnPhaseChanged?.Invoke(newPhase);

        switch(newPhase)
        {
            case TurnPhase.TurnStart:
                HandleTurnStart();
                break;
            case TurnPhase.CardDrifting:
                HandleCardDrifting();
                break;
            case TurnPhase.FirstRoll:
                HandleFirstRoll();
                break;
            case TurnPhase.RerollPhase:
                HandleRerollPhase();
                break;
            case TurnPhase.Resolution:
                HandleResolution();
                break;
            case TurnPhase.TugOfWarUpdate:
                HandleTugOfWarUpdate();
                break;
            case TurnPhase.TurnEnd:
                HandleTurnEnd();
                break;
        }
    }

    private void  HandleTurnStart()
    {
        Debug.Log($"Giliran Pemain {CurrentPlayerIndex} Dimulai");
        currentRerollCount = 0;

        ChangePhase(TurnPhase.CardDrifting);
    }

    private void HandleCardDrifting()
    {
        Debug.Log("Fase Drafting: Menunggu pemain memilih kartu dari Open Market...");

        ChangePhase(TurnPhase.FirstRoll);
    }

    private void HandleFirstRoll()
    {
        Debug.Log("Fase First Roll: Melempar semua dadu!");
    }

    private void HandleRerollPhase()
    {
        Debug.Log($"Fase Reroll ({currentRerollCount}/{MAX_REROLLS}): Menunggu pemain nge-lock dadu...");
    }

    private void ProcessedToResolution()
    {
        ChangePhase(TurnPhase.Resolution);
    }

    private void HandleResolution()
    {
        Debug.Log("Fase Resolution: Menghitung icon dadu...");
    }

    private void HandleTugOfWarUpdate()
    {
        Debug.Log("Fase Tug Of War: ");
    }

    private void HandleTurnEnd()
    {
        Debug.Log("Giliran Selesai. Mengecek Pemenang...");

        CurrentPlayerIndex = (CurrentPlayerIndex == 0) ? 1 : 0;
        OnPlayerTurnChanged?.Invoke(CurrentPlayerIndex);

        ChangePhase(TurnPhase.TurnStart);
    }

    private void HandleAllDiceStopped()
    {
        if (CurrentPhase == TurnPhase.FirstRoll)
        {
            Debug.Log("TurnManager: Mendapat laporan semua dadu sudah berhenti. Melanjutkan ke fase ReRoll!");
            ChangePhase(TurnPhase.RerollPhase);
        }
    }
}
