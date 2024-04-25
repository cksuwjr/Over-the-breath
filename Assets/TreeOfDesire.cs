using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfDesire : Monster
{
    enum Faze
    {
        Faze1,
        Faze2
    }

    Faze MyFaze = Faze.Faze1;

    int EyePosition = 2;

    public GameObject Seed;
    public GameObject Weed;
    public GameObject EarthquakeWeed;

    protected override void LevelUp()
    {
        float nowHp = stat.HP;
        base.LevelUp();
        stat.HP = nowHp;
    }

    protected override IEnumerator Act()
    {
        // ·£´ý ·ê·¿
        int Actnum = 0;
        if (MyFaze == Faze.Faze1)
        {
            if (GameObject.Find("TreeOfDesire_Weed(Clone)") == null)
                Actnum = Random.Range(0, 2);
            else
                Actnum = 0;
        }
        else if (MyFaze == Faze.Faze2)
        {
            Actnum = Random.Range(0, 4);
        }
        // Çàµ¿
        switch (Actnum)
        {
            case 0: // ¼ö¸¹Àº ¾¾¾Ñ¼ÒÈ¯
                SummonSeeds(Random.Range(8,20));
                break;
            case 1: // ÁÙ±â ¼ÒÈ¯
                if (GameObject.Find("TreeOfDesire_Weed(Clone)") == null)
                {
                    if (MyFaze == Faze.Faze1)
                        SummonWeed(new Vector2(88.5f + 232.82f, -4.820125f - 20.84f), new Vector2(1, 1));
                    else if (MyFaze == Faze.Faze2)
                        StartCoroutine(GrowingWeeds());
                }
                break;
            case 2: // ´« À§Ä¡ º¯È¯
                ChangeEye();
                break;
            case 3: // ÁöÁø ÁÙ±â ¼ÒÈ¯
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    if (player.transform.position.y < -21.9f)
                    {
                        SummonEarthquake(player.transform.position.x);
                        yield return new WaitForSeconds(4.5f);
                    }
                }

                break;

        }
        // µô·¹ÀÌ
        switch (Actnum)
        {
            case 0:
                if(MyFaze == Faze.Faze1)
                    yield return new WaitForSeconds(5f);
                else if(MyFaze == Faze.Faze2)
                    yield return new WaitForSeconds(2f);
                break;
            case 1:
                if (MyFaze == Faze.Faze1)
                    yield return new WaitForSeconds(10f);
                else if (MyFaze == Faze.Faze2)
                    yield return new WaitForSeconds(3f);
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                break;
            case 3:
                yield return new WaitForSeconds(0f);
                break;
            default:
                yield return new WaitForSeconds(0f);
                break;
        }
        ActCoroutine = StartCoroutine("Act");
    }
    void SummonSeeds(float count)
    {
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SeedManage());
        }
    }
    IEnumerator SeedManage()
    {
        Vector2 spawnpos;
        switch (EyePosition)
        {
            case 0:
                spawnpos = new Vector2(transform.position.x, -23.3f + 1.55f + 1.6f);
                break;
            case 1:
                spawnpos = new Vector2(transform.position.x, -23.3f + 1.55f);
                break;
            case 2:
                spawnpos = new Vector2(transform.position.x, -23.3f);
                break;
            default:
                spawnpos = new Vector2(transform.position.x, 0.15f);
                break;
        }
        GameObject Spawned = PoolManager.Instance.Get(4);
        Spawned.GetComponent<Status>().AttackPower = stat.AttackPower / 3;
        Spawned.transform.position = spawnpos;
        Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.02f, 0.02f), Random.Range(0.02f, 0.04f)));
        yield return new WaitForSeconds(3f);
        Spawned.SetActive(false);
    }
    void SummonWeed(Vector2 SummonPosition, Vector2 SummonScale)
    {
        GameObject SpawnedWeed = PoolManager.Instance.Get(6);
        SpawnedWeed.GetComponent<Status>().AttackPower = stat.AttackPower;
        SpawnedWeed.GetComponent<Monster>().Body = gameObject;
        SpawnedWeed.transform.position = SummonPosition;
        SpawnedWeed.transform.localScale = SummonScale;
    }
    void SummonEarthquake(float x)
    {
        var weed = PoolManager.Instance.Get(8);
        weed.transform.position = new Vector2(x, -3.538542f - 20.84f);
        Collider2D[] colliders = Physics2D.OverlapAreaAll(weed.transform.position + new Vector3(-2.13f, -2.36f), weed.transform.position + new Vector3(2.47f, 2.24f));
        Collider2D[] myCollider = GetComponentsInChildren<Collider2D>();

        List<GameObject> myEnemys = new List<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            bool isMeinclude = false;
            foreach (Collider2D mine in myCollider) if (collider == mine) isMeinclude = true;
            if (isMeinclude) continue;

            if (!myEnemys.Contains(collider.gameObject))
                myEnemys.Add(collider.gameObject);
        }
        foreach (GameObject enemy in myEnemys)
        {
            var monster = enemy.GetComponent<Monster>();
            var player = enemy.GetComponent<Player>();
            if (monster)
            {
                if (!monster.die)
                {
                    monster.GetDamaged(stat.AttackPower, gameObject);
                    monster.GetAirborne(new Vector3(0, 12));
                }
            }

            if (player)
            {
                player.GetDamage(stat.AttackPower, gameObject);
                player.GetAirborne(new Vector3(0, 12));
            }
        }
    }


    // ·£´ý ¼ýÀÚ ¹ß±Þ, OntriggerÀÇ GetDamage¿¡ ¾²ÀÓ
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    public override void DieEvent()
    {
        if (DieAndStartStory != "" && DieAndStartStory != null)
            GameManager.Instance.StoryManager.StartScenario(DieAndStartStory);
        //GameObject nextstage = Instantiate(Resources.Load("Prefab/NextStage") as GameObject, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        //SceneTransport nt = nextstage.GetComponent<SceneTransport>();
        //nt.NextScene = "new1-6";
    }

    public override void HitEvent()
    {

        if (MyFaze == Faze.Faze1 && stat.HP < stat.MaxHp * 0.5f)
        {
            transform.position = new Vector2(transform.position.x, -6.9f - 20.84f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(GetComponent<CapsuleCollider2D>().offset.x, 3.3f - 20.84f);
            anim.SetTrigger("Faze2");
            MyFaze = Faze.Faze2;

            StartCoroutine(Soaring());
            StartCoroutine(GrowingWeeds());
            //transform.position = new Vector2(transform.position.x, -1.24f);
        }
    }
    IEnumerator Soaring()
    {
        Transform tr = transform;
        for (int i = 0; i < 10; i++)
        {
            stat.HP += (stat.MaxHp * 0.05f);
            stat.AttackPower += 10;
            GetDamaged(1, gameObject);
            DamageReceivePercent -= 0.05f;

            tr.position += new Vector3(0, 0.396f);
            yield return new WaitForSeconds(0.1f);
        }
        GetComponent<CapsuleCollider2D>().offset = new Vector2(GetComponent<CapsuleCollider2D>().offset.x, 1.7f);
        stat.HP = stat.MaxHp;
        GetDamaged(1, gameObject);
    }
    IEnumerator GrowingWeeds()
    {
        yield return new WaitForSeconds(1f);
        SummonWeed(new Vector2(88.53f + 232.82f, -2.8f - 20.84f), new Vector2(1,0.6f));
        yield return new WaitForSeconds(0.75f);
        SummonWeed(new Vector2(88.67f + 232.82f, -0.27f - 20.84f), new Vector2(1.178f, 0.48f));
        yield return new WaitForSeconds(0.75f);
        SummonWeed(new Vector2(91.65f + 232.82f, 1.25f - 20.84f), new Vector2(-1.4f, -0.6f));
        yield return new WaitForSeconds(0.75f);
        SummonWeed(new Vector2(91.8f + 232.82f, -0.32f - 20.84f), new Vector2(-0.87f, -0.53f));
    }
    void ChangeEye()
    {
        EyePosition = Random.Range(0, 3);
        anim.SetInteger("Eye", EyePosition);
        CapsuleCollider2D cc;
        cc = GetComponent<CapsuleCollider2D>();
        switch (EyePosition)
        {
            case 0:
                cc.offset = new Vector2(cc.offset.x, 3.3f); 
                break;
            case 1:
                cc.offset = new Vector2(cc.offset.x, 1.7f);
                break;
            case 2:
                cc.offset = new Vector2(cc.offset.x, 0.15f);
                break;
        }
    }
}
