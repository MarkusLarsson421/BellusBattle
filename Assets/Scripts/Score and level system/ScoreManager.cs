using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] CameraFocus cameraFocus;
    [SerializeField] LevelManager levelManager;
    private bool hasGivenScore;
    [SerializeField] private float giveScoreTimer;
    [SerializeField] private float giveScoreTime;


    private void Start()
    {
        hasGivenScore = false;
    }

    private void Update()
    {
        if (cameraFocus._targets.Count == 1 && !hasGivenScore)
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
        }
        else
        {
            giveScoreTimer += Time.deltaTime;
        }
    }

    
}
