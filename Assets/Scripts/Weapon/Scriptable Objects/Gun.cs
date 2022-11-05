using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    /**
     * Put on every weapon
    **/
    [Header("References")]
    [SerializeField] private WeaponData weaponData; // The data of the weapon
    [SerializeField] private PlayerShoot playerShoot; // Actions
    [SerializeField] private int ownerID; // Player ID
    [SerializeField] private WeaponManager weaponManager;

    public int OwnerID { get => ownerID; }

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

    bool isStartTimer;
    [Tooltip("time before the weapon can be picked up again")]
    [SerializeField] float timeToWait = 2f;
    float timer;

    private void Start()
    {
        // Reload it
        weaponData.SetNewAmmoAmount(weaponData.magSize);

        projectile = weaponData.projectile;
        if (weaponData.projectile != null)
        {
            _projectile = projectile.GetComponent<Projectile>();
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        if (Time.deltaTime >= _nextTimeToFire)
        {
            _nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);
        }

        // Placeholder for future destory timer
        if (gunsAmmo >= 0) // && Time runs out
        {
            // Delete this gun
        }

        // USED FOR DROP
        if (!isStartTimer)
        {
            return;
        }
        timer += Time.unscaledDeltaTime;
        if (timer >= timeToWait)
        {
            timer = 0;
            isStartTimer = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerShoot = other.gameObject.GetComponent<PlayerShoot>();

            // Check who the owner of the weapon is 
            ownerID = other.gameObject.GetComponent<PlayerDetails>().playerID;

            weaponManager = other.gameObject.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                playerShoot.shootInput += Shoot;
                playerShoot.dropInput += Drop;
                weaponManager.EquipWeapon(weaponData, gameObject);
                isPickedUp = true;

                gunsAmmo = weaponData.Ammo;
            }
        }
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
            //Sound
            if (weaponData.shootAttackSound != null)
            {
                weaponData.shootAttackSound.Play();
            }
            
            //VFX
            
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

    public void Drop()
    {
        isPickedUp = false;
        weaponManager.UnEquipWeapon(gameObject);
        gameObject.transform.SetParent(null);
        isStartTimer = true;
    }
}
