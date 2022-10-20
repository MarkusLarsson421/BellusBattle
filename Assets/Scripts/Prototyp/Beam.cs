using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject Gun;

    public GameObject Bullet;
    public GameObject Beamy;

    public float bulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    public Camera mainCamera;



    public float timeUntilBeam = 1.7f;

    public float ammo = 6;


    GameObject beamIns;

    // Update is called once per frame
    void Update()
    {
        if (timeUntilBeam <= 0)
        {
            beamIns = Instantiate(Beamy, ShootPoint.position, ShootPoint.rotation);
            beamIns.transform.Rotate(Vector3.left * 180);
            Destroy(beamIns, 0.5f);
            timeUntilBeam = 1.7f;
        }
    }
}
