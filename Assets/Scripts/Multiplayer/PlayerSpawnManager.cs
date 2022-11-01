using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
	[SerializeField] 
	protected ScoreManager scoreManager;
	[SerializeField] 
	private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
	[SerializeField] 
	private GameObject[] players;
	
	private int _sceneIndex;
	private PlayerJoinManager _playerJoinManager;
    

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        foreach (GameObject player in scoreManager.players)
        {
            player.SetActive(true);
        }
        players = GameObject.FindGameObjectsWithTag("Player"); // Used for when changing level
        
        for(int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].GetComponent<FinalDash>().ResetValues();
            players[i].GetComponent<PlayerHealth>().UnkillPlayer();
            //players[i].gameObject.GetComponent<PlayerInput>().gameObject.SetActive(true);
            players[i].transform.position = spawnLocations[i].position;
            //Debug.Log(players[i].transform.position);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        for (int i = 0; i < players.Length; i++)
            players[i].gameObject.SetActive(true);
    }
}
