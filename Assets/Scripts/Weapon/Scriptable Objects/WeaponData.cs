using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    //[SerializeField] public Transform muzzle;
    [SerializeField] public GameObject projectile;
    [SerializeField, Tooltip("The amount of force placed on the projectile.")]
    public float projectileForce;

    [Header("Reloading")]
    public int currentAmmo, initialAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    //public float reloadTime;
    //[HideInInspector] public bool reloading;

    [Header("Sounds")]
    [SerializeField, Tooltip("Sound made when picking up weapon")]
    public AudioSource pickupSound;
    [SerializeField, Tooltip("Sound made when using weapon")]
    public AudioSource shootAttackSound;
    

    [Header("VFX")]
    private int placeholder;

    // Getters
    public int Ammo { get => currentAmmo; }


    // Metods
    public void ChangeAmmoBy(int changeBy)
    {
        currentAmmo += changeBy;
    }

    public void SetNewAmmoAmount(int newAmount)
    {
        currentAmmo = newAmount;
    }

    public void ResetAmmo()
    {
        currentAmmo = initialAmmo;
    }
}
