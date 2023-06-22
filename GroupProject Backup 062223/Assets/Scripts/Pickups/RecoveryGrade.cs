using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryGrade : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float gradeValue;
    public AudioClip pickup;

    [Header("Despawn")]
    public float beginDespawn = 5f;
    public float despawnDuration = 2f;
    public int numberOfBlinks = 5;

    void Start()
    {
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(beginDespawn);
        for (int i = 0; i < numberOfBlinks; i++)
        {
            sprite.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(despawnDuration / (numberOfBlinks * 2));
            sprite.color = Color.white;
            yield return new WaitForSeconds(despawnDuration / (numberOfBlinks * 2));
        }
        Destroy(gameObject);
    }

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
