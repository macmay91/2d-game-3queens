using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grade : MonoBehaviour
{
    [Header("Grade Values")]
    public float startingGrade;
    public float maxGrade;
    private float currentGrade;

    [Header("References")]
    public TMP_Text gradeText;
    public SpriteRenderer player;
    public GameObject recoveryHealth;

    [Header("iFrames")]
    public float iFramesDuration;
    public int numberOfBlinks;
    


    void Awake()
    {
        currentGrade = startingGrade;
        Debug.Log(currentGrade);
        gradeText.text = "" + currentGrade;
    }

    public void LoseGrade(float _grade)
    {
        if (currentGrade == 0)
        {
            //game over
            Debug.Log("Game Over");
        }

        currentGrade = Mathf.Clamp(currentGrade - _grade, 0, maxGrade);

        if(currentGrade  > 0)
        {
            StartCoroutine(Invulnerability());
            //spawn 3 recoveryHealth prefabs
            for (int i = 0; i < 3; i++)
            {
                Instantiate(recoveryHealth, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
            gradeText.text = "" + currentGrade;
        }
        else
        {
            //player goes down to 0 with no pickups
            StartCoroutine(Invulnerability());
            gradeText.text = "" + currentGrade;
        }
        
    }

    public void GainGrade(float _grade)
    {
        currentGrade = Mathf.Clamp(currentGrade + _grade, 0, maxGrade);
        gradeText.text = "" + currentGrade;
        Debug.Log(currentGrade);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(7, 9, true);
        for (int i = 0; i < numberOfBlinks; i++)
        {
            //transparent red
            player.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfBlinks * 2));
            //return to normal
            player.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfBlinks * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 9, false);
    }

}
