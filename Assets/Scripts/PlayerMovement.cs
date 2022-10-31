using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] [Tooltip("Variabel som påverkar spelarens hastighet")] [Range(1f, 25f)] private float moveSpeed;
    [SerializeField] [Tooltip("Hur högt spelaren kan hoppa")] [Range(1f, 25f)] private float jumpForce;
    [SerializeField] [Tooltip("Hur snabbt spelaren decelererar i luften")] [Range(25f, 150f)] private float airDeceleration;
    [SerializeField] [Tooltip("Hursnabbt spelaren decelererar på marken")] [Range(25f, 150f)] private float groundDeceleration;
    [SerializeField] [Tooltip("Påverkar hur snabbt spelaren förlorar energi i ett hopp")] [Range(10f, 300f)] private float airResistance;
    [SerializeField] [Tooltip("Hur mycket gravitation som påverkar spelaren i luften")] [Range(-100f, 0f)] private float downwardForce;
    [SerializeField] [Tooltip("ACCELERATION!!")] [Range(1f, 50000f)] private float acceleration;
   
    
    [SerializeField, Range(0f, 1f)] private float doubleJumpDecreaser;
    [SerializeField, Range(-1f, 0f)] private float downwardInputBound;
    [SerializeField] private float jumpBufferTime = 0.2f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask oneWayLayer;
    

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject doubleJumpVFX;
    
    [SerializeField] private AudioSource JumpSound;
    [SerializeField] private AudioSource doubleJumpSound;


    public UnityEvent jumpEvent;

    public LayerMask WallLayer
    {
        get { return collisionLayer; }
    }

    public Vector2 Velocity
    {
        get { return velocity; }
    }

    public float DownwardForce
    {
        get { return downwardForce; }
        set { downwardForce = value; }
    }

    private bool isGrounded
    {
        get { return CheckIsGrounded(); }
    }

    private Vector2 velocity;
    private Vector2 rayCastBottomLeft, rayCastBottomRight, rayCastTopRight, rayCastTopLeft;

    private Vector2 verticalRayOffset;
    private Vector2 horizontalRayOffset;

    private GameObject MuzzleFlashIns;

    private BoxCollider boxCollider;

    private int horizontalRayCount = 6; 
    private int verticalRayCount = 4;

    private float movementX, movementY;
    private float deceleration;
    private float coyoteTime = 0.2f;
    private float coyoteTimer;
    private float horizontalSkinWidth = 0.2f;
    private float verticalSkinWidth = 0.1f;
    private float downwardInput;
    private float verticalRaySpacing, horizontalRaySpacing;
    private float movementAmount;
    
    private float bufferTimer;
    private float initialSpeed;

    private bool hasJumpedOnGround;
    private bool hasCoyoteTime;
    private bool hasDoubleJump;
    private bool movingLeft, movingRight;
    private bool isStandingOnOneWayPlatform;
    private bool runBufferTimer;
    private bool hasJumpBuffer;
  
    void Start()
    {
        initialSpeed = moveSpeed - 5; //Används för acceleration
        boxCollider = GetComponent<BoxCollider>();
        CalculateRaySpacing();
        DontDestroyOnLoad(gameObject);
        CalculateRaycastOffset();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateRayCastOrgins();
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
            hasJumpedOnGround = false;
            
            if (hasJumpBuffer)
            {
                Jump();
                hasJumpBuffer = false;
                runBufferTimer = false;
            }
            
        }
        velocity = new Vector2(movementX, movementY);
        JumpBuffer();

        if (velocity.y != 0)
        {
            HandleVerticalCollisions(ref velocity);
        }
        if (velocity.x != 0)
        {
            HandleHorizontalCollisions(ref velocity);
        }
        transform.Translate(velocity * Time.deltaTime);
        
        playerAnimator.SetFloat("Speed", movementX);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        downwardInput = ctx.ReadValue<Vector2>().y;
        movementAmount = ctx.ReadValue<Vector2>().x;
      
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
        else if (ctx.ReadValue<Vector2>().x < 0.1f || ctx.ReadValue<Vector2>().x > -0.1f)
        {
            movingRight = false;
            movingLeft = false;
        }

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Jump();
        }
    }

    private void Jump()
    {
        float jumpDecreaser = 1f;
        float playerHeight = 1.8f;
        if (downwardInput <= downwardInputBound && isStandingOnOneWayPlatform)
        {
            //Debug.Log("jump down!");
            transform.position += Vector3.down * playerHeight;
            isStandingOnOneWayPlatform = false;
            return;
        }
        else if (isGrounded || hasCoyoteTime || hasDoubleJump)
        {
            if (isGrounded)
            {
                hasJumpedOnGround = true;
                JumpSound.Play();
            }
            if (!hasCoyoteTime && hasDoubleJump)
            {
                doubleJumpSound.Play();
                MuzzleFlashIns = Instantiate(doubleJumpVFX, transform.position, transform.rotation);
                StartCoroutine(VFXRemover());
                hasDoubleJump = false;
                jumpDecreaser = doubleJumpDecreaser;
            }
            movementY = jumpForce * jumpDecreaser;
            jumpEvent.Invoke();
        }
        else
        {
            //Debug.Log("JUMPY");
            runBufferTimer = true;
            bufferTimer = 0;
        }
    }
    private IEnumerator VFXRemover()
    {
        yield return new WaitForSeconds(1f);
        Destroy(MuzzleFlashIns);
    }

    private void JumpBuffer()
    {
        if (!runBufferTimer) return;
        bufferTimer += Time.deltaTime;
        if(bufferTimer <= jumpBufferTime)
        {
            hasJumpBuffer = true;
        }
        else
        {
            hasJumpBuffer = false;
            runBufferTimer = false;
        }
    }

    private void EdgeControl(RaycastHit hit)
    {
        float hitpointY = hit.point.y;
        Collider platformCollider = hit.collider;
        Bounds col = platformCollider.bounds;

        float colliderDif = col.max.y - hitpointY;
        Debug.Log(colliderDif);

        if (colliderDif > 0 && colliderDif < 0.5f)
        {
            if (velocity.x < 0f)
            {
                transform.position = new Vector3(col.max.x, col.max.y + 0.2f, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(col.min.x, col.max.y + 0.2f, transform.position.z);
            }
        }
    }

    private void UpdateMovementForce()
    {
        if(movementAmount > 0.1f || movementAmount < -0.1f)
        {
            if (movingRight)
            {
                movementX = Mathf.MoveTowards(initialSpeed, moveSpeed, acceleration * Time.deltaTime);
            }
            if (movingLeft)
            {
                movementX = Mathf.MoveTowards(initialSpeed, moveSpeed, acceleration * Time.deltaTime);
            }
            movementX *= movementAmount;
        }
        else
        {
            movementX = Mathf.MoveTowards(movementX, 0, deceleration * Time.deltaTime);
        }
    }
    private bool CheckIsGrounded()
    {
        
        if (Physics.Raycast(boxCollider.bounds.center, Vector2.down, 0.9f + verticalSkinWidth, oneWayLayer))
        {
            isStandingOnOneWayPlatform = true;
            deceleration = groundDeceleration;
            return true;
        }
        if (Physics.Raycast(boxCollider.bounds.center, Vector2.down, 0.9f + verticalSkinWidth, groundLayer))
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
    private void UpdateCoyoteTime()
    {
        if (isGrounded || !hasCoyoteTime) return;
 
        if (coyoteTimer > coyoteTime || hasJumpedOnGround)
        {
            hasCoyoteTime = false;
        }
        coyoteTimer += Time.deltaTime;
    }

    private void HandleVerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        Bounds bounds = boxCollider.bounds;

        float rayLength = ((bounds.max.y - bounds.min.y) / 2f) + verticalSkinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionY == -1)
            {
                rayOrigin = rayCastBottomLeft + verticalRayOffset;
            }
            else
            {
                rayOrigin = rayCastTopLeft - verticalRayOffset;
            }
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
           
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, collisionLayer))//rayOrigin, Vector2.up * directionY, out hit, rayLength, wallLayer))
            {
                velocity.y = 0;
                movementY = 0f;  
            }
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, rayLength, oneWayLayer))
            {
                if (velocity.y > 0)
                {
                    return;
                }
                else if(hit.collider.bounds.min.y < bounds.min.y)
                {
                    velocity.y = 0;
                }
            } 
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Bounds bounds = boxCollider.bounds;
        float rayLength = ((bounds.max.x - bounds.min.x) / 2) + horizontalSkinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = rayCastBottomLeft + horizontalRayOffset;
            }
            else
            {
                rayOrigin = rayCastBottomRight - horizontalRayOffset;
            }
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
     
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, rayLength, collisionLayer))//rayOrigin, Vector2.right * directionX, out hit, rayLength, wallLayer))
            {
                if(i == 0)
                {
                    EdgeControl(hit);
                }
                velocity.x = 0;
                movementX = 0;
            }
        }
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private void UpdateRayCastOrgins()
    {
        Bounds bounds = boxCollider.bounds;
        rayCastBottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastTopLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastBottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastTopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaycastOffset()
    {
        Bounds bounds = boxCollider.bounds;
        horizontalRayOffset = new Vector2((bounds.max.x - bounds.min.x) / 2, 0f);
        verticalRayOffset = new Vector2(0f, (bounds.max.y - bounds.min.y) / 2);
    }

    public void StopPlayer()
    {
        velocity.x = 0;
        velocity.y = 0;
        movementX = 0;
        movementY = 0;
    }
}
