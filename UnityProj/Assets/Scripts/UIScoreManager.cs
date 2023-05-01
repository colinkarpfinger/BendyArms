using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;   //todo might want to move this to a scriptable object or something 
    [SerializeField] private TextMeshProUGUI textContainerScore;   //todo might want to move this to a scriptable object or something 

    private void OnEnable()
    {
        gameManager.scoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        gameManager.scoreChanged -= UpdateScore;
    }

    private void UpdateScore(int containers)
    {
        textContainerScore.SetText(containers.ToString());
    }

}
