using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [Header("Dog Values")]
    public float damage;
    public float dogSpeed;
    private bool barkPlayed = false;
    public AudioClip bark;

    [Header("References")]
    private GameObject player;
    public LayerMask playerLayer;
    public Vector3 hitOffset;
    public Vector3 bounds;
    public Transform territory;
    private Vector3 initialScale;
    private bool stopMoving;

    private void Awake()
    {
        initialScale = gameObject.transform.localScale;

        //get reference for player
        player = GameObject.Find("Player");
    }
    private void Update()
    {

        if (PlayerInSight())
        {
            if(!barkPlayed)
            {
                //play sound once
                SoundManager.instance.PlaySound(bark);
                barkPlayed = true;
            }
            
            Chase();
        }

        if (!PlayerInSight())
        {
            barkPlayed = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //player loses grade
            player.GetComponent<Grade>().LoseGrade(damage);
            StartCoroutine(Wait());
        }
    }

    private bool PlayerInSight()
    {
        //if player is standing in front of old man
        RaycastHit2D hit = Physics2D.BoxCast(territory.position + hitOffset, bounds, 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(territory.position + hitOffset, bounds);
    }

    private IEnumerator Wait()
    {
        //stop dog from moving for 3 seconds
        stopMoving = true;
        yield return new WaitForSeconds(3);
        stopMoving = false;

    }

    private void Chase()
    {
         //face right direction
        if (player.transform.position.x <= gameObject.transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(initialScale.x * -1, initialScale.y, initialScale.z);
        }
        
        if (!stopMoving)
        {
            //move toward player at speed of dog
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(player.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), dogSpeed * Time.deltaTime);
        }

    }
}
