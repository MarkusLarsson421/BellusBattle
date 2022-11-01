using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField, Tooltip("The amount of score required to win.")]
	private int scoreToWin;
	[SerializeField, Tooltip("Main camera for the scene.")]
	private Camera mainCamera;

	public static GameManager Instance;
	public GameState state;
	public static event Action<GameState> OnGameStateChanged;
	public List<GameObject> players;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		UpdateGameState(GameState.Menu);
	}

	private void Update()
	{
		GameObject[] addPlayers = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject addPlayer in addPlayers)
		{
			AddPlayer(addPlayer);
		}
	}
	
	public enum GameState
	{
		Menu, //Players are in the menu.
		RoundStarting, //Round is starting.
		RoundOnGoing, //Round is on going.
		RoundEnding, //Round has ended.
	}

	public void UpdateGameState(GameState newState)
	{
		state = newState;
		switch (newState)
		{
			case GameState.Menu:
				HandleMenu();
				break;
			case GameState.RoundStarting:
				HandleRoundStarting();
				break;
			case GameState.RoundOnGoing:
				HandleRoundOnGoing();
				break;
			case GameState.RoundEnding:
				break;
			default:
				break;
		}
		OnGameStateChanged?.Invoke(newState);
	}

	private void HandleMenu()
	{
		
	}
	
	private void HandleRoundStarting()
	{
		
	}
	
	private void HandleRoundOnGoing()
	{
		if(players.Count <= 1)
		{
			
		}
	}

	public bool AddPlayer(GameObject go)
	{
		if (players.Contains(go)){return false;}
		
		players.Add(go);
		return true;
	}

	public bool RemovePlayer(GameObject go)
	{
		if (players.Contains(go) == false){return false;}
		
		players.Remove(go);
		CheckForVictor();
		return true;
	}

	private void CheckForVictor()
	{
		if (players.Count <= 1)
		{
			//players[1]
			//TODO Victory!
		}
	}
}