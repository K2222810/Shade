using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

/*    public LayerMask groundLayer;
    private Vector2 moveInput;*/
    [Header("INPUT AND MORE")]
    public CharacterController characterController;
    //PlAYER INPUT
    PlayerInput playerInput;
    //ANIMATION
    Animator animator;

    //-------------------------------------/////
    //ONPRESSED
    private bool isjumpPressed;
    private bool iswalkingPressed;


    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        // PLAYER WALK INPUT SYSTEM 
        playerInput.PlayerController.Walk.started += OnWalk;
        playerInput.PlayerController.Walk.canceled += OnWalk;
        playerInput.PlayerController.Walk.performed += OnWalk;
        // PLAYER JUMP INPUT SYSTME
        playerInput.PlayerController.Jump.started += OnJump;
        playerInput.PlayerController.Jump.canceled += OnJump;
    }
    private void OnEnable()
    {
        playerInput.PlayerController.Enable();
    }
    void OnDisable()
    {
        playerInput.PlayerController.Disable();
    }

    private void Update()
    {
        // Check ground status
/*        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);*/

/*        // Handle jumping
        if (jumpPressed && isGrounded)
        {
*//*            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
*//*            jumpPressed = false;
        }*/
    }
    void OnWalk(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (input.y > 0)
        Debug.Log("W pressed (up)");
        if (input.y < 0)
        Debug.Log("s pressed (back");
        if (input.x < 0)
        Debug.Log("a pressed (left)");
        if (input.x > 0)
        Debug.Log("d pressed (right");
        
    }
    void OnJump(InputAction.CallbackContext contex)
    {
        Debug.Log("Pressing jump");
    }
/*    private void FixedUpdate()
    {
        // Move the player horizontally
*//*        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
*//*    }
*/

/*    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Pressing jump");
        if (context.started)
        {
            jumpPressed = true;
        }
        if (context.canceled)
        {
            jumpPressed = false;
        }
    }*/
}
