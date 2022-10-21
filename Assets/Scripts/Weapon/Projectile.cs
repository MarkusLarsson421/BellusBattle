using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour{
	[SerializeField] [Tooltip("Whether the projectile will kill on impact or not.")]
	private bool lethal = true;
	[SerializeField] [Tooltip("")]
	private float lifeSpan = 5.0f;
	[SerializeField] [Tooltip("")]
	private float gravity = 9.82f;
	
	private Vector3 _velocity;

	[SerializeField] PlayerHealth ph;
	[SerializeField] CameraFocus CF;

	private void Start(){
		GetComponent<SphereCollider>().enabled = lethal;
		//CF = FindObjectOfType<CameraSh>().gameObject;
		CF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
	}

    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0) { Destroy(gameObject); }

        _velocity += Vector3.down * (gravity * Time.deltaTime);
        transform.position += _velocity * Time.deltaTime;
    }
	
    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("hitititititi");
        if (other.gameObject.tag == "Obstacle")
        {
			Destroy(gameObject);
			return;
        }
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("hit");
			ph = other.gameObject.GetComponent<PlayerHealth>();
			ph.TakeDamage(1);
			Debug.Log(ph.health);

			
			CF.RemoveTarget(other.transform);
			
			other.gameObject.SetActive(false);
			Debug.Log(ph.health);
			
			Destroy(gameObject);
		}
		
	}
	

    public void Fire(Vector3 vec){
		_velocity += vec;
	}
}
