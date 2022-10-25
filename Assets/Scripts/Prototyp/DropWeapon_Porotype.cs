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
            other.gameObject.transform.Find("revolver").gameObject.SetActive(false);
            other.gameObject.transform.Find("Sword").gameObject.SetActive(true);
            Debug.Log("Dropeed");
        }
    }
}
