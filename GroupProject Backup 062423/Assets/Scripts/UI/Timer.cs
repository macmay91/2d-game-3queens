using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeValue = 90;
    public TMP_Text timerText;
    private float minutes;
    private float seconds;


    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            //never display negative time value
            timeValue = 0;

            //run gameover method
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if(timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        //how many minutes remain
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        //remainder of seconds remain
        seconds = Mathf.FloorToInt(timeToDisplay % 60);

        //display text with proper formatting
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RecordTime()
    {
        //gameManager

        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene);

        if (scene.name == "Level1")
        {
          GameManager.gm.levelOneMin = minutes;
          GameManager.gm.levelOneSec = seconds;
          Debug.Log(GameManager.gm.levelOneMin + "min");
          Debug.Log(GameManager.gm.levelOneSec + "sec");
        }
        else if (scene.name == "Level2")
        {
                GameManager.gm.levelTwoMin = minutes;
                GameManager.gm.levelTwoSec = seconds;
                Debug.Log(GameManager.gm.levelTwoMin + "min");
                Debug.Log(GameManager.gm.levelTwoSec + "sec");
        }
        else
        {
          Debug.Log("Record Time Error");
        }

    }
}
