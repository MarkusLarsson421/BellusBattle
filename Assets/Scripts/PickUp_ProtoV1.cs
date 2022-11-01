using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp_ProtoV1 : MonoBehaviour
{
	[SerializeField] 
	private GameObject revolver;
	[SerializeField] 
	private GameObject grenade;
	[SerializeField] 
	private GameObject sword;
	[SerializeField] 
	private Transform weaponPosition;
	
	private Collider[] _hitColliders;
	private Weapon _currentWeapon;
	private bool _isHoldingWeapon;

	public void DropWeapon()
	{
		_isHoldingWeapon = false;
	}
	

    private void PickUpWeapon(Vector3 center, float radius)
    {
        _hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider col in _hitColliders)
        {
            if (col.CompareTag("Revolver") && !_isHoldingWeapon)
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);

                revolver.GetComponent<MeshRenderer>().enabled = true;
                revolver.GetComponent<Weapon>().enabled = true;
                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                sword.GetComponentInChildren<MeshRenderer>().enabled = false;
                sword.GetComponent<Sword_Prototype>().enabled = false;
                sword.GetComponentInChildren<DropWeapon_Porotype>().enabled = false;

                _isHoldingWeapon = true;

                return;
            }
            if (col.CompareTag("Grenade") && !_isHoldingWeapon)
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);
                Debug.Log("1");

                grenade.GetComponent<MeshRenderer>().enabled = true;
                grenade.GetComponent<Weapon>().enabled = true;
                Debug.Log("2");
                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                sword.GetComponentInChildren<MeshRenderer>().enabled = false;
                sword.GetComponent<Sword_Prototype>().enabled = false;
                sword.GetComponentInChildren<DropWeapon_Porotype>().enabled = false;
                Debug.Log("3");

                _isHoldingWeapon = true;
                
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
