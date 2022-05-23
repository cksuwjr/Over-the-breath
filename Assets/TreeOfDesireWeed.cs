using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfDesireWeed : MonoBehaviour
{

    Status stat;


    GameObject HPUI;
    Image HPbar;
    public GameObject DamageText;

    Coroutine AppearHPUICoroutine;


    public GameObject SpawnSpikeRange;
    public GameObject WoodSpike;

    GameObject Spike;
    GameObject SpikeRange;
    // Start is called before the first frame update
    void Start()
    {
        // UI ����
        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;

        stat = GetComponent<Status>();

        StartCoroutine(SummonSpike());
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            GetDamaged(GetRandomDamageValue(col.GetComponent<Fire>().Damage, 0.8f, 1.2f), GameObject.FindGameObjectWithTag("Player"));
            Destroy(col.gameObject);
        }
    }
    // ���� ���� �߱�, Ontrigger�� GetDamage�� ����
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    // ������ ����
    public void GetDamaged(float damage, GameObject Fromwho)
    {
        stat.HP -= damage;                          // ü�°���
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI ������Ʈ

        if (AppearHPUICoroutine != null)
            StopCoroutine(AppearHPUICoroutine);
        AppearHPUICoroutine = StartCoroutine("AppearHPUI");     // HP UI ���̱�

        PopUpDamageText(damage);



        if (stat.HP <= 0)                           // ü�� �ٴ޸�
        {
            Die(Fromwho);
        }
    }
    void Die(GameObject Fromwho)
    {
        if (SpikeRange != null)
            Destroy(SpikeRange);
        if (Spike != null)
            Destroy(Spike);
        Destroy(gameObject);
    }

    // ü�¹� UI 6�ʰ� ���̱�
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

    IEnumerator SummonSpike()
    {
        try
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            SpikeRange = Instantiate(SpawnSpikeRange, Player.transform.position, Quaternion.identity);
        }catch{
            SpikeRange = Instantiate(SpawnSpikeRange, transform.position, Quaternion.identity);
        }

        SpikeRange.SetActive(true);
        yield return new WaitForSeconds(1f);
        Spike = Instantiate(WoodSpike, SpikeRange.transform.position, Quaternion.identity);
        Destroy(SpikeRange);
        yield return new WaitForSeconds(0.5f);
        Destroy(Spike);
        yield return new WaitForSeconds(3f); // ��ȣ��
        StartCoroutine(SummonSpike());
    }


}
