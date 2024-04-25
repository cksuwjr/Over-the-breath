using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] internal int spawnId = -1;
    [SerializeField] internal GameObject spawnThing;
    internal GameObject spawnedInstance;
}
