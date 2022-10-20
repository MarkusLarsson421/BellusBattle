using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] CameraFocus cameraFocus;
    [SerializeField] LevelManager levelManager;
    [SerializeField] int pointsToWin;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private bool hasGivenScore;
    private float giveScoreTimer;
    private bool hasOnePlayerLeft;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;

    [SerializeField] private bool gameHasStarted; //för att den inte ska börja räkna poäng i lobbyn, är tänkt att sättas till true när man går igenom teleportern

    public bool GameHasStarted
    {
        get { return gameHasStarted; }
        set { gameHasStarted = value; }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level != 0)
        {
            gameHasStarted = true;
        }
        if (cameraFocus == null)
        {
            
           cameraFocus =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
           Debug.Log("camammammam");
        }
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        hasGivenScore = false;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(cameraFocus._targets.Count == 1)
        {
            hasOnePlayerLeft = true;
        }
        else if(cameraFocus._targets.Count > 1)
        {
            hasOnePlayerLeft = false;
        }
        if(hasOnePlayerLeft && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }
        Debug.Log("hahaha");
        
        Debug.Log("hihihihi");

    }

    public void AddPlayers(GameObject player)
    {
        players.Add(player);
    }

    private void AddScore(GameObject winner) //TODO använd playerID istället för hela spelarobjektet
    {
        if (!scoreDic.ContainsKey(winner))
        {
            scoreDic[winner] = 1;
        }
        else
        {
            scoreDic[winner]++;
        }

    }

    public int getScore(GameObject player)
    {
        if (!scoreDic.ContainsKey(player)) return 0;

        return scoreDic[player];
    }

    private void GiveScoreAfterTimer()
    {
        if (giveScoreTimer >= giveScoreTime)
        {
            Debug.Log("im runnig´ng");
            if(cameraFocus._targets.Count != 0)
            {
                AddScore(cameraFocus._targets[0].transform.gameObject);
                hasGivenScore = true;
                Debug.Log("Has given score to " + cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID);
                Debug.Log("score " + getScore(cameraFocus._targets[0].transform.gameObject));
                if (getScore(cameraFocus._targets[0].transform.gameObject) == pointsToWin)
                {
                    Debug.Log("YOU HAVE WON, " + cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID);
                }
            }
            else
            {
                Debug.Log("Its a draaaaw!");
            }

            foreach(GameObject player in players)
            {
                player.SetActive(true);
            }

            levelManager.StartNewLevel();
            giveScoreTimer = 0;
        }
        else
        {
            giveScoreTimer += Time.deltaTime;
        }
    }


}
