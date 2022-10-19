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
    public float cooldown = 3;
    public Transform ShootPoint;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    public void Attack(InputAction.CallbackContext ctx)
    {
        pos = Sword.transform.position;
        if (!ctx.performed) { return; }
        if (canAttack)
        {
            Sword.transform.position += ShootPoint.transform.up * 1.02f;
            canAttack = false;
            StartCoroutine(ResetAttack());
            Debug.Log(pos);
        }

        
    }

    IEnumerator ResetAttack()
    {
        Debug.Log(pos);
        Sword.transform.position = pos;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
