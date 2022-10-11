using UnityEngine;

public class Item : MonoBehaviour{
	private void PickUp()
    {
	    Die();
    }

	private void Die(){
        PickUpEvent pickUpEvent = new PickUpEvent();
        pickUpEvent.PickUpGo = gameObject;
        pickUpEvent.Description = name + " was picked up.";
        pickUpEvent.FireEvent();
        Destroy(gameObject);
	}
}