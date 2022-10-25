using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class FinalDash : MonoBehaviour
{
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    private bool isDashing;
    private bool isFacingRight = true;
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 1000f;
    //[SerializeField] private TrailRenderer tr; // these variable makes visual effect

    private Vector3 velocity;
    private PlayerMovement movement;
    private PlayerHealth health;
    private float currentDashingDistace;
    private float currentDashingDuration;

    public UnityEvent dashEvent;

    private void Start()
    {
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
    }

    private void OnLevelWasLoaded(int level)
    {
        isDashing = false;
    }
    void Update()
    {
        Flip();
        //DashWithKeyboard();
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
        {
            transform.position += velocity * Time.deltaTime;
            return;
        }
        velocity = new Vector3(0, velocity.y, velocity.z);
    }
    float temp;
    private void DashWithKeyboard()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // INPUT
        {
            StartCoroutine(DashAction());
        }
    }
    public void DashWithJoystick(InputAction.CallbackContext context)
    {
        if (isDashing)
        {
            return;
        }
        if (canDash)
        {
            StartCoroutine(DashAction());
        }
    }

    private IEnumerator DashAction()
    {
        CheckForCollision();
        canDash = false;
        isDashing = true;
        health.SetIsInvinsable(true);
        temp = movement.GetDownwardForce();
        if (stopGravityWhileDashing) movement.SetDownwardForce(-15);
        dashEvent.Invoke();

        if (isFacingRight)
        {
            velocity = new Vector3(currentDashingDistace - movement.velocity.x, 0f, 0f);
        }
        else if (!isFacingRight)
        {
            velocity = new Vector3(-currentDashingDistace - movement.velocity.x, 0f, 0f);
        }

        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(currentDashingDuration);
        //tr.emitting = false; //See variable TrailRenderer tr
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        health.SetIsInvinsable(false);
        movement.SetDownwardForce(temp);
        isDashing = false;
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;

    }
    private void Flip()
    {
        if (isFacingRight && movement.velocity.x < 0f || !isFacingRight && movement.velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
        }
    }
    float x;
    private void CheckForCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, currentDashingDistace/4, movement.WallLayer) && isFacingRight) //4 is a the number that make dash distance works correct 
        {
            if (hit.distance * 4 < 3)
            {
                currentDashingDistace = 0;
                return;
            }
            currentDashingDistace = hit.distance * 4 - 1f; /// 4 is a the number that make dash distance works correct // 0.5f är Players halv storlek
        }
        else if (Physics.Raycast(transform.position, Vector3.left, out hit, currentDashingDistace / 4, movement.WallLayer) && !isFacingRight) // 4 is a the number that make dash distance works correct 
        {
            if (hit.distance * 4 < 3)
            {
                currentDashingDistace = 0;
                return;
            }
            currentDashingDistace = hit.distance * 4 - 0.5f; /// 4 is a the number that make dash distance works correct // 0.5f är Players halv storlek
        }

    }


}
