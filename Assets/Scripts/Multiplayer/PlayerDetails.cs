using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int playerID; // Stores the players ID
    public Vector3 startPos; // Stores the start spawn position
   
    void Start()
    {
        transform.position = startPos;
    }
}
