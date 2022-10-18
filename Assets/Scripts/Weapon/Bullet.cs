using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] [Tooltip("For how long the bullet will exist in seconds.")]
    private float lifeSpan = 5.0f;

    private void Start(){
        StartCoroutine(Shoot(lifeSpan));
    }

    private IEnumerator Shoot(float seconds){
        yield return new WaitForSeconds(seconds);
        Die();
    }

    private void Die(){
        Destroy(gameObject);
    }
}
