using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Prototype : MonoBehaviour
{

    public GameObject Sword;

    public bool canAttack = true;
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        faceGun();
        if (Input.GetMouseButton(0))
        {
            if (canAttack)
            {
                Attack();
            }
        }
    }
    void faceGun()
    {
        float camToPlayerDist = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camToPlayerDist));
        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Sword.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
    void Attack()
    {
        canAttack = false;
        Animator ani = Sword.GetComponent<Animator>();
        ani.SetTrigger("Attack");
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
