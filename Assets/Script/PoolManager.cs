using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] pools;

    private static PoolManager instance;

    public static PoolManager Instance
    {
        get { 
            return instance; 
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        pools = new List<GameObject>[prefabs.Length];

        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, bool active = true)
    {
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(active);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
