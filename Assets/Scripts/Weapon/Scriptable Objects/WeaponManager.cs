using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponSlot;

    [SerializeField]
    private WeaponData equippedWeapon;

    private GameObject currentWeapon;

    [SerializeField]
    public Aim aim; // test to make bullet shoot in correct direction

    private void Start()
    {
        aim = gameObject.GetComponentInChildren<Aim>();
    }
    private void Update()
    {

    }

    public void EquipWeapon(WeaponData weaponData, GameObject nowWeapon)
    {
        equippedWeapon = weaponData;

        if (equippedWeapon.pickupSound != null)
        {
            equippedWeapon.pickupSound.Play();
        }

        /*
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        */

        //currentWeapon = Instantiate(weaponData.weaponPrefab);
        //currentWeapon.transform.SetParent(weaponSlot);
        nowWeapon.transform.SetParent(weaponSlot);
        nowWeapon.transform.localPosition = Vector3.zero;
        nowWeapon.transform.localRotation = Quaternion.identity;
        // When you pickup a weaon you want the ammo to be right (basically reload on pickup)
        weaponData.ResetAmmo();
        //weaponData.currentAmmo = weaponData.magSize;
        //currentWeapon.GetComponent<BoxCollider>().enabled = false;
        nowWeapon.GetComponent<BoxCollider>().enabled = false;
    }
}
