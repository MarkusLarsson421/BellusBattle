using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private CameraFocus CF;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Sätter isAlive bool till false
            other.gameObject.GetComponent<PlayerHealth>().KillPlayer();
            other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            other.gameObject.GetComponent<PlayerMovement>().StopPlayer();
            CF.RemoveTarget(other.transform);
        }
    }
}
