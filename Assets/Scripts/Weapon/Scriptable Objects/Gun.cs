using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    /**
     * Put on every gun
    **/
    [Header("References")]
    [SerializeField] private WeaponData weaponData;
    //[SerializeField] private Transform cam;
    
    [SerializeField] private WeaponManager weaponManager;

    private float timeSinceLastShot;

    [SerializeField]
    [Tooltip("The amount of force placed on the projectile.")]
    private float projectileForce = 10f;

    private Projectile _projectile;

    [SerializeField]
    [Tooltip("What projectile is being fired.")]
    private GameObject projectile;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        //PlayerShoot.reloadInput += StartReload;
        
        _projectile = projectile.GetComponent<Projectile>();
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (weaponData.currentAmmo <= 0)
        {
            // Destroy weapon
        }
        
        //Debug.DrawRay(muzzle.position, muzzle.forward * weaponData.maxDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        weaponManager = other.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.EquipWeapon(weaponData);
            
            Destroy(gameObject);
        }
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
    private bool CanShoot() => timeSinceLastShot > 1f / (weaponData.fireRate / 60f);//!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot()
    {
        if (weaponData.currentAmmo > 0)
        {
            weaponData.muzzle = GameObject.FindGameObjectWithTag("Muzzle").transform;
            GameObject firedProjectile = Instantiate(projectile, weaponData.muzzle.transform.position, transform.rotation);
            if (CanShoot())
            {
                Vector3 force = new Vector3(projectileForce * weaponManager.aim.transform.right.x, projectileForce * weaponManager.aim.transform.right.y, 0f);
                _projectile = firedProjectile.GetComponent<Projectile>();
                _projectile.GetComponent<Rigidbody>().AddForce(force);

                /*
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, weaponData.maxDistance))
                {
                    PlayerHealth damageable = hitInfo.transform.GetComponent<PlayerHealth>();
                    damageable?.TakeDamage(weaponData.damage);
                }
                */
                weaponData.currentAmmo--;
                timeSinceLastShot = 0;
                //OnGunShot();
            }
        }
    }

    
}
