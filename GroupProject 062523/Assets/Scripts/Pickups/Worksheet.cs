using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worksheet : MonoBehaviour
{
    public float gradeValue;
    public AudioClip pickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //player gains grade
            collision.GetComponent<Grade>().GainGrade(gradeValue);
            SoundManager.instance.PlaySound(pickup);
            Destroy(gameObject);
        }
    }
}
