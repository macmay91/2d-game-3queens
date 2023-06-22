using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldmanEnemy : MonoBehaviour
{
    [Header("Attack Values")]
    public float damage;
    public float attackCooldown;
    private float cooldownTimer;

    [Header("References")]
    private GameObject player;
    public BoxCollider2D oldmanCollider;
    public LayerMask playerLayer;
    public Vector3 hitOffset;
    public Animator oldmanAnim;

    private void Awake()
    {
        //get reference for player
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //attack when player in sight and cooldown up
        if(PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            oldmanAnim.SetTrigger("Attack");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //player loses grade
            player.GetComponent<Grade>().LoseGrade(damage);
        }
    }

    private bool PlayerInSight()
    {
        //if player is standing in front of old man
        RaycastHit2D hit = Physics2D.BoxCast(oldmanCollider.bounds.center + hitOffset * transform.localScale.x, oldmanCollider.bounds.size, 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(oldmanCollider.bounds.center + hitOffset * transform.localScale.x, oldmanCollider.bounds.size);
    }
    
    //method triggers on specific frame event to check if player is still in range
    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            //player loses grade
            player.GetComponent<Grade>().LoseGrade(damage);

        }
    }
}
