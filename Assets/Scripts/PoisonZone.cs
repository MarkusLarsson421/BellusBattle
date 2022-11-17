using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonZone : MonoBehaviour
{
    [SerializeField] private float timeToKill;
    [SerializeField] private string causeOfDeath = "Poison bullet";
    
    private static Dictionary<GameObject, float> poisonDic = new Dictionary<GameObject, float>();
    private static Dictionary<GameObject, bool> isInZoneDic = new Dictionary<GameObject, bool>();
    private List<GameObject> playersInZone = new List<GameObject>();
    private List<Collider> objectsInZone = new List<Collider>();
    private GameObject shooter;

    void Update()
    {
        LookForPLayers();
        PoisionPlayers();
        if (playersInZone.Count == 0) return;
        
        for (int i = playersInZone.Count - 1; i >= 0; i--)
        {
            if (objectsInZone.Contains(playersInZone[i].GetComponent<BoxCollider>()) == false)
            {
                isInZoneDic[playersInZone[i]] = false;
            }
        }
    }

    private void LookForPLayers()
    {
        objectsInZone = new List<Collider>(Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity)); 
        foreach (Collider col in objectsInZone)
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
        isInZoneDic[player] = true;
        if (poisonDic.ContainsKey(player)) return;
        poisonDic[player] = 0;   
    }

    private void AddPlayerToList(GameObject player)
    {
        if (playersInZone.Contains(player)) return;
        playersInZone.Add(player);
    }

    private void PoisionPlayers()
    {
        if (isInZoneDic.Count == 0) return;

        foreach(GameObject player in playersInZone)
        {

            if (isInZoneDic[player] == false && poisonDic[player] <= 0f) continue;
            poisonDic[player] += isInZoneDic[player] == true ? Time.deltaTime : -Time.deltaTime;

            if(isInZoneDic[player] == false)
            {
                player.GetComponent<PlayerHealth>().StopPoisoned();
            }
            else
            {
                player.GetComponent<PlayerHealth>().PlayPoisoned();
            }
            

            //Debug.Log(poisonDic[player]);
            if(poisonDic[player] >= timeToKill)
            {
                PlayerDeathEvent pde = new PlayerDeathEvent
                {
                    kille = player,
                    killer = shooter,
                    killedWith = causeOfDeath,
                };
                pde.FireEvent();
            }
        }
    }

    public void Clear()
    {
        poisonDic.Clear();
        isInZoneDic.Clear();
        playersInZone.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}
