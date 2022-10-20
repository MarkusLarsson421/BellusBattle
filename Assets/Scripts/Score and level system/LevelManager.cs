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
        
        CreateListOfScenes();
    }

    [System.Obsolete]
    void Update()
    {
        
    }

    public void StartNewLevel()
    {
        /*
        int randomNumber;
        string curentScene = scenes[0]; // Main menu scene
       
        do
        {
            randomNumber = (int)Random.Range(0, scenes.Length - 1);
        } while (scenes[randomNumber] == curentScene);
        SceneManager.LoadScene(scenes[randomNumber]);
        temp = 0;
        */
        int index = Random.Range(1, sceneCount);
        SceneManager.LoadScene(index);
        Debug.Log("Scene Loaded: " + index);
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
