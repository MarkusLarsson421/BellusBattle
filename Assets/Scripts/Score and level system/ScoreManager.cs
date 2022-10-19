using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] CameraFocus cameraFocus;
    [SerializeField] LevelManager levelManager;
    private bool hasGivenScore;
    private float giveScoreTimer;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;

    [SerializeField] private bool gameHasStarted; //för att den inte ska börja räkna poäng i lobbyn, är tänkt att sättas till true när man går igenom teleportern

    public bool GameHasStarted
    {
        get { return gameHasStarted; }
        set { gameHasStarted = value; }
    }


    private void Start()
    {
        hasGivenScore = false;
    }

    private void Update()
    {
        if (cameraFocus._targets.Count == 1 && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }
        DontDestroyOnLoad(gameObject);

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
        if(giveScoreTimer >= giveScoreTime && cameraFocus._targets.Count !=0)
        {
            AddScore(cameraFocus._targets[0].transform.gameObject);
            hasGivenScore = true;
            Debug.Log("Has given score to " + cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID);
            Debug.Log(getScore(cameraFocus._targets[0].transform.gameObject));
        }
        else
        {
            giveScoreTimer += Time.deltaTime;
        }
    }

    
}
