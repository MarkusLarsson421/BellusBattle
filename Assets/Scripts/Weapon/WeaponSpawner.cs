using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField]private Weapon[] weapons;
    private bool isholdingWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnRandomWeapon()
    {
        //if (isholdingWeapon) return;
        Instantiate(weapons[Random.Range(0, weapons.Length)], transform);
        isholdingWeapon = true;
    }
    public void WeaponIsPicked() // Den här ska försättas med PickUp Script
    {
        isholdingWeapon = false;
    }
    public bool GetIsHoldingWeapon()
    {
        return isholdingWeapon;
    }
}
