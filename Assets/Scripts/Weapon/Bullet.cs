using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
	[SerializeField] 
	public PlayerHealth ph;
    
	[SerializeField] [Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;

	private void Start(){
		StartCoroutine(Shoot(lifeSpan));
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("hitititititi");
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("hit");
			ph = other.gameObject.GetComponent<PlayerHealth>();
			ph.TakeDamage(1);
			Debug.Log(ph.health);

			//other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
			//other.gameObject.transform.position = new Vector3(999999f, 99999f, 999f);
			//other.gameObject.GetComponent<PlayerInput>().gameObject.SetActive(false);
			Debug.Log(ph.health);
			
			Die();
		}
	}

	private IEnumerator Shoot(float seconds){
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die(){
		Destroy(gameObject);
	}
}