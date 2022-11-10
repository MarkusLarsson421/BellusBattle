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
    [SerializeField] private List<LevelDetails> levels = new();
    [SerializeField] private float timeTillRestartGame;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject levelXPrefab;
    
    private static GameObject _singleTon;
    
    public List<string> scenesToChooseFrom = new();
    public List<string> scenesToRemove = new();

    private void Awake()
    {
        if (_singleTon == null){_singleTon = gameObject;}
        else{Die();}
        
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenesToRemove.Add("MainMenu");
        scenesToRemove.Add("The_End");
        LoadScenesList();
        if(SceneManager.GetActiveScene().buildIndex == 0) CreateLevelsUI();
    }
    public void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromList) CreateListOfScenesFromList();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuildAndList) { CreateListOfScenesFromBuild(); CreateListOfScenesFromList(); }
        foreach(string scene in scenesToRemove)
        {
            scenesToChooseFrom.Remove(scene);
        }
    }
    // ahhaa
    private void CreateListOfScenesFromBuild()
    {
        for (int i = 0; i < sceneCount; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            scenesToChooseFrom.Add(tempStr);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    private void CreateLevelsUI ()
    {
        for (int i = 0; i < sceneCount-1; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (i != 0)
            {
                GameObject g = Instantiate(levelXPrefab, content.transform, true);
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
            scenesToRemove.Add(scene.GetName());
        }
        else 
        {
            scenesToChooseFrom.Add(scene.GetName());
            scenesToRemove.Remove(scene.GetName());
        }
    }

    public void LoadNextScene()
    {
        if (scenesToChooseFrom.Count <= 0)
        {
            Application.OpenURL("https://www.youtube.com/watch?v=WEEM2Qc9sUg");
            return;
        }
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
    public void Finish(GameObject destroyMe) 
    {
        SceneManager.LoadScene("The_End");
        StartCoroutine(RestartGame(destroyMe));
    }
    private IEnumerator RestartGame(GameObject destroyMe)
    {
        yield return new WaitForSeconds(timeTillRestartGame);
        Destroy(destroyMe);
        Destroy(gameObject);
        SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}