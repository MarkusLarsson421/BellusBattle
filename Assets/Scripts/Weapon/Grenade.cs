using System.Collections;
using UnityEditor;
using UnityEngine;

public class Grenade : MonoBehaviour{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	[SerializeField] [Tooltip("Size of the explosion.")]
	private float explosionSize = 5.0f;
	
	private ParticleSystem _explosion;
	
	private void Start(){
		_explosion = GetComponent<ParticleSystem>();
	}

	private void Update(){
		fuse -= Time.deltaTime;
		if (fuse <= 0){StartCoroutine(Explode(_explosion.time, explosionSize));}
	}

	private IEnumerator Explode(float seconds, float explosionSize){
		_explosion.Play();
		Collider[] hits = Physics.OverlapSphere(transform.position, explosionSize);
		for (int i = 0; i < hits.Length; i++){
			if (hits[i].CompareTag("Player")){
				//TODO deal damage.
			}
		}
		yield return new WaitForSeconds(seconds);
		
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
