using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinningUIManager : MonoBehaviour
{
    private ScoreManager scoreManager;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private float timeTillQuit; 
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        StartCoroutine(TimeToQuit());
        for(int i = 0; i < panels.Length; i++)
        {
            if(scoreManager.winner == i)
            {
                panels.ElementAt(i).gameObject.SetActive(true);
            }
            else
            {
                panels.ElementAt(i).gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator TimeToQuit()
    {
        yield return new WaitForSeconds(timeTillQuit);
        Application.Quit();
    }
}
