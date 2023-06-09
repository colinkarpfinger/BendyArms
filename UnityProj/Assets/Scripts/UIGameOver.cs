using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button buttonResume;
  
    public enum UiState
    {
        GameOver,
        Paused,
        Disabled
    }

    public void SetState(UiState state)
    {
        if (state == UiState.Disabled)
        {
            this.gameObject.SetActive(false);
        }
        else if (state == UiState.Paused)
        {
            this.gameObject.SetActive(true);
            text.SetText("Paused");
            buttonResume.gameObject.SetActive(true);
        }
        else if (state == UiState.GameOver)
        {   
            this.gameObject.SetActive(true);
            text.SetText("Game Over");
            buttonResume.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator RestartLevel() {
        
        FMOD.Studio.Bus masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        yield return new WaitForSecondsRealtime(2);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    public void ButtonPressRestart()
    {
        StartCoroutine(RestartLevel());
    }

    public void ButtonPressQuit()
    {
        Application.Quit();
    }

    public void ButtonPressedResume()
    {
        SetState(UiState.Disabled);
        Time.timeScale = 1;
    }
}
