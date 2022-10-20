using System.Collections;
using UnityEngine;

public class Grenade : Projectile{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	[SerializeField] [Tooltip("Size of the explosion.")]
	private float explosionSize = 5.0f;


	private void Start(){
		StartCoroutine(StartFuse());
	}

	private IEnumerator StartFuse(){
		yield return new WaitForSeconds(fuse);
		Explode();
	}

	private void Explode(){
		Collider[] hits = Physics.OverlapSphere(transform.position, explosionSize);
		for (int i = 0; i < hits.Length; i++){
			if (hits[i].CompareTag("Player")){
				//TODO deal damage.
			}
		}
		Die();
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
