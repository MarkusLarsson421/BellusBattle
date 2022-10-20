using UnityEngine;
using Random = System.Random;

public class Weapon : MonoBehaviour
{
    [SerializeField] [Tooltip("How accurate the firearm is.")] [Range(0, 1)]
    private float inaccuracy = 1.0f;
    [SerializeField] [Tooltip("Rounds per second.")]
    private float fireRate = 5.0f;
    [SerializeField] [Tooltip("Ammunition of the firearm.")]
    private int ammo = 6;
    [SerializeField] [Tooltip("What projectile is being fired.")] 
    private GameObject projectile;
    [SerializeField] [Tooltip("Where the projectile is fired from.")] 
    private GameObject projectileOrigin;
    [SerializeField] [Tooltip("The amount of force placed on the projectile.")]
    private float projectileForce = 100.0f;

    private float _nextTimeToFire;
    private bool _isFiring;
    private ParticleSystem _muzzleFlash;
    private Projectile _projectile;
    private readonly Random _random = new();

    [SerializeField] public Aim aim; // test to make bullet shoot in correct direction

    private void Start(){
        _muzzleFlash = projectileOrigin.GetComponent<ParticleSystem>();
        _projectile = projectile.GetComponent<Projectile>();
    }

    public void Fire(){
        if (ammo <= 0) {Destroy(gameObject);}
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1.0f / fireRate;
            ammo--;
            if (_muzzleFlash != null){_muzzleFlash.Play();}
            Debug.Log(aim.rotation.z);
            GameObject firedProjectile = Instantiate(projectile, projectileOrigin.transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));

            // Original
            //GameObject firedProjectile = Instantiate(projectile, projectileOrigin.transform.position, Quaternion.identity);
            //Calculation is inefficient, could possibly be improved to simulate inaccuracy better.
            //Vector3 force = new Vector3(projectileForce, (float)_random.Next(0, (int)inaccuracy * 100) / 100, 0);

            Vector3 force = new Vector3(projectileForce * aim.transform.right.x, projectileForce * aim.transform.right.y, projectileForce * aim.transform.right.z);
            //Vector3 force = new Vector3(projectileForce * aim.transform.rotation.x, projectileForce * aim.transform.rotation.y, projectileForce * aim.transform.rotation.z);
            _projectile = firedProjectile.GetComponent<Projectile>();
            _projectile.Fire(force); 
        }
    }
}
