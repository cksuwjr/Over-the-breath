using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfDesire : MonoBehaviour
{
    Animator anim;
    Status stat;
    enum Faze
    {
        Faze1,
        Faze2
    }

    Faze MyFaze = Faze.Faze1;

    int EyePosition = 2;

    GameObject HPUI;
    Image HPbar;
    public GameObject DamageText;



    Coroutine AppearHPUICoroutine;


    public GameObject Seed;
    public GameObject Weed;
    public GameObject EarthquakeWeed;


    Coroutine ActCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        // UI 관련
        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;

        stat = GetComponent<Status>();







        ActCoroutine = StartCoroutine("Act");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Act()
    {
        // 랜덤 룰렛
        int Actnum = 0;
        if (MyFaze == Faze.Faze1)
        {
            Actnum = Random.Range(0, 2);
        }
        else if (MyFaze == Faze.Faze2)
        {
            Actnum = Random.Range(0, 4);
        }
        // 행동

        switch (Actnum)
        {
            case 0: // 수많은 씨앗소환
                SummonSeeds(Random.Range(8,20));
                break;
            case 1: // 줄기 소환
                if(GameObject.Find("TreeOfDesire_Weed(Clone)") == null)
                    SummonWeed(new Vector2(88.5f + 232.82f, -4.820125f - 20.84f), new Vector2(1,1));
                
                break;
            case 2: // 눈 위치 변환
                ChangeEye();
                break;
            case 3: // 지진 줄기 소환
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    if (player.transform.position.y < -0.8f)
                    {
                        SummonEarthquake(player.transform.position.x);
                        yield return new WaitForSeconds(2f);
                    }
                }

                break;

        }
        // 딜레이
        switch (Actnum)
        {
            case 0:
                yield return new WaitForSeconds(5f);
                break;
            case 1:
                yield return new WaitForSeconds(10f);
                break;
            case 2:
                yield return new WaitForSeconds(1f);
                break;
            case 3:
                yield return new WaitForSeconds(0f);
                break;
            default:
                yield return new WaitForSeconds(10f);
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
                spawnpos = new Vector2(transform.position.x, 3.3f);
                break;
            case 1:
                spawnpos = new Vector2(transform.position.x, 1.7f);
                break;
            case 2:
                spawnpos = new Vector2(transform.position.x, 0.15f);
                break;
            default:
                spawnpos = transform.position;
                break;
        }
        GameObject Spawned = Instantiate(Seed, spawnpos, Quaternion.identity);
        Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.02f, 0.02f), Random.Range(0.02f, 0.04f)));
        yield return new WaitForSeconds(3f);
        Spawned.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        Spawned.GetComponent<CircleCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(3f);
        Destroy(Spawned);
    }
    void SummonWeed(Vector2 SummonPosition, Vector2 SummonScale)
    {
        GameObject SpawnedWeed = Instantiate(Weed, SummonPosition, Quaternion.identity);
        SpawnedWeed.transform.localScale = SummonScale;
    }
    void SummonEarthquake(float x)
    {        
        Instantiate(EarthquakeWeed, new Vector2(x, -3.538542f - 20.84f), Quaternion.identity);
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            GetDamaged(GetRandomDamageValue(col.GetComponent<Fire>().Damage, 0.8f, 1.2f), GameObject.FindGameObjectWithTag("Player"));
            Destroy(col.gameObject);
        }
    }
    // 랜덤 숫자 발급, Ontrigger의 GetDamage에 쓰임
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    // 데미지 입음
    public void GetDamaged(float damage, GameObject Fromwho)
    {
        stat.HP -= damage;                          // 체력감소
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI 업데이트

        if (AppearHPUICoroutine != null)
            StopCoroutine(AppearHPUICoroutine);
        AppearHPUICoroutine = StartCoroutine("AppearHPUI");     // HP UI 보이기

        PopUpDamageText(damage);

        FazeCheck();

        if (stat.HP <= 0)                           // 체력 다달면
        {
            Die(Fromwho);
        }
    }
    void Die(GameObject Fromwho)
    {
        //if(Fromwho != null){
        //      Fromwho.GetComponent<Status>().
        //}
        for(int i = 0; i < 5; i++)
            if (GameObject.Find("TreeOfDesire_Weed(Clone)") != null)
                Destroy(GameObject.Find("TreeOfDesire_Weed(Clone)"));
        if (GameObject.Find("Trap") != null)
            Destroy(GameObject.Find("Trap"));

        GameObject nextstage = Instantiate(Resources.Load("Prefab/NextStage") as GameObject, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
        SceneTransport nt = nextstage.GetComponent<SceneTransport>();
        nt.NextScene = "new1-6";
        nt.StartStory = "1-5DieTree";




        Destroy(gameObject);
    }

    // 체력바 UI 6초간 보이기
    IEnumerator AppearHPUI()
    {
        HPUI.SetActive(true);
        yield return new WaitForSeconds(6f);
        HPUI.SetActive(false);
    }

    void PopUpDamageText(float damage)
    {
        GameObject DamageUI = Instantiate(DamageText, transform.localPosition, Quaternion.identity);
        DamageUI.GetComponentInChildren<DamageUI>().Spawn((int)damage, gameObject);
    }

    void FazeCheck()
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
            tr.position += new Vector3(0, 0.396f);
            yield return new WaitForSeconds(0.1f);
        }
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
