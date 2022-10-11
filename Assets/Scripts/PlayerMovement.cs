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

    private BoxCollider boxCollider;
    [SerializeField]private Vector3 rayCastBottomLeft, rayCastBottomRight, rayCastTopRight, rayCastTopLeft;

    private float verticalRaySpacing, horizontalRaySpacing;

    private int horizontalRayCount, verticalRayCount = 4;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
    }

    // Update is called once per frame
    void Update()
    {
        updateRayCastOrgins();
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
        
        /*
        if(velocity.y != 0)
        {
            HandleVerticalCollisions(ref velocity);
        }
        */
        
        
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
        Debug.DrawRay(transform.position, velocity, Color.green,0.5f + skinWidth);
        return Physics.Raycast(transform.position, velocity, 0.5f + skinWidth, wallLayer);
       
    }

    //Funkar inte än!
    private void HandleVerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.Log("adada");

            Vector2 rayOrigin = (directionY < 0) ? rayCastBottomLeft : rayCastTopLeft;
            //rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            
            

            Debug.DrawRay(rayOrigin, Vector2.right * directionY * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.right * directionY, out hit, rayLength, wallLayer))
            {
                Debug.Log("Hit!");
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                Debug.Log(hit.distance);
                
                //movementY = 0;
            }
        }
    }

    
    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    private void updateRayCastOrgins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        rayCastBottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        rayCastTopLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        rayCastBottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        rayCastTopRight = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
    }


}
