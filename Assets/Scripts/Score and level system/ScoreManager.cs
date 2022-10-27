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
    [SerializeField] public List<GameObject> players = new List<GameObject>();
    [SerializeField] private bool hasGivenScore;
    [SerializeField] private float giveScoreTimer;
    [SerializeField] private bool hasOnePlayerLeft;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;

    [SerializeField] private bool gameHasStarted; //för att den inte ska börja räkna poäng i lobbyn, är tänkt att sättas till true när man går igenom teleportern
    public int winner;

    public bool GameHasStarted
    {
        get { return gameHasStarted; }
        set { gameHasStarted = value; }
    }
    public void SetPointsToWin(int value)
    {
        pointsToWin = value;
    }

    private void OnLevelWasLoaded(int level)
    {
        giveScoreTimer = 0;
        if (level != 0)
        {
            gameHasStarted = true;
        }
        if (cameraFocus == null)
        {
            
           cameraFocus =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
           //Debug.Log("camammammam");
        }
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!gameHasStarted) return;
        
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
        return !scoreDic.ContainsKey(player) ? 0 : scoreDic[player];
    }

    private void GiveScoreAfterTimer()
    {
        giveScoreTimer += Time.deltaTime;
        if (giveScoreTimer <= giveScoreTime) return;

        
        if (cameraFocus._targets.Count != 0)
        {
            AddScore(cameraFocus._targets[0].transform.gameObject);
            hasGivenScore = true;
            Debug.Log("Has given score to " + cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID);
            Debug.Log("score " + getScore(cameraFocus._targets[0].transform.gameObject));
            if (getScore(cameraFocus._targets[0].transform.gameObject) == pointsToWin)
            {
                
                winner = cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID;
                Debug.Log("vinanren är"+winner);
                levelManager.Finish();
                Debug.Log("YOU HAVE WON, " + cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID);
                return;
            }
        }
        else
        {
            Debug.Log("Its a draaaaw!");
        }
        hasGivenScore = false;
        levelManager.LoadNextScene();





    }


}
