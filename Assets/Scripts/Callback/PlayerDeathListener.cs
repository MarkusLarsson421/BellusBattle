using UnityEngine;

public class PlayerDeathListener : MonoBehaviour
{
	[SerializeField] [Tooltip("The camera which focuses on the different players.")]
	private CameraFocus cf;

	private void Start()
	{
		PlayerDeathEvent.RegisterListener(OnPlayerDeath);
	}

	private void OnPlayerDeath(PlayerDeathEvent pde)
	{
		Debug.Log("Player " + pde.Kille + " was killed by " + pde.KilledBy + " using " + pde.KilledWith + ".");
		cf.RemoveTarget(pde.PlayerGo.transform);
		pde.PlayerGo.SetActive(false);
	}

	private void OnDestroy()
	{
		PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
	}
}
