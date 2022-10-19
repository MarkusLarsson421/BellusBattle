using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnerManager : MonoBehaviour
{
    [SerializeField] private float initialSpawnWeaponsTimer;
    [SerializeField] private float spawnWeaponsTimer;
    [SerializeField] private float numberOfWeaponsToSpawn;
    private float currentNumberOfWeaponsToSpawn;
    [SerializeField] private WeaponSpawner[] spawners;
    private List<WeaponSpawner> choosenSpawners = new List<WeaponSpawner>();
    private bool isReadyToSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        ControlNumberOfWeaponToSpawners();

        StartCoroutine(InitialSpawnWeapons());
    }

    // Update is called once per frame
    void Update()
    {
        if(isReadyToSpawn) StartCoroutine(SpawnWeapons());
    }
    private void ChooseRandomSpawners()
    {
        int temporaryNumber;
        for(int i = 0; i < currentNumberOfWeaponsToSpawn && i < spawners.Length; i++)
        {
            temporaryNumber = Random.Range(0, spawners.Length);
            if (spawners[temporaryNumber].GetIsHoldingWeapon() == true)
            {
                currentNumberOfWeaponsToSpawn++;
            }
            else
            {
                choosenSpawners.Add(spawners[temporaryNumber]);
                currentNumberOfWeaponsToSpawn = numberOfWeaponsToSpawn;
            }
        }
    }
    private void SpawnWeaponsInSpawners()
    {
        foreach(var spawner in choosenSpawners)
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
    IEnumerator InitialSpawnWeapons()
    {
        yield return new WaitForSeconds(initialSpawnWeaponsTimer);
        ChooseRandomSpawners();
        SpawnWeaponsInSpawners();
        choosenSpawners.Clear();
        StartCoroutine(SpawnWeapons());
    }

    private void ControlNumberOfWeaponToSpawners()
    {

        if(numberOfWeaponsToSpawn > spawners.Length)
        {
            numberOfWeaponsToSpawn = spawners.Length;
            Debug.LogError("\"numberOfWeaponsToSpawn\" is larger than the number of spawners that exists in the Scene");
        }
        currentNumberOfWeaponsToSpawn = numberOfWeaponsToSpawn;
    }
}
