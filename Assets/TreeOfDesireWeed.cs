using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfDesireWeed : Monster
{
    public GameObject SpawnSpikeRange;
    public GameObject WoodSpike;

    List<GameObject> Spikes = new List<GameObject>();
    List<GameObject> SpikeRanges = new List<GameObject>();
    // Start is called before the first frame update
    protected override void OnEnableInit()
    {
        StartCoroutine("SummonSpike");
    }
    IEnumerator SummonSpike()
    {
        yield return new WaitForSeconds(1f);

        var Player = GameObject.FindGameObjectWithTag("Player");
        {
            var spikeRange = PoolManager.Instance.Get(5);
            try
            {
                SpikeRanges.Add(spikeRange);
                spikeRange.transform.position = Player.transform.position;
            }
            catch
            {
                spikeRange.transform.position = transform.position;
            }
        }
        var naturalMonsters = GameObject.FindGameObjectsWithTag("Neutrality");
        foreach (var monster in naturalMonsters)
        {
            if(monster == gameObject) continue;
            if (monster.GetComponent<Monster>().Body == Body) continue;
            if (monster.GetComponent<Monster>().FixedType) continue;

            var spikeRange = PoolManager.Instance.Get(5);
            try
            {
                SpikeRanges.Add(spikeRange);
                spikeRange.transform.position = monster.transform.position;
            }
            catch
            {
                spikeRange.transform.position = transform.position;
            }
        }
        foreach(var spikeRange in SpikeRanges)
        {
            spikeRange.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        foreach (var spikeRange in SpikeRanges)
        {
            var spike = PoolManager.Instance.Get(7);
            spike.transform.position = spikeRange.transform.position;
            spike.GetComponent<Monster>().Body = Body;
            Spikes.Add(spike);
            spike.SetActive(true);
            spikeRange.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (var spike in Spikes)
        {
            spike.SetActive(false);
        }

        Spikes.Clear();
        SpikeRanges.Clear();
        yield return new WaitForSeconds(3f); // ¿Á»£√‚
        StartCoroutine(SummonSpike());
    }
    private void OnDisable()
    {
        StopCoroutine("SummonSpike");
        foreach(var spike in Spikes)
        {
            spike.SetActive(false);
        }
        foreach(var spikeRange in SpikeRanges)
        {
            spikeRange.SetActive(false);
        }
        Spikes.Clear();
        SpikeRanges.Clear();
    }

}
