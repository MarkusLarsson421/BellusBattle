using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DashAdvanced : MonoBehaviour
{
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    [SerializeField] private bool isInvincibileWhileDashing = true;
    private bool isGrounded;
    private float dashingDistace = 24f;
    private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 1f;
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
                    DrawDetailsE1();
                    break;
                case DashType.E4_GigaChadDash:
                    DrawDetailsE1();
                    break;
            }
        }

        private void DrawDetailsE1()
        {
            EditorGUILayout.LabelField("Details E1");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Distace", GUILayout.MaxWidth(50));
            dash.dashingDistace = EditorGUILayout.FloatField(dash.dashingDistace);
            EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(50));
            dash.dashingDuration = EditorGUILayout.FloatField(dash.dashingDuration);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDetailsE2()
        {
            DrawDetailsE1();
            EditorGUILayout.LabelField("Details E2");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("AirDistace", GUILayout.MaxWidth(50));
            dash.dashingDistace = EditorGUILayout.FloatField(dash.dashingDistace);
            EditorGUILayout.LabelField("AirDuration", GUILayout.MaxWidth(50));
            dash.dashingDuration = EditorGUILayout.FloatField(dash.dashingDuration);
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}
