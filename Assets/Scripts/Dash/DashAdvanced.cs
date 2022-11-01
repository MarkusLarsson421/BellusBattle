using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEditor.Rendering.LookDev;
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
    [Header(" ")]
    private float dashingDistace = 24f;
    private float dashingDuration = 0.2f;
    private float airDashingDistace = 24f;
    private float airDashingDuration = 0.2f;
    private float dashUpAngle = 20f;
    private float dashDownAngle = 20f;
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
    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(DashAdvanced))]
    public class DashAdvancedEditor : Editor
    {
        DashAdvanced dash;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            dash = (DashAdvanced)target;
            switch (dash.dashType)
            {
                case DashType.E1_BasicDash:
                    DrawDetailsE1();
                    break;
                case DashType.E2_TwoStateDash:
                    DrawDetailsE2();
                    break;
                case DashType.E3_AdvancedDash:
                    DrawDetailsE3();
                    break;
                case DashType.E4_GigaChadDash:
                    DrawDetailsE4();
                    break;
            }
        }

        private void DrawDetailsE1()
        {
            EditorGUILayout.LabelField("Details E1", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Distace", GUILayout.MaxWidth(100));
            dash.dashingDistace = EditorGUILayout.FloatField(dash.dashingDistace);
            EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(100));
            dash.dashingDuration = EditorGUILayout.FloatField(dash.dashingDuration);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDetailsE2()
        {
            DrawDetailsE1();
            EditorGUILayout.LabelField("Details E2", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AirDistace", GUILayout.MaxWidth(100));
            dash.airDashingDistace = EditorGUILayout.FloatField(dash.airDashingDistace);
            EditorGUILayout.LabelField("AirDuration", GUILayout.MaxWidth(100));
            dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDetailsE3()
        {
            DrawDetailsE2();
            EditorGUILayout.LabelField("Details E3", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Up Angle", GUILayout.MaxWidth(100));
            dash.dashUpAngle = EditorGUILayout.FloatField(dash.dashUpAngle);
            EditorGUILayout.LabelField("Down Angle", GUILayout.MaxWidth(100));
            dash.dashDownAngle = EditorGUILayout.FloatField(dash.dashDownAngle);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDetailsE4()
        {
            DrawDetailsE3();
            EditorGUILayout.LabelField("Details E4", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();
        }
        //private void TextIO(string title, float widthSpacing, DashAdvanced d)
        //{
        //    EditorGUILayout.LabelField(title, GUILayout.MaxWidth(widthSpacing));
        //    dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
        //}
    }
#endif
    #endregion

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
                AdvancedDashAction();
                break;
            case DashType.E4_GigaChadDash:
                break;
        }

    }
    private void SetDirection()
    {
        if (direction.x == 0 && direction.y == 0)
        {
            if (isFacingRight) direction = Vector2.right;
            else direction = Vector2.left;
        }
        else if (isFacingRight)
        {
            direction = Vector2.right * direction;
            direction.Normalize();
        }
        else
        {
            direction = Vector2.left * direction;
            direction.Normalize();
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
        if (!isFacingRight)
        {
            currentDashingDistace *= -1;
        }
    }
    private IEnumerator BasicDashAction()
    {
        StartDashProtocol();
        CheckBoolValues();
        velocity = new Vector3(currentDashingDistace - movement.Velocity.x, direction.y * 4 * currentDashingDistace, 0f);
        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;

    }
    private void TwoStateDashAction()
    {
        if (movement.CheckIsGrounded())
        {
            currentDashingDistace = airDashingDistace;
            currentDashingDuration = airDashingDuration;
        }
        StartCoroutine(BasicDashAction());
    }
    private void AdvancedDashAction()
    {
        //Change Direction
        TwoStateDashAction();
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
            currentDashingDistace = hit.distance * 4 - 1f; /// 4 is a the number that make dash distance works correct // 0.5f är Players halv storlek
            }
        }
    }
}
