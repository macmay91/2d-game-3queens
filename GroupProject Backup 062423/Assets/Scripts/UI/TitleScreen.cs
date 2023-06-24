using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

}
