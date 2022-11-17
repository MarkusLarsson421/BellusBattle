using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponSlot;

    [SerializeField]
    private WeaponData equippedWeapon;

    private GameObject currentWeapon;

    public WeaponData EquippedWeapon { get => equippedWeapon; }

    private void Start()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (weaponSlot.childCount > 0)
        {
            currentWeapon = weaponSlot.GetChild(0).gameObject;
            UnEquipWeapon(currentWeapon);
            currentWeapon.SetActive(false);
            currentWeapon.transform.SetParent(null);
        }
        
    }

    public bool pickingUP;
    public void OnPickup(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            pickingUP = true;
        }
    }

    public void EquipWeapon(WeaponData weaponData, GameObject nowWeapon)
    {
        if (equippedWeapon != null)
        {
            return;
        }
        equippedWeapon = weaponData;

        if (equippedWeapon.pickupSound != null)
        {
            equippedWeapon.pickupSound.Play();
        }

        nowWeapon.transform.SetParent(weaponSlot);
        nowWeapon.transform.localPosition = Vector3.zero;
        nowWeapon.transform.localRotation = Quaternion.identity;
        nowWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    public void UnEquipWeapon(GameObject nowWeapon)
    {
        //nowWeapon.GetComponent<Gun>().Drop();
        equippedWeapon = null;
        nowWeapon = null;
    }

}
