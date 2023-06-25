using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text gradeText;

    private void Awake()
    {
        //calculate time elapsed stage 1
        float LevelOne = (GameManager.gm.levelOneMin * 60 + GameManager.gm.levelOneSec);
        //how many minutes remain
        float minutes1 = Mathf.FloorToInt(LevelOne / 60);
        //remainder of seconds
        float seconds1 = Mathf.FloorToInt(LevelOne % 60);


        //calculate time elapsed stage 2
        float LevelTwo = (GameManager.gm.levelTwoMin * 60 + GameManager.gm.levelTwoSec);
        //how many minutes remain
        float minutes2 = Mathf.FloorToInt(LevelTwo / 60);
        //remainder of seconds
        float seconds2 = Mathf.FloorToInt(LevelTwo % 60);

        float totalMinutes = Mathf.FloorToInt((minutes1 + minutes2) + (seconds1 + seconds2)/ 60);
        Debug.Log(totalMinutes + "total minutes");
        float totalSeconds = Mathf.FloorToInt((seconds1+seconds2) % 60);
        Debug.Log(totalSeconds + "total seconds");

        //display time text with proper formatting
        timeText.text = string.Format("{0:00}:{1:00}", minutes1, seconds1) + "<br>" + string.Format("{0:00}:{1:00}", minutes2, seconds2) + "<br>" + string.Format("{0:00}:{1:00}", totalMinutes, totalSeconds);

        float gradeAverage = (GameManager.gm.levelOneGrade + GameManager.gm.levelTwoGrade) / 2;

        //displays grade values
        gradeText.text = GameManager.gm.levelOneGrade + "<br>" + GameManager.gm.levelTwoGrade + "<br>" + gradeAverage;

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

}
