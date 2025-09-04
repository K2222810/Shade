using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Enemy : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed;
    public float jumpForce;
    public int  damage = 1; 

    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isJumping; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down , 1f,groundLayer);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        if (isGrounded)
        {
            //Chase player
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

            //If ground
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            //If gap
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
            //If plataform above
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 2f, groundLayer);

            if (!groundInFront.collider && !gapAhead.collider)
            {
                isJumping = true;
            }
            else if (!isPlayerAbove && !gapAhead.collider)
            {
                isJumping = true;
            }

        }

    }

    private void FixedUpdate()
    {
        if (isGrounded && isJumping)
        {
            isJumping = false;
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;

            rb.AddForce(new Vector2(jumpDirection.x, jumpForce),ForceMode2D.Impulse);

        
        }
    }

}
