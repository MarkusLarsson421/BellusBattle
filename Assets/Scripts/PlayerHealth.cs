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

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private SkinnedMeshRenderer skr;
    [SerializeField] private MeshRenderer gunMesh;
    [SerializeField] private MeshRenderer swordMesh;
    [SerializeField] private MeshRenderer grenadeMesh;
    [SerializeField] private PickUp_ProtoV1 ppV1;

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
            KillPlayer();
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
