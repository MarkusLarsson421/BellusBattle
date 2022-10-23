using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{


    public GameObject Beamy;

    public float bulletSpeed;

    Transform grenadePoint;

    public float fireRate;

    public Camera mainCamera;



    public float timeUntilBeam = 1.7f;

    public float ammo = 6;


    GameObject beamIns;

    private void Start()
    {
        grenadePoint = this.transform;
    }
    
    void Update()
    {
        timeUntilBeam -= Time.deltaTime;
        if (timeUntilBeam <= 0)
        {
            beamIns = Instantiate(Beamy, this.transform.position, this.transform.rotation);
            //beamIns.transform.Rotate(Vector3.left * 180);

            Destroy(beamIns, 0.5f);
            timeUntilBeam = 1.7f;
        }
        if (beamIns != null)
        {
            beamIns.transform.position = this.transform.position;
            beamIns.transform.rotation = grenadePoint.rotation;
        }

    }
}
