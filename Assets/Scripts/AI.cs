using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float checkDistance = 1;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
    }
    private void FixedUpdate()
    {
        velocity = direction * speed * Time.deltaTime;
        transform.position += velocity;
    }
    private void CheckCollision()
    {
        if (Physics.BoxCast(transform.position, transform.localScale, direction, Quaternion.identity, checkDistance))
        {
            if (direction == Vector3.right) direction = Vector2.left;
            else direction = Vector2.right;
        }
    }
    public void KillAI()
    {
        Destroy(gameObject);
    }
}
