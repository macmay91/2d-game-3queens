using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Newspaper : MonoBehaviour
{
    public float damage;
    public SpriteRenderer sprite;
    private GameObject player;

    [Header("Despawn")]
    public float beginDespawn = .5f;
    public float despawnDuration = 2f;
    public int numberOfBlinks = 5;

    // Start is called before the first frame update
    void Start()
    {
        //get reference for player
        player = GameObject.Find("Player");

        StartCoroutine(Despawn());
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //player loses grade
            player.GetComponent<Grade>().LoseGrade(damage);
            Destroy(gameObject);
        }
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
}
