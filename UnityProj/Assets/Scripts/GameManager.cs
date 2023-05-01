using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startTime = 60;
    [SerializeField] private int timeBonusPerContainer = 5;
    private int containersDelivered = 0;
    private int timer;

    public delegate void ScoreChanged(int containers);

    public event ScoreChanged scoreChanged;
    
    public delegate void TimerChanged(int timeLeft);

    public event TimerChanged timerChanged;
    void Start()
    {
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
    }
}
