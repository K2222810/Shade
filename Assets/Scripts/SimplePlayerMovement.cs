using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEditor.Animations;
using UnityEngine.InputSystem.Utilities;
using UnityEditor.Rendering.LookDev;

public class SimplePlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;
    BoxCollider2D playerCollider; 

    //Shade Colors
    private bool changetoblack = false;
    private bool changetowhite = false;
    public bool shadeiswhite = false;
    public bool shadeisblack = false;
    public bool worldisblack = false;
    public bool worldiswhite = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    bool isOnPlatform; 
    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trainlRender; 

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxjumps = 2;
    int jumpsReamaning;
    
    [Header("SHADE CHARACTER")]
    public GameObject whiteshade;
    public GameObject blackshade;

    [Header("Black And White Universe Background")]
    public GameObject BlackUniverse;
    public GameObject WhiteUniverse;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundChecksize = new Vector2(0.5f, 0.05f);
    public LayerMask GroundLayer;
    bool isGrounded;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallChecksize = new Vector2(0.49f, 0.03f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //Animation
    public Animator animator; 

    //wall jumping

    bool isWallJumping;
    float wallJumpDirection;
    float wallJumptime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    private void Start()
    {
        trainlRender = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();  
    }
    void Update()
    {

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("Magnitud", rb.velocity.magnitude);
        animator.SetBool("IsWallSliding", isWallSliding);

        if (isDashing)
        {
            return;
        }
        ChangeUniversecontext();
        // It chekcs if there is any ground in the floor,if not the player doesnt jump again(unles it has double jump).
        GroundCheck();
        //  Applies a more realistic gravity to the player.
        Gravity();
        // The character flips if facing the right direction the player moves, the void function 
        flip();

        processWallSlide();

        processWallJump();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            flip();
        }

    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else 
        {
            rb.gravityScale = baseGravity;
            
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
     
    }
    public void Drop(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && isOnPlatform && playerCollider.enabled)
        {
            StartCoroutine(DisablePlayerCollider(0.25f));
        }
    }
    private IEnumerator DisablePlayerCollider(float disableTime)
    { 
        playerCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);       
        playerCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("whiteobstacles"))
        { 
            isOnPlatform = true;
        }
        if (collision.gameObject.CompareTag("blackobstacles"))
        {
            isOnPlatform = true;
        }

    }


    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine()); 
        }
    }

    private IEnumerator DashCoroutine()
    { 
        Physics2D.IgnoreLayerCollision(7,8,true);
        canDash = false;    
        isDashing = true;
        trainlRender.emitting = true;
        
        float dashDirection = isFacingRight ? 1f : -1f; 
            
        rb.velocity = new Vector2(dashDirection * dashSpeed,rb.velocity.y); //dash movement

        yield return new WaitForSeconds(dashDirection);

        rb.velocity = new Vector2(0f, rb.velocity.y); //Reset horinzontal Velocity
        
        isDashing = false;
        trainlRender.emitting = false;
        Physics2D.IgnoreLayerCollision(7, 8, false);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsReamaning > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsReamaning--;
                JumpFX();

            }
            else if (context.canceled)
            {   
                //light tap of jump button = half the height
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsReamaning--;
                JumpFX();
            }
        }

        //wall jump
        if (context.performed && wallJumpTimer > 0f)
        { 
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); // jump away from wall
            wallJumpTimer = 0;
            JumpFX();

            //force flip
            if (transform.localScale.x != wallJumpDirection) 
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
            Invoke(nameof(CancelWallJump), wallJumptime + 0.1f);
        
        }
    }

    private void JumpFX()
    {
        animator.SetTrigger("jump");
        smokeFX.Play();
    }    


    // checks the ground,
    private void GroundCheck() 
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundChecksize, 0, GroundLayer))
        {
            jumpsReamaning = maxjumps;
            isGrounded = true;
        }
        else 
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallChecksize,0, wallLayer); 
    }

    private void processWallSlide()
    {   //not grounded & On a wall & movement != 0
        if(!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else 
        {
            isWallSliding = false; 
        } 
    }

    private void processWallJump()
    {
        if (isWallSliding)
        {
            isWallSliding = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumptime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        { 
            wallJumpTimer -= Time.deltaTime;    
        }
    }

    private void CancelWallJump()
    { 
        isWallJumping = false;
    } 

    private void flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if(rb.velocity.y == 0)
            {
                smokeFX.Play();
            }

        }
    }

    public void ChangeUniverse(InputAction.CallbackContext context)
    {
        if (context.performed && changetowhite)
        {
            //SHADE CHARACTER
            blackshade.SetActive(false);
            whiteshade.SetActive(true);
            //UNIVERSE
            WhiteUniverse.SetActive(false);
            BlackUniverse.SetActive(true);
            //BOOL FOR THE OTHER SCRIPT
            shadeiswhite = true;
            worldisblack = true;
            shadeisblack = false;
            worldiswhite = false;

        }
        if (context.performed && changetoblack)
        {
            // SHADE CHARACTER
            whiteshade.SetActive(false);
            blackshade.SetActive(true);
            //UNIVERSE
            BlackUniverse.SetActive(false);
            WhiteUniverse.SetActive(true);
            //BOOL FOR THE OTHER SCRIPT
            shadeiswhite = false;
            worldisblack = false;
            shadeisblack = true;
            worldiswhite = true;

        }
    }
    public void ChangeUniversecontext()
    {
        if (blackshade.activeSelf && !whiteshade.activeSelf)
        {
            //CHANGE TO WHITE
            changetoblack = false;
            changetowhite = true;
        }
        if (whiteshade.activeSelf && !blackshade.activeSelf)
        {
            // CHANGE TO BLACK
            changetowhite = false;
            changetoblack = true;

        }
    }
}
