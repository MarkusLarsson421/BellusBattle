using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    /**
     * Put on every gun
    **/
    [Header("References")]
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private WeaponManager weaponManager;

    [Tooltip("What projectile is being fired.")]
    private GameObject projectile;

    private Projectile _projectile;
    [SerializeField] private Transform muzzle;
    [Tooltip("Used for FireRate")]
    private float timeSinceLastShot;

    //int ammoNow;

    [SerializeField] private PlayerMovement player;
    [SerializeField] public Aim aim; // test to make bullet shoot in correct direction

    [SerializeField] bool isPickedUp;

    private float _nextTimeToFire;

    private void Start()
    {
        // Get the ammo the weapon has
        //ammoNow = weaponData.Ammo;

        // Reload it
        weaponData.SetNewAmmoAmount(weaponData.magSize);

        
        //PlayerShoot.reloadInput += StartReload;
        muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;

        projectile = weaponData.projectile;
        _projectile = projectile.GetComponent<Projectile>();

    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Time.deltaTime >= _nextTimeToFire)
        {
            _nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);
        }

        /*
        if (weaponData.Ammo <= 0)
        {
            // Destroy weapon
            //Destroy(this.gameObject);
        }
        */
        if (muzzle == null)
        {
            muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
        }

        //aim = player.GetComponentInChildren<Aim>();

        //Debug.DrawRay(muzzle.position, muzzle.forward * weaponData.maxDistance);
    }

    //float aimingy, aimingx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerShoot = other.gameObject.GetComponent<PlayerShoot>();
            //player = gameObject.GetComponent<PlayerMovement>();
            //aim = player.GetComponentInChildren<Aim>();
            weaponManager = other.gameObject.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                playerShoot.shootInput += Shoot;
                //aimingx = weaponManager.aim.transform.right.x;
                //aimingy = weaponManager.aim.transform.right.y;
                weaponManager.EquipWeapon(weaponData, gameObject);
                isPickedUp = true;

                

            }
            //Destroy(gameObject);
            //Destroy(muzzle.gameObject);

        }

    }
    private void OnTriggerExit(Collider other)
    {
        isPickedUp = false;
    }


    //private void OnDisable() => gunData.reloading = false;

    /*
    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }
    */
    private bool CanShoot() => timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && weaponData.Ammo > 0;//!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);


    private void Shoot()
    {
        if (CanShoot() && isPickedUp)
        {
            //_nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);


            //weaponData.ChangeAmmoBy(ammoNow--);
            weaponData.currentAmmo--;
            Debug.Log(weaponData.currentAmmo);
            //Muzzleflash
            //Sound

            GameObject firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

            float forceForwrd = weaponData.projectileForce;
            float aimx = muzzle.transform.forward.x;
            float aimy = muzzle.transform.forward.y;
            Vector3 force = new Vector3(forceForwrd * aimx, forceForwrd * aimy, 0f);
            _projectile = firedProjectile.GetComponent<Projectile>();
            _projectile.GetComponent<Rigidbody>().AddForce(force);

            /*
            if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, weaponData.maxDistance))
            {
                PlayerHealth damageable = hitInfo.transform.GetComponent<PlayerHealth>();
                damageable?.TakeDamage(weaponData.damage);
            }
            */

            timeSinceLastShot = 0;
            //OnGunShot();
        }

    }


}
