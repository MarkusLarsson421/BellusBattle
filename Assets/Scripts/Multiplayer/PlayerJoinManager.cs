using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : PlayerSpawnManager
{
    [SerializeField] private Camera camera;
    //[SerializeField] public Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    //[SerializeField] public ScoreManager scoreManager;

    [SerializeField] GameObject characterLow;
    [SerializeField] GameObject[] accessorites;
    [SerializeField] GameObject accessoritesSlot;

    private List<int> inputDevices = new List<int>();
    public List<PlayerInput> listOfPlayers = new List<PlayerInput>();
    PlayerSpawnManager psm;

    public Material[] colors;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        //Debug.Log(playerInput.GetComponent<InputDevice>().deviceId);
        /*
        if (inputDevices.Contains(playerInput.GetComponent<InputDevice>().deviceId))
        {
            Destroy(playerInput.gameObject);
        }
        */
        // Set the player ID, add one to the index to start at Player 1
        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;
        //
        scoreManager.AddPlayers(playerInput.gameObject);
        //inputDevices.Add(playerInput.GetComponent<InputDevice>().deviceId);
        listOfPlayers.Add(playerInput);
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);


        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].position;
        //playerInput.gameObject.GetComponent<PlayerDetails>().startPos = LevelSpawnsDic[1][playerInput.playerIndex - 1].position;

        playerInput.gameObject.GetComponent<CameraTest>().focus = camera.gameObject.GetComponent<CameraFocus>();
        AddPlayerInFocus(playerInput.transform);

        // Change color depending on index
        // Kanske ej behövs i framtiden då alla karaktärer är unika /Martin Nyman
        //playerInput.gameObject.GetComponent<MeshRenderer>().material = colors[playerInput.playerIndex];

        // Changes the texture/material of the player
        playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = colors[playerInput.playerIndex];
        // Finds where the accessory should be placed (HeadSlot)
        //accessoritesSlot = GameObject.FindGameObjectWithTag("HeadSlot");
        // Put the accessory on the HeadSlot
        //Instantiate(accessorites[playerInput.playerIndex], accessoritesSlot.transform);
    }
    private void AddPlayerInFocus(Transform player)
    {
        camera.gameObject.GetComponent<CameraFocus>()._targets.Add(player);
    }
}
