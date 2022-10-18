using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] [Tooltip("Variabel som påverkar spelarens hastighet")] [Range(1f, 25f)] private float moveSpeed;
    [SerializeField] [Tooltip("Hur högt spelaren kan hoppa")] [Range(1f, 25f)] private float jumpForce;
    [SerializeField] [Tooltip("Hur snabbt spelaren decelererar i luften")] [Range(25f, 150f)] private float airDeceleration;
    [SerializeField] [Tooltip("Hursnabbt spelaren decelererar på marken")] [Range(25f, 150f)] private float groundDeceleration;
    [SerializeField] [Tooltip("Påverkar hur snabbt spelaren förlorar energi i ett hopp")] [Range(10f, 300f)] private float airResistance;
    [SerializeField] [Tooltip("Hur mycket gravitation som påverkar spelaren i luften")] [Range(-100f, 0f)] private float downwardForce;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask oneWayLayer;
    [SerializeField] private LayerMask downWayLayer;

    public LayerMask WallLayer
    {
        get { return wallLayer; }
    }

    private bool isGrounded
    {
        get { return CheckIsGrounded(); }
    }

    public Vector2 velocity;
    private Vector2 rayCastBottomLeft, rayCastBottomRight, rayCastTopRight, rayCastTopLeft;

    private int horizontalRayCount, verticalRayCount = 4;

    private float movementX, movementY;
    private float deceleration;
    private float coyoteTime = 0.2f;
    private float coyoteTimer;
    private float skinWidth = 0.012f;
    private float downwardInput;
    private float verticalRaySpacing, horizontalRaySpacing;

    private bool hasCoyoteTime;
    private bool hasDoubleJump;
    private bool movingLeft, movingRight;
    private bool isStandingOnOneWayPlatform;

    private BoxCollider boxCollider;

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
        UpdateCoyoteTime();
        if (!CheckIsGrounded())
        {
            movementY = Mathf.MoveTowards(movementY, downwardForce, airResistance * Time.deltaTime);
        }

        if (isGrounded && velocity.y < 0)
        {
            movementY = 0;
            coyoteTimer = 0;
            hasCoyoteTime = true;
            hasDoubleJump = true;
        }
      
        velocity = new Vector2(movementX, movementY);

        HandleVerticalCollisions(ref velocity);
        HandleHorizontalCollisions(ref velocity);


        if (!CheckCollision())
        {
            transform.Translate(velocity * Time.deltaTime);
        }

        

        // So that when we change scene we don't have to re-join the lobby
        DontDestroyOnLoad(this.gameObject);
    }

    private void FLipPlayer()
    {
        if (velocity.x < -.01)
        {
            transform.eulerAngles = new Vector3(0, 180, 0); // Flipped
        }
        else if (velocity.x > .01)
        {
            transform.eulerAngles = new Vector3(0, 0, 0); // Normal
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        downwardInput = ctx.ReadValue<Vector2>().y;
        Debug.Log(ctx.ReadValue<Vector2>().y);
       // movementX2 = ctx.ReadValue<Vector2>().x;
      
        if (ctx.ReadValue<Vector2>().x > 0.1f)
        {
            
            movingRight = true;
            movingLeft = false;
        }
        else if (ctx.ReadValue<Vector2>().x < -0.1f)
        {
            
            movingRight = false;
            movingLeft = true;
        }
        else if (ctx.ReadValue<Vector2>().x < 0.2f || ctx.ReadValue<Vector2>().x > -0.2f)
        {
            movingRight = false;
            movingLeft = false;
        }
     

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;

        if (downwardInput <= -0.98f && isStandingOnOneWayPlatform)
        {
            Debug.Log("jump down!");
            transform.position += Vector3.down;
            isStandingOnOneWayPlatform = false;
            return;
        }
        
        
        else if (isGrounded || hasCoyoteTime || hasDoubleJump)
        {
            if (!hasCoyoteTime && hasDoubleJump)
            { 
                hasDoubleJump = false; 
            }
            movementY = jumpForce;

        }
    }


    private void UpdateMovementForce()
    {
        if (movingRight)
        {
            movementX = moveSpeed;
        }
        if (movingLeft)
        {
            movementX = -moveSpeed;
        }
        else
        {
            movementX = Mathf.MoveTowards(movementX, 0, deceleration * Time.deltaTime);
        }


    }
    private bool CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector2.down, 0.5f + skinWidth, oneWayLayer))
        {
            isStandingOnOneWayPlatform = true;
            deceleration = groundDeceleration;
            return true;
        }
        if (Physics.Raycast(transform.position, Vector2.down, 0.5f + skinWidth, groundLayer))
        {
            deceleration = groundDeceleration;
            isStandingOnOneWayPlatform = false;
            return true;
        }
        else
        {
            deceleration = airDeceleration;
            isStandingOnOneWayPlatform = false;
            return false;
        }
        

    }
    private void Jump()
    {
        if (CheckIsGrounded() || hasCoyoteTime || hasDoubleJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!hasCoyoteTime && hasDoubleJump) { hasDoubleJump = false; }
                movementY = jumpForce;
            }
        }

    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded || !hasCoyoteTime) return;
 
        if (coyoteTimer > coyoteTime)
        {
            hasCoyoteTime = false;
        }
        coyoteTimer += Time.deltaTime;
    }

    private bool CheckCollision()
    {
        Debug.DrawRay(transform.position, velocity, Color.green, 0.5f + skinWidth);
        return Physics.Raycast(transform.position, velocity, 0.5f + skinWidth, wallLayer);

    }


    private void HandleVerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionY == -1)
            {
                rayOrigin = rayCastBottomLeft + new Vector2(0f, 0.5f);
            }
            else
            {
                rayOrigin = rayCastTopLeft - new Vector2(0f, 0.5f);
            }
            rayOrigin += Vector2.right * (verticalRaySpacing * i);



            Debug.DrawRay(rayOrigin, Vector2.up * directionY * (0.5f + skinWidth), Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, 0.5f + skinWidth, wallLayer))//rayOrigin, Vector2.up * directionY, out hit, rayLength, wallLayer))
            {

                velocity.y = 0;
                movementY = 0f;

                
            }
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, 0.5f + skinWidth, oneWayLayer))
            {
                if (velocity.y > 0)
                {
                    return;
                }
                else
                {
                    velocity.y = 0;
                }

            }
            
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {

            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = rayCastBottomLeft + new Vector2(0.5f, 0f);
            }
            else
            {
                rayOrigin = rayCastBottomRight - new Vector2(0.5f, 0f); ;
            }
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);



            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, 0.6f + skinWidth, wallLayer))//rayOrigin, Vector2.right * directionX, out hit, rayLength, wallLayer))
            {
                velocity.x = 0;
                movementX = 0;;
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
      
        rayCastBottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastTopLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastBottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastTopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public float GetDownwardForce()
    {
        return downwardForce;
    }
    public void SetDownwardForce(float value)
    {
        downwardForce = value;


    }


}
