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

    [Header("SHADE CHARACTER")]
    public GameObject whiteshade;
    public GameObject blackshade;

    [Header("Black And White Universe Background")]
    public GameObject BlackUniverse;
    public GameObject WhiteUniverse;


    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        ChangeUniversecontext();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
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
            Debug.Log("Can change to white");
            changetoblack = false;
            changetowhite = true;
        }
        if (whiteshade.activeSelf && !blackshade.activeSelf)
        {
            // CHANGE TO BLACK
            Debug.Log("Can change to black");
            changetowhite = false;
            changetoblack = true;

        }
    }

}
