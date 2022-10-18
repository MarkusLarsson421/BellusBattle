using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public int playerAmountOnTeleporter = 0; // Amount of players on the Teleporter
    [SerializeField] PlayerJoinManager playerJoinManager; // Keeps track of players in game
    [SerializeField] string startSceneName; // The name of the scene that is the beginner scene

    private string[] scenes;
    private int sceneCount;

    private void Start()
    {
        CreateListOfScenes();
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a player stands on the Teleporter the playerAmountOnTeleporter goes up
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter++;
        }

        // There needs to be at least two players in the scene
        // All players in game needs to be in the Teleporter for the game to start
        if (playerJoinManager.listOfPlayers.Count >= 1)//playerSpawnManager.listOfPlayers.Count >= 2 && playerAmountOnTeleporter == playerSpawnManager.listOfPlayers.Count)
        {
            LoadRandomScene();
            //SceneManager.LoadScene(startSceneName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When a player gets off the Teleporter the playerAmountOnTeleporter goes down
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter--;
        }
    }

    // author Khaled
    private void CreateListOfScenes()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount - 1; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            //Debug.Log(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }

    }

    public void LoadRandomScene()
    {
        int index = Random.Range(1, sceneCount);
        SceneManager.LoadScene(index);
        Debug.Log("Scene Loaded: " + index);
    }
}
