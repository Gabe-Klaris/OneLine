using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private GameObject player;
    private string sceneName;
    public AudioSource StartGameSFX;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartGameSFX.Play();
        SceneManager.LoadScene("Level_One");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Options()
    {

    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                  UnityEditor.EditorApplication.isPlaying = false;
            #else
                  Application.Quit();
            #endif
    }

    public void CreditsBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
