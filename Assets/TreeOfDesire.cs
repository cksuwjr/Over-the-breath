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



    GameObject HPUI;
    Image HPbar;
    public GameObject DamageText;



    Coroutine AppearHPUICoroutine;


    public GameObject Seed;
    public GameObject Weed;


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
        yield return new WaitForSeconds(10f);

        int Actnum;
        if (MyFaze == Faze.Faze1) 
        {
             Actnum = Random.Range(0, 2);
        }
        else if(MyFaze == Faze.Faze2)
        {
            Actnum = Random.Range(2, 3);
        }
        else
        {
             Actnum = Random.Range(0, 2);
        }
        switch (Actnum)
        {
            case 0:
                SummonSeeds(Random.Range(8,20));
                break;
            case 1:
                if(GameObject.Find("TreeOfDesire_Weed(Clone)") == null)
                    SummonWeed();
                break;
            case 2:
                ChangeEye();
                break;

        }
        //yield return new WaitForSeconds(10f);
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
        GameObject Spawned = Instantiate(Seed, transform.position, Quaternion.identity);
        Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-0.02f, 0.02f), Random.Range(0.02f, 0.04f)));
        yield return new WaitForSeconds(3f);
        Spawned.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        Spawned.GetComponent<CircleCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(3f);
        Destroy(Spawned);
    }
    void SummonWeed()
    {
        Instantiate(Weed, new Vector3(88.5f, -4.820125f, 0), Quaternion.identity);
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
        if(MyFaze == Faze.Faze1 && stat.HP < stat.MaxHp * 0.5f)
        {
            transform.position = new Vector2(transform.position.x, -6.9f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(GetComponent<CapsuleCollider2D>().offset.x, 3.3f);
            anim.SetTrigger("Faze2");
            MyFaze = Faze.Faze2;

            StartCoroutine(Soaring());
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
    void ChangeEye()
    {
        int EyePosition = Random.Range(0, 3);
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
