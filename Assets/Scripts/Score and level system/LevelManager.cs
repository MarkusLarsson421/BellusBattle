using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadScenesList();
    }
    public void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScencesFromList) CreateListOfScenesFromList();
        CreateListTofScenesToChooseFrom();
    }
    public void LoadNextScene()
    {
        if (playingScenesOrder == WhichOrderToPlayScenes.Random) LoadNextSceneInRandomOrder();
        else if (playingScenesOrder == WhichOrderToPlayScenes.NumiricalOrder) LoadNextSceneInNumericalOrder();
        if (scenesToChooseFrom.Count <= 0)
        {
            LoadScenesList();
        }
    }
    private void CreateListOfScenesFromBuild()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            scenesToChooseFrom.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    private void CreateListOfScenesFromList()
    {
        foreach (string scene in scenes)
        {
            scenesToChooseFrom.Add(scene);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in scences list or choose ScenesFromBuild from " + gameObject);
    }
    private void CreateListTofScenesToChooseFrom()
    {
        scenesToChooseFrom.Remove("MainMenu");
    }
    private void LoadNextSceneInNumericalOrder()
    {
        SceneManager.LoadScene(scenesToChooseFrom.ElementAt(0));
        scenesToChooseFrom.RemoveAt(0);
    }
    private void LoadNextSceneInRandomOrder()
    {
        int randomNumber = Random.Range(0, scenesToChooseFrom.Count);
        SceneManager.LoadScene(scenesToChooseFrom.ElementAt(randomNumber));
        scenesToChooseFrom.RemoveAt(randomNumber);
    }

}












//try
//{
//    SceneManager.LoadScene(scenesToChooseFrom.ElementAt(SceneManager.GetActiveScene().buildIndex + 1));

//}
//catch
//{
//    SceneManager.LoadScene(scenesToChooseFrom.ElementAt(0));
//}