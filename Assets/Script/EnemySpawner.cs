using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public float SpawnTerm = 5f;
    public float StartTime = 0f;
    bool isSpawnable = true;
    void Start()
    {
        if (SpawnTerm == 0f)
            SpawnTerm = 5f;

        if (Enemy == null)
            Enemy = Resources.Load("Prefab/Slime") as GameObject;
        InvokeRepeating("EnemySpawn", StartTime, SpawnTerm);
    }
    void EnemySpawn()
    {
        if (isSpawnable)
        {
            StartCoroutine("CalTime");
            GetComponentInParent<EnemySpawnManager>().AdjustEnemyCount(+1);
            GameObject spawned = Instantiate(Enemy, transform.position, Quaternion.identity);
            if(spawned.tag == "Enemy" || spawned.tag == "Neutrality")
                spawned.GetComponent<EnemyAI2>().MySpawner(transform.parent.gameObject);
            else if(spawned.tag == "Trap")
                spawned.GetComponent<TrapDamage>().MySpawner(transform.parent.gameObject);
        }
    }
    public void resumeSpawn()
    {
        if(isSpawnable)
            InvokeRepeating("EnemySpawn", StartTime, SpawnTerm);
    }
    public void stopSpawn()
    {
        CancelInvoke("EnemySpawn");
    }
    IEnumerator CalTime()
    {
        isSpawnable = false;
        yield return new WaitForSeconds(SpawnTerm);
        isSpawnable = true;
    }
}
