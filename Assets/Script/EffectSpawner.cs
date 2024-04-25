using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    private GameObject effect;
    float time;
    public void SpawnEffect(int id, float time)
    {
        if (id == -1) return;
        this.time = time;
        StartCoroutine("Effect",id);
    }
    private IEnumerator Effect(int id)
    {
        effect = PoolManager.Instance.Get(id);
        effect.transform.SetParent(transform);
        effect.transform.localPosition = spawnPosition;
        yield return new WaitForSeconds(time);
        if(effect.activeSelf)
            effect.SetActive(false);
    }
    private void OnDisable()
    {
        StopCoroutine("Effect");
        if(effect)
            effect.SetActive(false);
    }
}
