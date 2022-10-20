using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CameraFocus CF;
    public float health = 1;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    private void Start()
    {
        
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Ded");
            
            //onGameOver.Invoke();
        }
    }
}
