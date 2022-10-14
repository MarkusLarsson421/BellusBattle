using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	
	private ParticleSystem _explosion;
	private SphereCollider _collider;
	
	private void Start(){
		_explosion = GetComponent<ParticleSystem>();
		_collider = GetComponent<SphereCollider>();
		_collider.enabled = false;
	}

	private void Update(){	
		fuse -= Time.deltaTime;
		if (fuse <= 0){StartCoroutine(Explode(_explosion.time));}
	}

	private IEnumerator Explode(float seconds){
		_collider.enabled = true;
		_explosion.Play();
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
