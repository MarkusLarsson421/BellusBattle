using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_Prototype : MonoBehaviour
{

    public GameObject Sword;
    Vector3 pos;
    Vector3 rotation;
    public bool canAttack = true;
    public float cooldown;
    public Transform ShootPoint;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (canAttack)
            {
                //Attack();
            }
        }
    }
   
    void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }

        pos = Sword.transform.position;
        Sword.transform.position += ShootPoint.transform.up * 1.1f;
        canAttack = false;
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        
        yield return new WaitForSeconds(cooldown);
        Sword.transform.position = pos;
        canAttack = true;
    }

}
