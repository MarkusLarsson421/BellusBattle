using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] CameraFocus CF;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Sätter isAlive bool till false
            other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            CF.RemoveTarget(other.transform);
            //other.gameObject.GetComponent<PlayerInputManager>().gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }
}
