using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField, Tooltip("For how long the game shall wait until giving the remaining players their score.")] 
	private float roundEndingPause = 10.0f;

	public static GameManager Instance;
	
	private GameState _state;
	private readonly Dictionary<GameObject, int> _players = new();
	private int _scoreToWin;

	private void Awake()
	{
		Instance = this;
	}

	private void Start(){
		PlayerSpawnEvent.RegisterListener(AddPlayer);

		UpdateGameState(GameState.Menu);
	}

	private void Update()
	{
		if (_state == GameState.RoundOnGoing && _players.Count <= 1){
			UpdateGameState(GameState.RoundEnding);
		}
	}

	public void UpdateGameState(GameState newState)
	{
		_state = newState;
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
				HandleRoundEnding();
				break;
			case GameState.GameEnding:
				HandleGameEnding();
				break;
			default:
				throw new Exception("Unexpected GameState state.");
		}
	}
	
	private void AddPlayer(PlayerSpawnEvent pse)
	{
		if (_players.ContainsKey(pse.playerGo)){return;}
		_players.Add(pse.playerGo, 0);
	}

	private void RemovePlayer(PlayerDeathEvent pde)
	{
		if (_players.ContainsKey(pde.kille) == false){return;}
		_players.Remove(pde.kille);
		CheckForVictor(pde.killedBy);
	}

	public int PlayerCount(){
		return _players.Count;
	}

	public void SetScoreToWin(int score){
		_scoreToWin = score;
	}

	public GameState GetGameState(){
		return _state;
	}

	/*
	 * Called while in the menu
	 */
	private void HandleMenu(){
		/*foreach (var key in _players.Keys){
			PlayerSpawnEvent pse = new PlayerSpawnEvent{
				playerIndex = key.GetComponent<PlayerDetails>().playerID,
				playerGo = key,
			};
			pse.FireEvent();
		}*/
	}
	
	/*
	 * Called when the round is starting.
	 */
	private void HandleRoundStarting(){
		
	}
	
	/*
	 * Called after the round has started.
	 */
	private void HandleRoundOnGoing()
	{
		//Handled in the void Update() method.
	}
	
	/*
	 * 
	 */
	private void HandleRoundEnding(){
		StartCoroutine(RoundEndingPause(roundEndingPause));
	}
	
	/*
	 * 
	 */
	private void HandleGameEnding(){
		
	}

	private IEnumerator RoundStartingPause(float seconds){
		Debug.Log("Pausing for a moment before starting the round.");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Round start!");
		
		UpdateGameState(GameState.RoundOnGoing);
	}

	private IEnumerator RoundEndingPause(float seconds){
		Debug.Log("Round has ended, waiting to see if the remainder will die.");
		yield return new WaitForSeconds(seconds);
		Debug.Log("Giving score to the remainder.");
		
		//In a loop in-case we later decide to include teams.
		foreach(var key in _players.Keys){
			//Adds the score to the player and then checks if they have enough to win.
			if (++_players[key] >= _scoreToWin){
				UpdateGameState(GameState.GameEnding);
				//Unsure if yield break will work or continue to loop through the list.
				yield break;
			}
		}
		//levelManager.LoadNextScene();
	}

	private void CheckForVictor(GameObject killer){
		if (_players[killer] >= _scoreToWin){
			UpdateGameState(GameState.GameEnding);
		}
	}

	private void OnDestroy(){
		//Unregister from the event.
		PlayerSpawnEvent.UnregisterListener(AddPlayer);
	}
}

public enum GameState
{
	Menu, //Players are in the menu.
	RoundStarting, //Round is starting.
	RoundOnGoing, //Round is on going.
	RoundEnding, //Round has ended.
	GameEnding, //Game has ended.
}