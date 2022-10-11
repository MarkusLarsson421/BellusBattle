using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] [Range(1f, 25f)] private float moveSpeed;
    [SerializeField] [Range(1f, 25f)] private float jumpForce;
    [SerializeField] [Range(25f, 150f)] private float airDeceleration;
    [SerializeField] [Range(25f, 150f)] private float groundDeceleration;
    [SerializeField] [Range(10f, 300f)] private float airResistance;
    [SerializeField] [Range(-100f, 0f)] private float downwardForce;

    private float deceleration;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Vector2 velocity;
    private float movementX, movementY;

    private float coyoteTime = 0.2f;
    private float coyoteTimer;
    
    private bool hasCoyoteTime;
    private bool hasDoubleJump;

    private float skinWidth = 0.012f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementForce();
        CheckIsGrounded();
        Jump();
        UpdateCoyoteTime();
        if (!CheckIsGrounded())
        {
            movementY = Mathf.MoveTowards(movementY, downwardForce, airResistance * Time.deltaTime);
        }

        if(CheckIsGrounded() && velocity.y < 0)
        {
            movementY = 0;
            coyoteTimer = 0;
            hasCoyoteTime = true;
            hasDoubleJump = true;
        }
        velocity = new Vector2(movementX, movementY);
        if (!CheckCollision())
        {
            transform.Translate(velocity * Time.deltaTime);
        }
        
    }

    private void UpdateMovementForce() 
    {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            movementX = moveSpeed * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            movementX = Mathf.MoveTowards(movementX, 0, deceleration * Time.deltaTime);
        }
        
    
    }
    private bool CheckIsGrounded() 
    {
        if(Physics.Raycast(transform.position, Vector2.down, 0.5f, groundLayer))
        {
            deceleration = groundDeceleration;
            return true;
        }
        deceleration = airDeceleration;
        return false;
    
    }
    private void Jump() 
    {
        if (CheckIsGrounded() || hasCoyoteTime || hasDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(!hasCoyoteTime && hasDoubleJump) { hasDoubleJump = false; }
                movementY = jumpForce;
            }
        }
    
    }

    private void UpdateCoyoteTime()
    {
        if (CheckIsGrounded() || !hasCoyoteTime) return;
        Debug.Log("awooo");
        if(coyoteTimer > coyoteTime)
        {
            hasCoyoteTime = false;
        }
        coyoteTimer += Time.deltaTime;
    }

    private bool CheckCollision()
    {
        return Physics.Raycast(transform.position, velocity, 0.5f + skinWidth, wallLayer);
       
    }
}
