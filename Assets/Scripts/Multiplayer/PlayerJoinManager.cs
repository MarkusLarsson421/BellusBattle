using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] protected Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] public List<PlayerInput> listOfPlayers = new List<PlayerInput>();

    public Material[] colors;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // Set the player ID, add one to the index to start at Player 1
        playerInput.gameObject.GetComponent<PlayerDetails>().playerID = playerInput.playerIndex + 1;
        listOfPlayers.Add(playerInput);

        Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[playerInput.playerIndex].position;
        //playerInput.gameObject.GetComponent<PlayerDetails>().startPos = LevelSpawnsDic[1][playerInput.playerIndex - 1].position;

        playerInput.gameObject.GetComponent<CameraTest>().focus = camera.gameObject.GetComponent<CameraFocus>();
        AddPlayerInFocus(playerInput.transform);

        // Change color depending on index
        playerInput.gameObject.GetComponent<MeshRenderer>().material = colors[playerInput.playerIndex];
    }
    private void AddPlayerInFocus(Transform player)
    {
        camera.gameObject.GetComponent<CameraFocus>()._targets.Add(player);
    }
}
