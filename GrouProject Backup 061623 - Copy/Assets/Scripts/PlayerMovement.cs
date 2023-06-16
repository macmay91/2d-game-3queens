using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public ParticleSystem dust;
    
    [Header("Horizontal Movement")]
    public float moveSpeed = 10;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 8f;
    public float jumpDelay = 0.25f;
    private float jumpTimer;
    public GameObject characterHolder;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool grounded;
    public float groundLength = 0.6f;
    public Vector3 colliderOffset;

    void Update()
    {
        bool wasOnGround = grounded;
        
        //checks if either foot(raycast) is touching ground
        grounded = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        //if player wasn't on ground until this frame
        if(!wasOnGround && grounded)
        {
            CreateDust();
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }
        
        //player movement
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //buffer frames for jumping
        if(Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }

        //Set animation parameters
        animator.SetFloat("horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetBool("grounded", grounded);
    }

    void FixedUpdate()
    {
        moveCharacter(direction.x);

        //jump if on ground and within buffer window
        if(jumpTimer > Time.time && grounded)
        {
            Jump();
        }

        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical", Mathf.Abs(rb.velocity.y));

        //flips player when moving left/right
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        
    }
    void Flip()
    {
        if(grounded == true)
        {
            CreateDust();
        }
        facingRight = !facingRight;
        transform.localScale = facingRight ? Vector3.one : new Vector3(-1, 1, 1);
    }

    void modifyPhysics()
    {
        //checks if player is changing horizontal direction
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (grounded)
        {
            //if player stops moving or changes direction increase drag
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
           //if player is in air
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if(rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            //if player is moving up but releases jump
            else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    void OnDrawGizmos()
    {
        //displays raycasts in unity
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
 
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        CreateDust();
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    
    //squash and stretch player animation while jumping or landing
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }

    void CreateDust()
    {
        dust.Play();
    }

}
