using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour
{
    const float Term = 10f;
    const float MinXRange = 0f;
    const float MaxXRange = 52f;

    List<GameObject> Enemys = new List<GameObject>();
    void Start()
    {
        if (Enemys.Count == 0) {
            Enemys.Add(Resources.Load("Prefab/Slime") as GameObject);
            Enemys.Add(Resources.Load("Prefab/Warrior") as GameObject);
        }
        for (int i = 0; i < Enemys.Count; i++)
            StartCoroutine(Spawn(Enemys[i], Term));
    }

    IEnumerator Spawn(GameObject Enemy, float Term)
    {
        Instantiate(Enemy, new Vector2(Random.Range(MinXRange, MaxXRange), 0), Quaternion.identity);
        yield return new WaitForSeconds(Term);
        StartCoroutine(Spawn(Enemy, Term));
    }
}
