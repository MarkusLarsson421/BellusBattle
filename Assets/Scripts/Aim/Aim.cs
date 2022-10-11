using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Videon som jag har följt
// https://www.youtube.com/watch?v=gaDFNCRQr38&t=23s
public class Aim : MonoBehaviour
{
    private Transform arm;
    float armAngle;
    public float armSpeed;
    private void Start()
    {
        arm = GetComponent<Transform>();
    }
    private void Update()
    {
        Rotate2();
    }
    void Rotate()
    {
        armAngle += Input.GetAxis("Mouse Y") * armSpeed * -Time.deltaTime;
        armAngle = Mathf.Clamp(armAngle, 0, 360);
        arm.localRotation = Quaternion.AngleAxis(armAngle, Vector3.forward);
    }
    void Rotate2()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(mousePos);
        Vector3 direction = mousePos - transform.position;
        direction.Normalize();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
}
