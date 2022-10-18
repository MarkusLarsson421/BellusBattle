using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 1;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            onGameOver.Invoke();
        }
    }
}
