using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float startTime = 60f;
    private int containersDelivered = 0;
    private float timer;

    public delegate void ScoreChanged(int containers);

    public event ScoreChanged scoreChanged;
    void Start()
    {
        containersDelivered = 0;
    }

    public void IncrementContainerCount()
    {
        containersDelivered++;
        Debug.Log("Containers Delivered: "+containersDelivered.ToString());
        OnScoreChanged();
    }

    void OnScoreChanged()
    {
        scoreChanged?.Invoke(containersDelivered);
    }
}
