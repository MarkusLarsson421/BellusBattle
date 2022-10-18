using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    enum AngleRotations{ FullAngleRotation, HalvAngleRotation, EightFixedAnglesRotation, FourFixedAnglesRotation }
    [SerializeField] AngleRotations rotations;

    Vector3 mousePos;
    Vector3 direction;
    Quaternion rotation;
    float angle;

    private void Update()
    {
        //MouseInputToAngleCalculation();
        ChooseAngleRotation();
        transform.rotation = rotation;
    }
    private void MouseInputToAngleCalculation()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        direction = mousePos - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
    public void JoiStickInputToAngleCalculation(InputAction.CallbackContext context)
    {
        Vector2 t = context.ReadValue<Vector2>();
        if (t.x == 0 && t.y == 0) return;
        direction = t - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg -90f; // -90 degrees
        //Debug.Log(direction);
    }
    private void ChooseAngleRotation()
    {
        switch (rotations)
        {
            case AngleRotations.FullAngleRotation:
                FullAngleRotation();
                break;
            case AngleRotations.HalvAngleRotation:
                HalvAngleRotation();
                break;
            case AngleRotations.EightFixedAnglesRotation:
                EightFixedAnglesRotation();
                break;
            case AngleRotations.FourFixedAnglesRotation:
                FourFixedAnglesRotation();
                break;
            default: break;
        }
    }
    private void FullAngleRotation()
    {
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void HalvAngleRotation()
    {
        if (angle > 90) rotation = Quaternion.AngleAxis(90, Vector3.forward);
        else if (angle < -90) rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        else rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void EightFixedAnglesRotation()
    {
        for (int i = -180; i < 180; i += 45)
        {
            if (angle >= i - 22.5 && angle < i +45)
            {
                rotation = Quaternion.AngleAxis(i , Vector3.forward);
            }
        }
    }
    private void FourFixedAnglesRotation()
    {
        for (int i = -180; i < 180; i+= 90)
        {
            if (angle >= i+45 && angle < i + 90+45)
            {
                rotation = Quaternion.AngleAxis(i+90, Vector3.forward);
            }
        }
    }
}
