using System.Collections;
using UnityEngine;
using Random = System.Random;

public class Bullet : Projectile
{
	[SerializeField] [Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;
	CameraFocus cf;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	public AudioSource[] hitSounds;

	public float bulletDamage;

	private void Start(){
		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
		StartCoroutine(Shoot(lifeSpan));
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player") && Shooter != playerGo)
		{
			playerGo.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);

            if (playerGo.GetComponent<PlayerHealth>().Health <= 0)
            {
				//playerGo.SetActive(false);
				playerGo.GetComponent<PlayerHealth>().KillPlayer();
				cf.RemoveTarget(playerGo.transform);
			}
			
			PlayerDeathEvent pde = new PlayerDeathEvent{
				PlayerGo = other.gameObject,
				Kille = other.name,
				KilledBy = "No Idea-chan",
				KilledWith = "Bullets",
			};
			pde.FireEvent();

			Die();
		}
		if(other.CompareTag( "target"))
        {
			GetComponent<Destroy>().gone();

		}

		if (other.gameObject.tag == "Obstacle")
		{
			Debug.Log("Obstacle");
			Destroy(gameObject);
			return;
		}

        if (other.gameObject.CompareTag("Breakable"))
        {
			Destroy(other.gameObject);
			Destroy(gameObject);
        }

		if (hitSounds.Length > 0)
		{
			hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)].Play();
		}

	}

	public void SetDamage(float setTo)
    {
		bulletDamage = setTo;
    }

	private IEnumerator Shoot(float seconds){
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die(){
		Destroy(gameObject);
	}
}