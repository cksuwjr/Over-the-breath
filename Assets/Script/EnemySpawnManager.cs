using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    int Enemys = 0;
    EnemySpawner[] Spawners;

    public int MaxCount = 10;
    void Start()
    {
        Spawners = GetComponentsInChildren<EnemySpawner>();
        
    }
    public void AdjustEnemyCount(int n)
    {
        Enemys += n;

        if (Enemys >= MaxCount)
            for (int i = 0; i < Spawners.Length; i++)
                Spawners[i].stopSpawn();
        else
            for (int i = 0; i < Spawners.Length; i++)
                Spawners[i].resumeSpawn();
    }
}
