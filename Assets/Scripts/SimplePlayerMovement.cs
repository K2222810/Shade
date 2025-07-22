using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEditor.Animations;

public class SimplePlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true;  

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


    void Update()
    {
        
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
        if(context.performed)
        {
/*            Debug.Log(" Walking working");
*/        }
        if (context.canceled)
        {
/*            Debug.Log(" Walking stopped");
*/        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsReamaning > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsReamaning--; 
            }
            else if (context.canceled)
            {   
                //light tap of jump button = half the height
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsReamaning--;
            }
        }

        //wall jump
        if (context.performed && wallJumpTimer > 0f)
        { 
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); // jump away from wall
            wallJumpTimer = 0;

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
