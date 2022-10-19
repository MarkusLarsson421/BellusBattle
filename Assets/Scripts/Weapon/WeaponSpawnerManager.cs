using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnerManager : MonoBehaviour
{
    [SerializeField] private float spawnWeaponsTimer;
    [SerializeField] private float numberOfWeaponsToSpawn;
    [SerializeField] private WeaponSpawner[] spawners;
    private List<WeaponSpawner> choosenSpawners = new List<WeaponSpawner>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWeapons());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ChooseRandomSpawners()
    {
        int temporaryNumber;
        for(int i = 0; i < numberOfWeaponsToSpawn; i++)
        {
            temporaryNumber = Random.Range(0, spawners.Length);
            if (spawners[temporaryNumber].GetIsHoldingWeapon() == true)
            {
                i--;
                return;
            }
            choosenSpawners.Add(spawners[temporaryNumber]);
        }
    }
    private void SpawnWeaponsInSpawners()
    {
        foreach(var spawner in spawners)
        {
            spawner.SpawnRandomWeapon();
        }
    }
    IEnumerator SpawnWeapons()
    {
        yield return new WaitForSeconds(spawnWeaponsTimer);
        ChooseRandomSpawners();
        SpawnWeaponsInSpawners();
        choosenSpawners.Clear();
        StartCoroutine(SpawnWeapons());
    }
}
