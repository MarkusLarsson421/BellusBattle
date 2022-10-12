using UnityEngine;

public class Hitscan : MonoBehaviour {
	[SerializeField] 
	private int damage = 20;
	[SerializeField] 
	private float range = 100.0f;
	[SerializeField] [Tooltip("Rounds per second.")]
	private float fireRate = 5.0f;
	[SerializeField] 
	private Camera fpsCamera;
	[SerializeField] [Tooltip("The total amount of ammo this has.")]
	private int ammo;

	private float _nextTimeToFire;
	private bool _isFiring;
	private ParticleSystem _muzzleFlash;
	
	void Start()
	{
		ParticleSystem particleSys = transform.GetChild(0).GetComponent<ParticleSystem>();
		if (particleSys == null)
		{
			Debug.LogWarning("Gun requires a particle system as a muzzle flash to be attached to the gun as a child!");
		}
		else
		{
			_muzzleFlash = particleSys;
		}
	}
	
	public void OnFire()
    {
        if (Time.time >= _nextTimeToFire && ammo > 0)
        {
	        _nextTimeToFire = Time.time + 1.0f / fireRate;
			Fire();
        }
    }

	private void Fire(){
		ammo--;
		_muzzleFlash.Play();

		RaycastHit hit;
		//TODO Change raycast from FPS camera to raycast from the player center.
		if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range)){
			/*EnemyAI target = hit.collider.GetComponent<EnemyAI>();
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red, 2);
			if(target != null){
				target.TakeDamage(damage);
			}*/
		}
	}
}