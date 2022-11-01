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
    [SerializeField] public Transform muzzle;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    //public float reloadTime;
    //[HideInInspector] public bool reloading;
}
