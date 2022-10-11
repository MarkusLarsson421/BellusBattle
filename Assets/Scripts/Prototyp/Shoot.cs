using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{


    public GameObject Gun;

    public GameObject Bullet;

    public float bulletSpeed;

    public Transform ShootPoint;

    public float fireRate;

    public Camera mainCamera;

    float fireAgain;

    public float ammo = 6;

    // Start is called before the first frame update

    public void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

        faceGun();

        if (ammo > 0)
        {
if (Input.GetMouseButtonDown(0))
        {
            
            if(Time.time > fireAgain)
            {
                fireAgain = Time.time + 1/fireRate;
                shoot();
            }
            
        }
        }
        
    }
     void faceGun()
    {
        float camToPlayerDist = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camToPlayerDist));
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Gun.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void eqiup(GameObject pickup)
    {
        Gun = pickup;
        
    }
    public void die()
    {
        Destroy(gameObject);
    }
    void shoot()
    {
        ammo = ammo -1;
        GameObject bulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        bulletIns.transform.Rotate(Vector3.left * 90);
        Rigidbody bulletIns_rig;
        bulletIns_rig = bulletIns.GetComponent<Rigidbody>();
        bulletIns_rig.AddForce(ShootPoint.transform.up * bulletSpeed);
        Destroy(bulletIns, 4f);
    }
}
