using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon_Porotype : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            //&& !this.gameObject.transform.parent == other.transform.gameObject
            //other.GetComponent<Shoot>().enabled = false;
            other.gameObject.transform.Find("revolver_low").gameObject.SetActive(false);
            other.gameObject.transform.Find("Sword").gameObject.SetActive(true);



        }
    }
}
