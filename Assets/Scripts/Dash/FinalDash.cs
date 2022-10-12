using System.Collections;
using UnityEngine;
public class FinalDash : MonoBehaviour
{
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    private bool isDashing;
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 0.01f;

    private Vector3 velocity;
    private PlayerMovement movement;
    //[SerializeField] private TrailRenderer tr; // these variable makes visual effect

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
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

    private IEnumerator DashAction()
    {

        canDash = false;
        isDashing = true;
        temp = movement.GetDownwardForce();
        if (stopGravityWhileDashing) movement.SetDownwardForce(0);
        velocity = new Vector3(dashingDistace, 0f, 0f);
        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(dashingDuration);
        //tr.emitting = false; //See variable TrailRenderer tr
        movement.SetDownwardForce(temp);
        isDashing = false;
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;

    }


}
