using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressRestart()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    public void ButtonPressQuit()
    {
        Application.Quit();
    }
}
