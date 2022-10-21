using UnityEngine;
using UnityEngine.InputSystem;

public class FirearmTest : MonoBehaviour
{
	[SerializeField] [Tooltip("")]
	private GameObject grenade;
	[SerializeField] [Tooltip("")] 
	private GameObject rifle;
	
	[SerializeField]
	private GameObject _currentWeapon;
	private Weapon _weapon;
	private bool _hasFired; // Används för att kunna använda med det nya Input Systemet

	private void Update()
	{
		UserInput();
	}

	public void PickUpWeapon(GameObject weapon)
	{
		if (_currentWeapon == null)
		{
			_weapon = weapon.GetComponent<Weapon>();
			weapon.transform.parent = gameObject.transform;
		}
	}

	public void DropWeapon()
	{
		_weapon.Drop(new Vector3(10, 2, 0));
		_weapon = null;
		_currentWeapon = null;
	}

	private void UserInput()
	{
		if (_currentWeapon != null && (_hasFired || Input.GetKey(KeyCode.Mouse1)))
		{
			_weapon.Fire();
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			rifle.transform.position = transform.position;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			grenade.transform.position = transform.position;
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			DropWeapon();
		}
	}

	void OnFire(InputAction.CallbackContext ctx)
    {
		if (ctx.started)
		{
			_hasFired = true;
		}
	}
}
