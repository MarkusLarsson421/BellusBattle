using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEditor.Rendering.LookDev;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DashAdvanced : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    [SerializeField] private bool isInvincibileWhileDashing = true;
    private bool isFacingRight;
    private bool isDashing;
    private bool onControlOverride;
    [Header("E1")]
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [Header("E2")]
    [SerializeField] private float airDashingDistace = 24f;
    [SerializeField] private float airDashingDuration = 0.2f;
    [Header("E3")]
    [SerializeField] private float dashUpAngle = 20f;
    [SerializeField] private float dashDownAngle = 20f;
    [Header("Extra")]
    [SerializeField] private float dashingActivationCooldown = 1f;
    [SerializeField] private float dashingInvincibilityDuration = 1f;

    private Vector3 direction;
    private Vector3 velocity;
    private PlayerMovement movement;
    private PlayerHealth health;
    private float currentDashingDistace;
    private float currentDashingDuration;
    private float gravity;
    public UnityEvent dashEvent;

    enum DashType { E1_BasicDash, E2_TwoStateDash, E3_AdvancedDash, E4_GigaChadDash }
    [SerializeField] private DashType dashType;

    //#region Editor
    //[CustomEditor(typeof(DashAdvanced))]
    //public class DashAdvancedEditor : Editor
    //{
    //    DashAdvanced dash;
    //    public float dashingDistac;
    //    public override void OnInspectorGUI()
    //    {
    //        dash = (DashAdvanced)target;
    //        base.OnInspectorGUI();
    //        switch (dash.dashType)
    //        {
    //            case DashType.E1_BasicDash:
    //                DrawDetailsE1();
    //                break;
    //            case DashType.E2_TwoStateDash:
    //                DrawDetailsE2();
    //                break;
    //            case DashType.E3_AdvancedDash:
    //                DrawDetailsE3();
    //                break;
    //            case DashType.E4_GigaChadDash:
    //                DrawDetailsE4();
    //                break;
    //        }
    //    }

    //    private void DrawDetailsE1()
    //    {
    //        EditorGUILayout.LabelField("Details E1", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        GUILayout.Label("Distace", GUILayout.MaxWidth(100));
    //        //dash.SetDashingDistance(EditorGUILayout.FloatField(dash.dashingDistace));
    //        dashingDistac = EditorGUILayout.FloatField("", dashingDistac);
    //        DashAdvanced.dashingDistace = dashingDistac;
    //        EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(100));
    //        dash.dashingDuration = EditorGUILayout.FloatField(dash.dashingDuration);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE2()
    //    {
    //        DrawDetailsE1();
    //        EditorGUILayout.LabelField("Details E2", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("AirDistace", GUILayout.MaxWidth(100));
    //        dash.airDashingDistace = EditorGUILayout.FloatField(dash.airDashingDistace);
    //        EditorGUILayout.LabelField("AirDuration", GUILayout.MaxWidth(100));
    //        dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE3()
    //    {
    //        DrawDetailsE2();
    //        EditorGUILayout.LabelField("Details E3", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("Up Angle", GUILayout.MaxWidth(100));
    //        dash.dashUpAngle = EditorGUILayout.FloatField(dash.dashUpAngle);
    //        EditorGUILayout.LabelField("Down Angle", GUILayout.MaxWidth(100));
    //        dash.dashDownAngle = EditorGUILayout.FloatField(dash.dashDownAngle);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE4()
    //    {
    //        DrawDetailsE3();
    //        EditorGUILayout.LabelField("Details E4", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    //private void TextIO(string title, float widthSpacing, DashAdvanced d)
    //    //{
    //    //    EditorGUILayout.LabelField(title, GUILayout.MaxWidth(widthSpacing));
    //    //    dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
    //    //}
    //}
    ////#endregion

    public void DashWithJoystick(InputAction.CallbackContext context)
    {
        if (canDash && !isDashing)
        {
            CheckDashType();
        }
    }
    public void CheckDashWithJoystickDirection(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }
    private void Start()
    {
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        gravity = movement.DownwardForce;
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
    private void CheckDashType()
    {
        switch (dashType)
        {
            case DashType.E1_BasicDash:
                StartCoroutine(BasicDashAction());
                break;
            case DashType.E2_TwoStateDash:
                TwoStateDashAction();
                break;
            case DashType.E3_AdvancedDash:
               StartCoroutine(AdvancedDashAction());
                break;
            case DashType.E4_GigaChadDash:
                break;
        }

    }
    private IEnumerator BasicDashAction()
    {
        StartDashProtocol();
        CheckBoolValues();
        velocity = new Vector3(currentDashingDistace - movement.Velocity.x, 0f, 0f);
        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;
        //direction.y * 4 * currentDashingDistace
    }
    private void StartDashProtocol()
    {
        SetDirection();
        CheckForCollision();
        canDash = false;
        isDashing = true;
        dashEvent.Invoke();
    }
    private void EndDashProtocol()
    {
        //tr.emitting = false; //See variable TrailRenderer tr
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        movement.DownwardForce = gravity;
        isDashing = false;
        onControlOverride = false;
    }
    private void SetDirection()
    {
        if (onControlOverride)
        {
            return;
        }
        else if (isFacingRight && !onControlOverride)
        {
            direction = Vector2.right * direction;
        }
    }
    private void CheckBoolValues()
    {
        if (stopGravityWhileDashing)
        {
            movement.DownwardForce = 0f;
        }
        if (isInvincibileWhileDashing)
        {
            StartCoroutine(Invincibility());
        }
        if (!isFacingRight && !onControlOverride)
        {
            currentDashingDistace *= -1;
        }
    }

    private IEnumerator AdvancedDashAction()
    {
        float angle;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // -90 degrees

        if (angle >= dashUpAngle - 20 && angle < dashUpAngle + 20) // make variable instead for 20
        {
            onControlOverride = true;
            direction = Vector3.up;
        }
        if (!movement.CheckIsGrounded())
        {
            currentDashingDistace = airDashingDistace;
            currentDashingDuration = airDashingDuration;
        }
        StartDashProtocol();
        CheckBoolValues();
        velocity = new Vector3(0f, direction.y * currentDashingDistace, 0f);
        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;
        //direction.y * 4 * currentDashingDistace
    }
    private void TwoStateDashAction()
    {
        if (!movement.CheckIsGrounded())
        {
            currentDashingDistace = airDashingDistace;
            currentDashingDuration = airDashingDuration;
        }
        StartCoroutine(BasicDashAction());
    }

    private IEnumerator Invincibility()
    {
        health.SetInvincible(true);
        yield return new WaitForSeconds(dashingInvincibilityDuration);
        health.SetInvincible(false);
    }
    private void Flip()
    {
        if (isFacingRight && movement.Velocity.x < 0f || !isFacingRight && movement.Velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
        }
    }
    private void CheckForCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, currentDashingDistace / 4, movement.CollisionLayer)) //4 is a the number that make dash distance works correct 
        {
            if (hit.distance * 4 < currentDashingDistace/4)
            {
                currentDashingDistace = 0;
            }
            else
            {
            currentDashingDistace = hit.distance * 4 - 1f; /// 4 is a the number that make dash distance works correct // 0.5f �r Players halv storlek
            }
        }
    }
}
