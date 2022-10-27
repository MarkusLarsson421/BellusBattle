using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    private enum WhichScenesListToPlay{ ScenesFromBuild, ScenesFromList, ScenesFromBuildAndList };
    [SerializeField] WhichScenesListToPlay scenceToPlay;
    private enum WhichOrderToPlayScenes { Random, NumiricalOrder };
    [SerializeField] WhichOrderToPlayScenes playingScenesOrder;
    private int sceneCount;
    [SerializeField] private string[] scenes;
    [SerializeField] private List<LevelDetails> levels = new List<LevelDetails>();
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject levelXPrefab;


    public List<string> scenesToChooseFrom = new List<string>();
    public List<string> GetScencesList()
    {
        return scenesToChooseFrom;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadScenesList();
        if(SceneManager.GetActiveScene().buildIndex == 0) CreateLevelsUI();
    }
    public void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromList) CreateListOfScenesFromList();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuildAndList) { CreateListOfScenesFromBuild(); CreateListOfScenesFromList(); }
        scenesToChooseFrom.Remove("MainMenu");
        scenesToChooseFrom.Remove("The_End");
    }
    private void CreateListOfScenesFromBuild()
    {
        string tempStr = "";
        GameObject g;
        sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            scenesToChooseFrom.Add(tempStr);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    private void CreateLevelsUI ()
    {
        string tempStr = "";
        GameObject g;
        sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (i != 0)
            {
                g = Instantiate(levelXPrefab);
                g.transform.parent = content.transform;
                levels.Add(g.GetComponent<LevelDetails>());
                levels.ElementAt(i - 1).SetName(tempStr);
            }

        }
    }
    private void CreateListOfScenesFromList()
    {
        foreach (string scene in scenes)
        {
            scenesToChooseFrom.Add(scene);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in scences list or choose ScenesFromBuild from " + gameObject);
    }

    public void ChangeScenesToChooseFrom(LevelDetails scene)
    {
        if (scene.GetToggle() && scenesToChooseFrom.Count > 0)
        {
            scenesToChooseFrom.Remove(scene.GetName());
            Debug.Log("I work " + scene.GetName());
        }
        else scenesToChooseFrom.Add(scene.GetName());
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
    public void Finish() 
    {
        SceneManager.LoadScene("The_End");
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