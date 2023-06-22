using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform leftEdge;
    public Transform rightEdge;

    [Header("Enemy")]
    public Transform enemy;
    public float speed =1f;
    private Vector3 initialScale;
    private bool movingLeft;

    [Header("Enemy Idle")]
    public float idleDuration;
    private float idleTimer;

    private void Awake()
    {
        initialScale = enemy.localScale;
    }


    void Update()
    {
        
        //move left until reach leftEdge, turn around, move right until rightEdge
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            { 
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if ((enemy.position.x <= rightEdge.position.x))
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }

    }

    private void DirectionChange()
    {
        //stand idle at either edge
        idleTimer += Time.deltaTime;
        
        //face other direction
        if(idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }

    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        
        //enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initialScale.x) * _direction * -1, initialScale.y, initialScale.z);

        //enemy move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}
