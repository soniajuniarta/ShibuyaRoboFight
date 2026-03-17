using System;
using System.Collections;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Tarik Panel Main Battle dari Hierarchy ke sini!")]
    public GameObject panelMainBattle;

    [Tooltip("Tarik Panel Dice Screen dari Hierarchy ke sini!")]
    public GameObject panelDiceScreen;

    [Tooltip("Tarik btnTapToRoll dari Hierarchy ke sini!")]
    public GameObject btnTapToRoll;

    [Tooltip("Masukin object 'Action Buttons' dari Hierarchy ke sini!")]
    public GameObject actionButtons;

    [Header("Cinemachine Cameras")]
    public GameObject VCamArena;
    public GameObject VCamPlayer;
    public GameObject VCamEnemy;

    private void Awake()
    {
        ShowMainBattleScreen();
    }

    private void OnEnable()
    {
        TurnManager.OnPhaseChanged += HandlePhaseChange;
    }

    private void OnDisable()
    {
        TurnManager.OnPhaseChanged -= HandlePhaseChange;    
    }

    private void HandlePhaseChange(TurnManager.TurnPhase phase)
    {
        bool isPlayerTurn = (TurnManager.Instance.CurrentPlayerIndex == 0);

        switch(phase)
        {
            case TurnManager.TurnPhase.FirstRoll:
                ShowDiceScreen(isPlayerTurn);
                break;

            case TurnManager.TurnPhase.RerollPhase:
                if (panelDiceScreen != null) panelDiceScreen.SetActive(true);
                Debug.Log("<color=cyan>UIManager: Reroll Phase - Tampilkan UI Reroll!</color>");

                if (actionButtons != null) actionButtons.SetActive(isPlayerTurn);
                break;

            default:
                ShowMainBattleScreen();
                break;
        }
    }

    private void ShowMainBattleScreen()
    {
        if (panelMainBattle != null) panelMainBattle.SetActive(true);
        if (panelDiceScreen != null) panelDiceScreen.SetActive(false);

        if (VCamArena != null) VCamArena.SetActive(true);
        if (VCamPlayer != null) VCamPlayer.SetActive(false);
        if (VCamEnemy != null) VCamEnemy.SetActive(false);

        Debug.Log("<color=cyan>UIManager: Pindah ke Layar Main Battle!</color>");
    }

    private void ShowDiceScreen(bool isPlayerTurn)
    {
        if (panelMainBattle != null) panelMainBattle.SetActive(false);
        if (panelDiceScreen != null) panelDiceScreen.SetActive(false);
        if (btnTapToRoll != null) btnTapToRoll.SetActive(isPlayerTurn);
        if (actionButtons != null) actionButtons.SetActive(isPlayerTurn);

        if (VCamArena != null) VCamArena.SetActive(false);

        if (isPlayerTurn)
        {
            if (VCamPlayer != null) VCamPlayer.SetActive(true);
            if (VCamEnemy != null) VCamEnemy.SetActive(false);
        }
        else
        {
            if (VCamPlayer != null) VCamPlayer.SetActive(false);
            if (VCamEnemy != null) VCamEnemy.SetActive(true);
        }

        Debug.Log("<color=cyan>UIManager: Pindah ke Layar Lempar Dadu!</color>");

        StartCoroutine(DelayDiceUI(2f));
    }

    private IEnumerator DelayDiceUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (panelDiceScreen != null) panelDiceScreen.SetActive(true);
    }
}
