using UnityEngine;

//Collection class containing all projectiles.
public class Projectile : MonoBehaviour{
	protected GameObject Shooter;

	public void SetShooter(GameObject playerGo)
	{
		Shooter = playerGo;
	}
}
