using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBoy : MonoBehaviour
{

    [Header("Paper Boy Values")]
    public float damage;
    public float paperBoyRadius;
    public float throwCooldown;
    public float throwForce;
    private float cooldownTimer;
    
    public Vector3 hitOffset;
    public Vector3 bounds;

    [Header("References")]
    private GameObject player;
    private Vector3 initialScale;
    public LayerMask playerLayer;
    public bool faceLeft = true;
    public GameObject paper;
    public GameObject spawnPoint;

    void Awake()
    {
        initialScale = gameObject.transform.localScale;
        //get reference for player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= throwCooldown)
        {
            cooldownTimer = 0;
            Throw();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //player loses grade
            player.GetComponent<Grade>().LoseGrade(damage);
        }
    }

    private bool PlayerInSight()
    {
        //if player is within box (shown in gizmo)
        RaycastHit2D hit = Physics2D.BoxCast(spawnPoint.transform.position + hitOffset, bounds, 0, Vector2.up, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spawnPoint.transform.position + hitOffset, bounds);
    }

    private void Throw()
    {
        float x;
        //face right direction
        if (faceLeft)
        {
            gameObject.transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            x = 1;
            faceLeft = false;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(initialScale.x * -1, initialScale.y, initialScale.z);
            x = -1;
            faceLeft = true;
        }

        //instantiate newspaper left or right of paper boy
        var instance = Instantiate(paper, spawnPoint.transform.position + new Vector3(1 * x, 0, 0), Quaternion.identity);

        //send it left or right with physics
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(x, 0) * throwForce, ForceMode2D.Impulse);
    }

}
