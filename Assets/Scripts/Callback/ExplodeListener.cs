using System.Collections;
using UnityEngine;

public class ExplodeListener : MonoBehaviour
{
	[SerializeField] private GameObject particleSys;
	
	private void Start()
	{
		ExplodeEvent.RegisterListener(OnUnitDied);
	}

	private void OnUnitDied(ExplodeEvent ee)
	{
		GameObject go = Instantiate(particleSys, ee.ExplosionGO.transform);
		go.transform.parent = null;
		ParticleSystem particle = go.GetComponent<ParticleSystem>();
		particle.Play();
		StartCoroutine(Explode(particle.main.duration, go));
	}
	
	private IEnumerator Explode(float seconds, Object go)
	{
		yield return new WaitForSeconds(seconds);

		Destroy(go);
	}

	private void OnDestroy()
	{
		ExplodeEvent.UnregisterListener(OnUnitDied);
	}
}