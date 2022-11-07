using UnityEngine;
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

    //USCH
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private SkinnedMeshRenderer skr;
    [SerializeField] private MeshRenderer gunMesh;
    [SerializeField] private MeshRenderer swordMesh;
    [SerializeField] private MeshRenderer grenadeMesh;
    [SerializeField] private PickUp_ProtoV1 ppV1;

   
    public void TakeDamage(float damage)
    {
        if(isInvinsable) return;
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
        boxCollider.enabled = false;
        rightArm.SetActive(false);
        skr.enabled = false;
        gunMesh.enabled = false;
        grenadeMesh.enabled = false;
    }

    public void UnkillPlayer()
    {
        boxCollider.enabled = true;
        rightArm.SetActive(true);
        skr.enabled = true;
        gunMesh.enabled = false;
        grenadeMesh.enabled = false;
        swordMesh.enabled = true;
        ppV1.isHoldingWeapon = false;
    }
    
}
