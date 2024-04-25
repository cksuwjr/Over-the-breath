using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    EnemySpawner[] Spawners;

    [SerializeField] float CheckTerm = 20f;
    void Start()
    {

        try
        {
            StartCoroutine("Spawn");
        }
        catch { }
    }

    IEnumerator Spawn()
    {
        Spawners = GetComponentsInChildren<EnemySpawner>();

        foreach (EnemySpawner spawner in GetReadySpawners())
        {
            if (spawner.spawnId != -1)
                spawner.spawnedInstance = PoolManager.Instance.Get(spawner.spawnId);
            else
            {
                if (spawner.spawnedInstance)
                {
                    var obj = spawner.spawnedInstance;
                    obj.transform.position = spawner.transform.position;
                    obj.SetActive(true);
                }
                else
                {
                    spawner.spawnedInstance = Instantiate(spawner.spawnThing, spawner.transform.position, spawner.spawnThing.transform.rotation);
                }
            }
            spawner.spawnedInstance.transform.position = spawner.transform.position;
        }

        yield return new WaitForSeconds(CheckTerm);
        StartCoroutine("Spawn");
    }
    List<EnemySpawner> GetReadySpawners()
    {
        List<EnemySpawner> readiedSpawners = new List<EnemySpawner>();
        foreach(EnemySpawner spawner in Spawners)
        {
            //if (!spawner.spawnedInstance.activeSelf)
            //    spawner.spawnedInstance = null;
            if (spawner.spawnedInstance == null)
                readiedSpawners.Add(spawner);
            else
                if(!spawner.spawnedInstance.activeSelf)
                    readiedSpawners.Add(spawner);
        }
        return readiedSpawners;
    }

}
