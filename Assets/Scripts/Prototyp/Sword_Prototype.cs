using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Prototype : MonoBehaviour
{

    public GameObject Sword;
    Vector3 pos;
    public bool canAttack = true;
    public float cooldown;
    public Transform ShootPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (canAttack)
            {
                Attack();
            }
        }
        faceGun();
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
