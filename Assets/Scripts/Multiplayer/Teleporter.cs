using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public int playerAmountOnTeleporter = 0;
    //public EventStarter eventStarter;
    [SerializeField] private GameObject infoJoinText;
    PlayerSpawnManager playerSpawnManager;

    private void Start()
    {
        infoJoinText.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter++;
        }

        if (playerAmountOnTeleporter >= 2 || playerAmountOnTeleporter == playerSpawnManager.listOfPlayers.Count)
        {
            SceneManager.LoadScene("TestTemp_StartScene");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter--;
        }
    }
}
