using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEditor.Animations;

public class SimplePlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool changetoblack = false;
    private bool changetowhite = false;

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


    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        ChangeUniversecontext();
        
        // It chekcs if there is any ground in the floor,if not the player doesnt jump again(unles it has double jump).
        GroundCheck();
        //  Applies a more realistic gravity to the player.
        Gravity();
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
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsReamaning--;
            }
        }
    }

    // checks the ground,
    private void GroundCheck() 
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundChecksize, 0, GroundLayer))
        {
            jumpsReamaning = maxjumps;
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

        }
        if (context.performed && changetoblack)
        {
            // SHADE CHARACTER
            whiteshade.SetActive(false);
            blackshade.SetActive(true);
            //UNIVERSE
            BlackUniverse.SetActive(false);
            WhiteUniverse.SetActive(true);

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


    // A rentagle that represent the collison of the ground check
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(groundCheckPos.position, groundChecksize);
    }

}
