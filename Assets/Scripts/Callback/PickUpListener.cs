using UnityEngine;

public class PickUpListener : MonoBehaviour
{
	[SerializeField] private GameObject particleSystem;
	
	private void Start()
	{
		PickUpEvent.RegisterListener(OnPickUp);
	}

	private void OnPickUp(PickUpEvent pue)
	{
		GameObject go = Instantiate(particleSystem, pue.PickUpGo.transform);
		go.transform.parent = null;
		ParticleSystem particle = go.GetComponent<ParticleSystem>();
		particle.Play();
		Destroy(go);
	}

	private void OnDestroy()
	{
		PickUpEvent.UnregisterListener(OnPickUp);
	}
}
