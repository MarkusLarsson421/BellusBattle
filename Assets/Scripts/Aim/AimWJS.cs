using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWJS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float num;
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Debug.Log(mousePos);

        num += Time.deltaTime * 30;
        transform.rotation = Quaternion.AngleAxis(num, Vector3.forward);

    }
}
