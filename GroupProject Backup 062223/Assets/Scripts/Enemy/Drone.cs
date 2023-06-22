using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Dog Values")]
    public float damage;
    public float droneSpeed;
    public float lifeSpan;
    public float slowdownDistance;
    private Vector3 initialScale;
    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        initialScale = gameObject.transform.localScale;
        
        //get reference for player
        player = GameObject.Find("Player");

        Invoke("SelfDestruct", lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void Movement()
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

        float xDifference = Mathf.Abs(gameObject.transform.position.x - player.transform.position.x);
        float yDifference = Mathf.Abs(gameObject.transform.position.y - player.transform.position.y);
        
        //if drone is close to player move at half speed
        if(xDifference <= slowdownDistance && yDifference <= slowdownDistance)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z), (droneSpeed * .75f) * Time.deltaTime);
        }
        else
        {
        //move toward player at normal speed of drone
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z), droneSpeed * Time.deltaTime);
        }

    }
}
