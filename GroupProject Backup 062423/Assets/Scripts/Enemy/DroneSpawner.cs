using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    [Header("Spawner Values")]
    public float spawnCooldown;
    public Vector3 spawnOffset;
    public Vector3 bounds;
    private float cooldownTimer;
    
    [Header("References")]
    private GameObject player;
    public GameObject dronePrefab;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Awake()
    {
        //get reference for player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        //spawn drone when in range & off cooldown
        if (PlayerInSight() && cooldownTimer >= spawnCooldown)
        {
            SpawnDrone();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + spawnOffset, bounds);
    }

    private bool PlayerInSight()
    {
        //if player is in range of drone spawner
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + spawnOffset,  bounds, 0, Vector2.up, 0, playerLayer);

        return hit.collider != null;
    }

    private void SpawnDrone()
    {
        cooldownTimer = 0;
        Instantiate(dronePrefab, transform.position, Quaternion.identity);
    }
}
