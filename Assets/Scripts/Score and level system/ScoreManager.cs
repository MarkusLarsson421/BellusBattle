using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    Dictionary<GameObject, int> ScoreDic = new Dictionary<GameObject, int>();
    [SerializeField] CameraFocus cameraFocus;
    bool hasGivenScore;

    private void Start()
    {
        hasGivenScore = false;
    }

    private void Update()
    {
        if (cameraFocus._targets.Count == 1 && !hasGivenScore)
        {
            AddScore(cameraFocus._targets[0].transform.gameObject);
            hasGivenScore = true;
        }

        //Debug.Log(ScoreDic[cameraFocus._targets[0].transform.gameObject]);
    }

    public void AddScore(GameObject winner)
    {
        if (!ScoreDic.ContainsKey(winner))
        {
            ScoreDic[winner] = 1;
        }
        else
        {
            ScoreDic[winner]++;
        }
        
    }
}
