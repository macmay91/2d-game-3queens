using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public TMP_Text soundText;
    public TMP_Text musicText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
        
    public void Resume()
        {
            pauseMenuUI.SetActive(false);
            //game time back to default
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
        
        
    public void Sound()
    {
        soundText.text = "Sound: ";
    }

        
    public void Music()
    {
        musicText.text = "Music: ";
    }


        
    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

        
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

        
    void Pause()
    {
            //turns on pause overlay
            pauseMenuUI.SetActive(true);
            //pause the game
            Time.timeScale = 0f;
            gameIsPaused = true;
    }
    
}
