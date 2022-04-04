using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Text DieCount;
    private void Start()
    {
        DieCount = transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }
    public void PlayerDie(Status stat)
    {
        StartCoroutine("CountingDieCount", stat);
    }
    IEnumerator CountingDieCount(Status stat)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int second = 10; second > 0; second--)
        {
            DieCount.text = second.ToString();
            yield return new WaitForSeconds(1f);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Spawner").GetComponent<Spawner>().PlayerReSpawn(stat);
    }
}
