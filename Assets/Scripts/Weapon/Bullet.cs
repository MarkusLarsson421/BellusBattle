using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
	[SerializeField] [Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;
	CameraFocus cf;

	private void Start(){
		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
		StartCoroutine(Shoot(lifeSpan));
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player"))
		{
			//playerGo.SetActive(false);
			playerGo.GetComponent<PlayerHealth>().KillPlayer();
			cf.RemoveTarget(playerGo.transform);
			playerGo.GetComponent<PlayerHealth>().TakeDamage(1);
			
			PlayerDeathEvent pde = new PlayerDeathEvent{
				PlayerGo = other.gameObject,
				Kille = other.name,
				KilledBy = "No Idea-chan",
				KilledWith = "Bullets",
			};
			pde.FireEvent();

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