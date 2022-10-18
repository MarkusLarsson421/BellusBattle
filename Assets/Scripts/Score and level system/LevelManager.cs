using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int sceneCount;
    private string[] scenes;
    [SerializeField] private float timer = 5;
    private float temp;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        CreateListOfScenes();
    }

    [System.Obsolete]
    void Update()
    {
        StartNewLevel();
    }

    private void StartNewLevel()
    {
        int randomNumber;
        string curentScene = scenes[0]; // Main menu scene
        if (temp < timer)
        {
            temp += Time.deltaTime;
            return;
        }
        do
        {
            randomNumber = (int)Random.Range(0, scenes.Length - 1);
        } while (scenes[randomNumber] == curentScene);
        SceneManager.LoadScene(scenes[randomNumber]);
        temp = 0;
    }
    private void CreateListOfScenes()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount-1; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            Debug.Log(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        
    }

}
