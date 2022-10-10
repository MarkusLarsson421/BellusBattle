using System.Collections;
using UnityEngine;
public class Dash2 : MonoBehaviour
{
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    private bool isDashing;
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 0.01f;

    private Rigidbody rb;
    //[SerializeField] private TrailRenderer tr; // these variable makes visual effect

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(DashAction());
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
    }
    private IEnumerator DashAction()
    {
        canDash = false;
        isDashing = true;
        if(stopGravityWhileDashing) rb.useGravity = false;
        rb.velocity = new Vector3(transform.localScale.x * dashingDistace, 0f, 0f);
        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(dashingDuration);
        //tr.emitting = false; //See variable TrailRenderer tr
        rb.useGravity = true;
        isDashing = false;
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;
    }
}
