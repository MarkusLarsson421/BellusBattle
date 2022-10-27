using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grenade : Projectile
{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	[SerializeField] [Tooltip("Size of the explosion.")]
	private float explosionSize = 5.0f;

	[SerializeField] PickUp_ProtoV1 pickUp_Proto;

	private void Start(){
		StartCoroutine(StartFuse());
	}

	private IEnumerator StartFuse(){
		yield return new WaitForSeconds(fuse);
		Explode();
	}

	public void OnPickUp(InputAction.CallbackContext ctx)
	{
		if (ctx.started)
		{
			StartFuse();
		}
	}

	private void Explode(){

		Collider[] hits = Physics.OverlapSphere(transform.position, explosionSize);
		for (int i = 0; i < hits.Length; i++){
			if (hits[i].CompareTag("Player"))
			{
				PlayerHealth ph = hits[i].GetComponent<PlayerHealth>();
				ph.TakeDamage(1);
				pickUp_Proto.isHoldingWeapon = false;

				/*
				PlayerDeathEvent pde = new PlayerDeathEvent{
					PlayerGo = hits[i].gameObject,
					Kille = hits[i].name,
					KilledBy = "No Idea-chan",
					KilledWith = "Bullets",
				};
				pde.FireEvent();
				*/
			}
		}
		Destroy(gameObject);
		//Die();
	}

	private void Die(){
		ExplodeEvent ee = new ExplodeEvent{
			Description = "Grenade " + name + " exploded!",
			ExplosionGo = gameObject
		};
		ee.FireEvent();
		
		Destroy(gameObject);
	}
}
