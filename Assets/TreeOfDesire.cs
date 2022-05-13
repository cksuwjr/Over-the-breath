using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfDesire : MonoBehaviour
{
    public GameObject Seed;

    Coroutine ActCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        ActCoroutine = StartCoroutine("Act");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Act()
    {
        yield return new WaitForSeconds(10f);

        int Actnum = 0;//Random.Range(0, 3);
        switch (Actnum)
        {
            case 0:
                SummonSeeds(Random.Range(8,20));
                break;
        }
        //yield return new WaitForSeconds(10f);
        ActCoroutine = StartCoroutine("Act");
    }
    void SummonSeeds(float count)
    {
        Debug.Log("น฿ป็!");
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SeedManage());
        }
    }
    IEnumerator SeedManage()
    {
        GameObject Spawned = Instantiate(Seed, transform.position, Quaternion.identity);
        Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.02f, 0.02f), Random.Range(0.02f, 0.04f)));
        yield return new WaitForSeconds(3f);
        Spawned.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        Spawned.GetComponent<CircleCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(3f);
        Destroy(Spawned);
    }
}
