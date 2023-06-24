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
    public float recoveryHealthForce;
    public AudioClip playerHit1;
    public AudioClip playerHit2;
    

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

            //spawn 3 recoveryHealth prefabs going left/middle/right
            SoundManager.instance.PlaySound(playerHit2);
            float x = Random.Range(-1, -.725f);
            DropGrade(x);
            float y = Random.Range(-.775f, .225f);
            DropGrade(y);
            float z = Random.Range(.275f, 1f);
            DropGrade(z);

            gradeText.text = "" + currentGrade;
        }
        else
        {
            //player goes down to 0 with no pickups
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(playerHit1);
            gradeText.text = "" + currentGrade;
        }
        
    }

    public void GainGrade(float _grade)
    {
        currentGrade = Mathf.Clamp(currentGrade + _grade, 0, maxGrade);
        gradeText.text = "" + currentGrade;
        Debug.Log(currentGrade);
    }

    private void DropGrade(float random)
    {
        SoundManager.instance.PlaySound(playerHit2);
        var instance = Instantiate(recoveryHealth, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

        //upward force in a random horizontal direction & randomized force
        float randomX = random;
        float randomF = recoveryHealthForce + Random.Range(-6, 6);
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(randomX, 1) * randomF, ForceMode2D.Impulse);
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
