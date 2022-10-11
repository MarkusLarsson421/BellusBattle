using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Prototype : MonoBehaviour
{
    [SerializeField] float timer = 3;
    float countDown;
    bool exploded;

    [SerializeField] GameObject explosionParticles;

    // Update is called once per frame
    private void Start()
    {
        countDown = timer;
    }
    void FixedUpdate()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0 && !exploded)
        {
            explode();
        }
    }
    void explode()
    {
        Instantiate(explosionParticles, transform.position, transform.rotation);
        exploded = true;
    }

}
