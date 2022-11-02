using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject anchor;
    [SerializeField] private int playerLayer;
    [SerializeField] private Collider[] colliders; 
    private BoxCollider boxCollider;
    private GameObject currentPlayer;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }


    private void FixedUpdate()
    {
        CheckForPlayers();
    }

    

   private void CheckForPlayers()
    {
        colliders = Physics.OverlapBox(transform.position, transform.localScale * 2, Quaternion.identity);
        foreach(Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                currentPlayer = col.gameObject;
                if(col.bounds.center.x > boxCollider.bounds.center.x)
                {
                    RotateAnchor(90f);
                }
                else if(col.bounds.center.x < boxCollider.bounds.center.x)
                {
                    RotateAnchor(-90f);
                }
                return;
            }
            else
            {
                anchor.transform.localEulerAngles = Vector3.zero;
            }
        }
        
    }

    private void RotateAnchor(float rotationAmount)
    {
        if ((anchor.transform.localEulerAngles.y == 90f && rotationAmount > 0f) || (anchor.transform.localEulerAngles.y == -90f && rotationAmount < 0f)) return;
        anchor.transform.Rotate(new Vector3(0f,rotationAmount, 0f));
    }
}
