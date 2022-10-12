using UnityEngine;

public class Projectile : MonoBehaviour{
	[SerializeField] [Tooltip("Whether the projectile will kill on impact or not.")]
	private bool lethal = true;
	[SerializeField] [Tooltip("")]
	private float lifeSpan = 5.0f;
	
	private Vector3 _velocity;

	private void Start(){
		GetComponent<SphereCollider>().enabled = lethal;
	}

	private void Update(){
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0){Destroy(gameObject);}
	}

	public void Fire(){
		
	}
}
