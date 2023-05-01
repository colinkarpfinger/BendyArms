using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string newGameScene;
    public Animator[] animators;
    public string[] animations;
    public GameObject musicManager;

    public void NewGame()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play(animations[i]);
        }
        StartCoroutine(LoadSceneAfterPause(.7f));
    }

    // public void QuitGame()
    // {
    //     Application.Quit();
    // }

    private IEnumerator LoadSceneAfterPause(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        musicManager.GetComponent<MusicManager>().StopMusic();
        SceneManager.LoadScene(newGameScene);
    }
}