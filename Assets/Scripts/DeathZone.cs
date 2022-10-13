using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    PlayerMovement pm;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Sätter isAlive bool till false
            Debug.Log("FUCK");
            //pm.gameObject.GetComponent<PlayerMovement>().isAlive = false;
            //collision.gameObject.SetActive(false);
        }
    }
}
