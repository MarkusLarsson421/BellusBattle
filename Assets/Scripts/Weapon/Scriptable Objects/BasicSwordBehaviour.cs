using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwordBehaviour : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ahhh");
        if (other.transform.tag == "Revolver")
        {
            other.gameObject.GetComponent<Gun>().Drop();
            Debug.Log("Disarmed weapon");
        }
    }
}
