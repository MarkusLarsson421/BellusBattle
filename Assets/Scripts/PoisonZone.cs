using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonZone : MonoBehaviour
{
    [SerializeField] private float timeToKill;
    private static Dictionary<GameObject, int> poisonDic = new Dictionary<GameObject, int>();
    private List<GameObject> playersInZone = new List<GameObject>();
    private Collider[] collidersInZone;
    
    private BoxCollider zoneCollider;
    // Start is called before the first frame update
    void Start()
    {
        zoneCollider = GetComponent<BoxCollider>(); 
    }

    // Update is called once per frame
    void Update()
    {
        LookForPLayers();
    }

    private void LookForPLayers()
    {
        collidersInZone = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);
        foreach(Collider col in collidersInZone)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                AddPLayerToDictionary(col.gameObject);
                AddPlayerToList(col.gameObject);
            }
        }
    }

    private void AddPLayerToDictionary(GameObject player)
    {
        if (poisonDic.ContainsKey(player) == true) return;
        poisonDic[player] = 0;
    }

    private void AddPlayerToList(GameObject player)
    {
        if (playersInZone.Contains(player)) return;
        playersInZone.Add(player);
    }
}
