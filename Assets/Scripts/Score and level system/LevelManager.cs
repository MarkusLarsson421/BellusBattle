using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private enum WhichScenesListToPlay{ ScenesFromBuild, ScencesFromList };
    [SerializeField] WhichScenesListToPlay scenceToPlay;
    private int sceneCount;
    [SerializeField] private float timer = 5;
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] scenesToRemove;
    private enum WhichOrderToPlayScenes { Random, NumiricalOrder };
    [SerializeField] WhichOrderToPlayScenes playingScenesOrder;
    private List<string> scenesToChooseFrom = new List<string>();
    private float temp;
    void Start()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScencesFromList) CreateListOfScenesFromList();
        CreateListTofScenesToChooseFrom();
    }
    private void LoadNextScene()
    {
        if (playingScenesOrder == WhichOrderToPlayScenes.Random) LoadNextSceneInRandomOrder();
        else if (playingScenesOrder == WhichOrderToPlayScenes.NumiricalOrder) LoadNextSceneInNumericalOrder();
    }
    private void CreateListOfScenesFromBuild()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount - 1; i++)
        {
            scenesToChooseFrom[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        if (scenesToChooseFrom.Count >= 0) Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    private void CreateListOfScenesFromList()
    {
        foreach (string scene in scenes)
        {
            scenesToChooseFrom.Add(scene);
        }
        if (scenesToChooseFrom.Count >= 0) Debug.LogError("There is no scenes in build. please put scenes in scences list or choose ScenesFromBuild from " + gameObject);
    }
    private void CreateListTofScenesToChooseFrom()
    {
        scenesToChooseFrom.Remove("MainMenu");
    }
    void Update()
    {
        
    }

    public void StartNewLevel()
    {
        LoadNextScene();
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

    }
    private void LoadNextSceneInNumericalOrder()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private void LoadNextSceneInRandomOrder()
    {
        SceneManager.LoadScene(scenesToChooseFrom.ElementAt(Random.Range(1, scenesToChooseFrom.Count)));
    }

}
