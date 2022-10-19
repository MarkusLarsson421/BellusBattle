using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Flip : MonoBehaviour
{
    [SerializeField] private GameObject g;
    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.velocity.x > 0) g.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        else if(movement.velocity.x < 0) g.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
    }
}
