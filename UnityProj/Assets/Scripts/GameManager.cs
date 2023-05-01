using System;
using System.Collections;
using System.Collections.Generic;
using BendyArms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputControllerRewired playerInput;  // for pause
    [SerializeField] private int startTime = 60;
    [SerializeField] private int timeBonusPerContainer = 5;
    [SerializeField] private UIGameOver uiGameOver;
    private int containersDelivered = 0;
    private int timer;

    private bool isPaused = false;

    public delegate void ScoreChanged(int containers);

    public event ScoreChanged scoreChanged;
    
    public delegate void TimerChanged(int timeLeft);

    public event TimerChanged timerChanged;

    private void OnEnable()
    {
        playerInput.pauseMenu.down += PauseGame;
    }

    private void OnDisable()
    {
        playerInput.pauseMenu.down -= PauseGame;
    }

    private void PauseGame()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            uiGameOver.SetState(UIGameOver.UiState.Paused);
            Time.timeScale = 0;
        }
        else
        {
            uiGameOver.SetState(UIGameOver.UiState.Disabled);
            Time.timeScale = 1;
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        uiGameOver.SetState(UIGameOver.UiState.Disabled);
        containersDelivered = 0;
        timer = startTime;
        StartCoroutine(UpdateTime());
    }

    public void IncrementContainerCount()
    {
        containersDelivered++;
        timer += timeBonusPerContainer;
        Debug.Log("Containers Delivered: "+containersDelivered.ToString());
        OnScoreChanged();
        OnTimeChanged();
    }

    IEnumerator UpdateTime()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            OnTimeChanged();
        } 
        TimerEnded();
    }
    void OnScoreChanged()
    {
        scoreChanged?.Invoke(containersDelivered);
    }

    void OnTimeChanged()
    {
        timerChanged?.Invoke(timer);
    }

    void TimerEnded()
    {
        //show game over screen or something 
        Time.timeScale = 0f;
        uiGameOver.SetState(UIGameOver.UiState.GameOver);
    }
}
