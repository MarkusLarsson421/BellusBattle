using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private enum WhichScenesListToPlay { ScenesFromBuild, ScenesFromList, ScenesFromBuildAndList };
    [SerializeField] WhichScenesListToPlay scenceToPlay;
    private enum WhichOrderToPlayScenes { Random, NumiricalOrder };
    [SerializeField] WhichOrderToPlayScenes playingScenesOrder;
    private int sceneCount;
    [SerializeField] private string[] scenes;
    [SerializeField] private string[] scenesToRemove;

    private List<string> scenesToChooseFrom = new List<string>();
    public List<string> GetScencesList()
    {
        return scenesToChooseFrom;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadScenesList();
    }
    public void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromList) CreateListOfScenesFromList();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuildAndList) { CreateListOfScenesFromBuild(); CreateListOfScenesFromList(); }
        CreateListTofScenesToChooseFrom();
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
    public void LoadNextScene()
    {
        if (playingScenesOrder == WhichOrderToPlayScenes.Random) LoadNextSceneInRandomOrder();
        else if (playingScenesOrder == WhichOrderToPlayScenes.NumiricalOrder) LoadNextSceneInNumericalOrder();
        if (scenesToChooseFrom.Count <= 0)
        {
            LoadScenesList();
        }
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
