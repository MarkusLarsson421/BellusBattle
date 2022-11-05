using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwordBehaviour : MonoBehaviour
{
    [SerializeField] private int playerHoldingThisWeaponID;
    private PlayerMovement playerMovement;

    [SerializeField] private Transform swordPointDirection;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ahhh");
        Vector2 forceDir = swordPointDirection.right;

        if (other.transform.tag == "Player")
        {
            // Find the ID of the player it's colliding with
            playerHoldingThisWeaponID = other.gameObject.GetComponent<PlayerDetails>().playerID;

            if (other.gameObject.GetComponent<PlayerDetails>().playerID.Equals(playerHoldingThisWeaponID))
            {
                Debug.Log("Is owner");
                return;
            }
            else
            {
                playerMovement = other.gameObject.GetComponent<PlayerMovement>();
                playerMovement.AddExternalForce(forceDir * 10f);
                Debug.Log("Knocked back player");
            }
        }
    }
}
