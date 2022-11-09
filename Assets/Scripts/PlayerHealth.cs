using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private CameraFocus CF;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    [SerializeField] private AudioSource playerDeathSound;

    private float health = 1;
    private bool isInvinsable=false;

    [SerializeField] Transform deathPosition;

    public float Health { get => health; }

    //USCH
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private SkinnedMeshRenderer skr;

    private void OnLevelWasLoaded(int level)
    {
        CF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
    }

    public void TakeDamage(float damage)
    {
        //if(isInvinsable) return;
        health -= damage;
        if (health <= 0)
        {
            KillPlayer();
            playerDeathSound.Play();
            //onGameOver.Invoke();
        }
    }
    public void SetInvincible( bool value)
    {
        isInvinsable = value;
    }


    public void KillPlayer()
    {
        gameObject.transform.position = deathPosition.position;
    }

    public void UnkillPlayer()
    {
        skr.enabled = true;
        /*
        boxCollider.enabled = true;
        rightArm.SetActive(true);
        skr.enabled = true;
        */
        //gunMesh.enabled = false;
        //grenadeMesh.enabled = false;
        //swordMesh.enabled = true;
        //ppV1.isHoldingWeapon = false;
    }
    
}
