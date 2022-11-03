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

    int gunsAmmo;

    private float _nextTimeToFire;

    private void Start()
    {
        // Get the ammo the weapon has
        //ammoNow = weaponData.Ammo;

        // Reload it
        weaponData.SetNewAmmoAmount(weaponData.magSize);

        playerShoot.dropInput += Drop;
        //PlayerShoot.reloadInput += StartReload;
        //muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;

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

        // Placeholder for future
        if (gunsAmmo >= 0) // && Time runs out
        {
            // Delete this gun
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerShoot = other.gameObject.GetComponent<PlayerShoot>();
            weaponManager = other.gameObject.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                playerShoot.shootInput += Shoot;
                weaponManager.EquipWeapon(weaponData, gameObject);
                isPickedUp = true;

                gunsAmmo = weaponData.Ammo;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPickedUp = false;
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && gunsAmmo > 0 && isPickedUp;//!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f); //weaponData.Ammo > 0

    private void Shoot()
    {
        if (CanShoot())
        {
            //weaponData.ChangeAmmoBy(ammoNow--);
            //weaponData.currentAmmo--;
            //Debug.Log(weaponData.currentAmmo);

            gunsAmmo--;
            Debug.Log(gunsAmmo);
            //Muzzleflash
            //Sound
            //Animation

            GameObject firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

            float forceForwrd = weaponData.projectileForce;
            float aimx = muzzle.transform.forward.x;
            float aimy = muzzle.transform.forward.y;
            Vector3 force = new Vector3(forceForwrd * aimx, forceForwrd * aimy, 0f);
            _projectile = firedProjectile.GetComponent<Projectile>();
            _projectile.GetComponent<Rigidbody>().AddForce(force);

            timeSinceLastShot = 0;
        }
    }

    private void Drop()
    {

    }
}
