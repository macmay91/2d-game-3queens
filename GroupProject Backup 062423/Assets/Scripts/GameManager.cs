using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public float levelOneMin;
    public float levelTwoMin;
    public float levelOneSec;
    public float levelTwoSec;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level2");
    }

    public void WinScene()
    {
        SceneManager.LoadScene("Win");
    }
}
