using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CameraFocus CF;
    public float health = 1;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    private bool isInvinsable=false;

    private void Start()
    {
        
    }
    public void TakeDamage(float damage)
    {
        if(isInvinsable) return;
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Ded");
            
            //onGameOver.Invoke();
        }
    }
    public void SetIsInvinsable( bool value)
    {
        isInvinsable = value;
    }
}
