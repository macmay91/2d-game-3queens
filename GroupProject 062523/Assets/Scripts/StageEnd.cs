using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEnd : MonoBehaviour
{

    private GameObject timer;
    private GameObject player;

    private void Awake()
    {
        timer = GameObject.Find("Timer");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Scene currentScene = SceneManager.GetActiveScene();  //load current scene name
        string sceneName = currentScene.name;

        if (collision.tag == "Player")
        {
            
            if(sceneName == "Level1")
            {
                //record level one variables and go to next stage
                timer.GetComponent<Timer>().RecordTime();
                GameManager.gm.levelOneGrade = GameManager.gm.currentGrade;
                GameManager.gm.LevelTwo();
            }
            else if(sceneName == "Level2")
            {
                //record level two variables and go to next stage
                GameManager.gm.levelTwoGrade = GameManager.gm.currentGrade;
                timer.GetComponent<Timer>().RecordTime();
                GameManager.gm.WinScene();
            }
            else
            {
                Debug.Log("Scene Change Error");
                //default to main menu
                SceneManager.LoadScene("MainMenu");  
            }


        }
    }
}
