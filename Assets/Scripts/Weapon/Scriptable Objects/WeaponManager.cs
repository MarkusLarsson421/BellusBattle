using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponSlot;

    //[SerializeField]
    //AudioSource pickupSound;

    [SerializeField]
    private WeaponData equippedWeapon;

    private GameObject currentWeapon;

    [SerializeField]
    public Aim aim; // test to make bullet shoot in correct direction

    private void Start()
    {
        aim = gameObject.GetComponentInChildren<Aim>();
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        //pickupSound.Play();
        equippedWeapon = weaponData;
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(weaponData.weaponPrefab);
        currentWeapon.transform.SetParent(weaponSlot);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }
}
