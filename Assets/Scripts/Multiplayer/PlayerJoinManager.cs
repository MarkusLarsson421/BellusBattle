using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : PlayerSpawnManager
{
    [SerializeField] private Camera camera;
    [SerializeField] GameObject characterLow;
    [SerializeField] GameObject[] accessorites;
    //[SerializeField] GameObject accessoritesSlot;

    public List<PlayerInput> listOfPlayers = new List<PlayerInput>();
    public Material[] colors;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        // Set the player ID, add one to the index to start at Player 1
        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;
        //
        scoreManager.AddPlayers(playerInput.gameObject);
        listOfPlayers.Add(playerInput);
        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);


        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = SpawnLocations[playerInput.playerIndex].position;
        //playerInput.gameObject.GetComponent<PlayerDetails>().startPos = LevelSpawnsDic[1][playerInput.playerIndex - 1].position;

        playerInput.gameObject.GetComponent<CameraTest>().focus = camera.gameObject.GetComponent<CameraFocus>();
        AddPlayerInFocus(playerInput.transform);

        // Changes the texture/material of the player
        playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material = colors[playerInput.playerIndex];
        // Finds where the accessory should be placed (HeadSlot)
        //accessoritesSlot = GameObject.FindGameObjectWithTag("HeadSlot");
        PlayerDetails playerDetails = playerInput.gameObject.GetComponent<PlayerDetails>();
        // Put the accessory on the HeadSlot
        GameObject accessory = Instantiate(accessorites[playerInput.playerIndex], playerDetails.HeadGearSlot());//accessoritesSlot.transform);
        accessory.transform.SetParent(playerInput.gameObject.transform);
    }
    private void AddPlayerInFocus(Transform player)
    {
        camera.gameObject.GetComponent<CameraFocus>()._targets.Add(player);
    }
}
