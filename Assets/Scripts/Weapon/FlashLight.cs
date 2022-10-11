using UnityEngine;

public class FlashLight : MonoBehaviour{
	private Light flashLight;
    private bool isOn = true;

    private void Start()
    {
        flashLight = GetComponent<Light>();
    }

    public void OnFire()
    {
	    Toggle();
    }

    public void TurnOn()
    {
	    isOn = true;
        flashLight.enabled = true;
    }

    public void TurnOff()
    {
        isOn = false;
        flashLight.enabled = false;
    }

    private void Toggle()
    { 
	    if (isOn) {TurnOff();}
	    else {TurnOn();}
    }
}