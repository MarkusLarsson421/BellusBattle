using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp_ProtoV1 : MonoBehaviour
{

    Collider[] hitColliders;
    [SerializeField] private Transform weaponPosition;
    private Weapon currentWeapon;
    [SerializeField] GameObject revolver;
    [SerializeField] GameObject Grenade;

    private void PickUpWeapon(Vector3 center, float radius)
    {
        hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Revolver"))
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);

                revolver.GetComponent<MeshRenderer>().enabled = true;
                revolver.GetComponent<Weapon>().enabled = true;
                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                return;
            }
            if (col.CompareTag("Grenade"))
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);

                Grenade.GetComponent<MeshRenderer>().enabled = true;
                Grenade.GetComponent<Grenade>().enabled = true;
                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                return;
            }
        }
    }

    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PickUpWeapon(transform.position, 2f);
        }
    }
}
