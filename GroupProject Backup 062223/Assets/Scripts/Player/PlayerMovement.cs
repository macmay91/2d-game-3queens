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

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer;
    public GameObject characterHolder;
    public CapsuleCollider2D playerCollider;
    public AudioClip jump;

    [Header("Physics")]
    public float maxSpeed = 7f;
    private float initialMaxSpeed;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5f;
    public float crouchSpeed = .5f;
    public float crouchMaxSpeed = 7f;
    public float dragConstant = .9f;

    [Header("Collision")]
    public bool grounded;
    public float groundLength = 0.6f;
    public Vector3 groundColliderOffset;
    private bool crouched = false;
    private bool underPlatform = false;
    public Vector3 ceilingGizmoX;
    public Vector3 ceilingGizmoY;
    public Vector3 ceilingGizmoOffset;
    public Vector3 crouchSize;
    public Vector3 crouchOffset;
    public Vector3 ceilingOffset;
    

    private void Start()
    {
        //holds value of starting max speed
        initialMaxSpeed = maxSpeed;

    }
    void Update()
    {
        bool wasOnGround = grounded;
        
        //checks if either foot(raycast) is touching ground
        grounded = Physics2D.Raycast(transform.position + groundColliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - groundColliderOffset, Vector2.down, groundLength, groundLayer);
        
        underPlatform = CeilingCheck();
        
        //if player is crouching or stuck under ceiling
        if (Input.GetButton("Crouch") && grounded)
        {
            crouched = true;
        }
        else if (!CeilingCheck())
        {
            crouched = false;
        }


        //if player wasn't on ground until this frame
        if (!wasOnGround && grounded)
        {
            CreateDust();
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }
        
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //buffer frames for jumping
        if(Input.GetButtonDown("Jump") && (Input.GetKey(KeyCode.S) == false))
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
        if(jumpTimer > Time.time && grounded  && !crouched)
        {
            Jump();
        }
        //top collider collision turned off if crouched
        //GetComponent<BoxCollider2D>().isTrigger = (crouched || underPlatform) ? true : false;
        //resizes collider if crouched
        playerCollider.size = (crouched || underPlatform) ? new Vector3(0.3600313f, crouchSize.y, 1) : new Vector3(0.3600313f, 1.813367f, 1);
        //collider offset while crouched
        playerCollider.offset = (crouched || underPlatform) ? (new Vector3(0, crouchOffset.y, 0)) : new Vector3(0, 0, 0);
        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
        //adds force left or right
        if(crouched)
        {
            maxSpeed = crouchMaxSpeed;
            rb.AddForce(Vector2.right * horizontal * (moveSpeed * crouchSpeed));
        }
        else
        {
            maxSpeed = initialMaxSpeed;
            rb.AddForce(Vector2.right * horizontal * moveSpeed);
        }
        

        //starts running animation if absolute value of x > .5
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
        
        //limits velocity to value of maxSpeed
        if(Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        
    }

    //flip player sprite around
    void Flip()
    {
        //create dust if grounded
        if(grounded == true)
        {
            CreateDust();
        }
        facingRight = !facingRight;
        //flip player scale (similar to rotate 180)
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
                ApplyHorizontalDrag();
            }
            
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
        //gizmos to check grounded under feet
        Gizmos.DrawLine(transform.position + groundColliderOffset, transform.position + groundColliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - groundColliderOffset, transform.position - groundColliderOffset + Vector3.down * groundLength);

        //Gizmo for ceiling check parameters
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(playerCollider.bounds.center + new Vector3(0, ceilingOffset.y, 0), new Vector3(0.3600313f, .3f, 1));
    }

    //player jump
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        CreateDust();
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
        SoundManager.instance.PlaySound(jump);
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

    //applies drag to horizontal movement
    void ApplyHorizontalDrag()
    {
        rb.velocity = new Vector2(rb.velocity.x * Mathf.Clamp01(1 - dragConstant), rb.velocity.y);
    }

    //checks if there is a ceiling above player
    private bool CeilingCheck()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center + new Vector3(0, ceilingOffset.y, 0), new Vector2(0.3600313f, .3f), 0f, Vector2.up, 0f, groundLayer);
    }

    //create dust particle effect
    void CreateDust()
    {
        dust.Play();
    }

}
