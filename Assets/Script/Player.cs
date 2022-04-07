using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    SpriteRenderer sr;  // Move, Skill
    Animator anim;  // Move, Skill

    Status stat;    // Move, Skill
    Skill skill;
    PlayerUI playerUI;

    bool die = false;

    public string ChangeMode;                      // DragonMode, public

    public GameObject DamageText;           // Skill

    public Color originColor;                      // Skill

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        stat = GetComponent<Status>();
        skill = GetComponent<Skill>();
        playerUI = GetComponentInChildren<PlayerUI>();

        originColor = sr.color;


        
        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUip") as GameObject;


        ChangeDragon("default");

    }
    void Start()
    {
        playerUI.PlayerUIUpdate();
    }
    // Player 스탯 변화
    public void PlayerStatChange(Dictionary<string, float> statu)
    {
        foreach(string Plus_stat in statu.Keys)
        {
            float value = statu[Plus_stat];
            switch (Plus_stat)
            {
                case "Hp":
                    stat.HP += value;
                    if (stat.HP > stat.MaxHp)
                        stat.HP = stat.MaxHp;
                    break;
                case "AttackPower":
                    stat.AttackPower += (int)value;
                    break;
                case "MaxHp":
                    stat.MaxHp += value;
                    break;
                case "MoveSpeed":
                    stat.MoveSpeed += value;
                    break;
                case "JumpPower":
                    stat.JumpPower += value;
                    break;
            }
        }

        playerUI.PlayerUIUpdate();
        CheckEvent();
    }


    // 드래곤 변화 조건 설정 및 변화시작 설정
    public void CheckEvent(string fixmode = null)
    {
        if (ChangeMode == "default")
        {
            if (stat.AttackPower >= 80)
                ChangeDragon("iron");
        }

    }

    // 드래곤 외형 변화 및 히트박스 활성화
    public void ChangeDragon(string dragontype)
    {
        ChangeMode = dragontype;
        switch (ChangeMode)
        {
            case "default":
                anim.SetInteger("ChangeMode", 0);
                break;
            case "iron":
                anim.SetInteger("ChangeMode", 1);
                anim.SetTrigger("Change");
                // 히트박스 활성화
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                break;
        }
    }

    // 피해 입을시
    public void GetDamage(int damage)
    {
        if (!die)
        {

            stat.HP -= damage;
            playerUI.PlayerUIUpdate();

            StartCoroutine("BeShadowing");
            PopUpDamageText(damage);

            if (stat.HP <= 0)
                Die();
        }
    }


    // 데미지 수치 UI 띄우기
    void PopUpDamageText(int damage)
    {
        GameObject DamageUI = Instantiate(DamageText, transform.localPosition, Quaternion.identity);
        DamageUI.GetComponentInChildren<DamageUI>().Spawn(damage, gameObject);
    }

    // 피격당할시 깜빡거림
    IEnumerator BeShadowing()
    {
        if (!skill.isMumchit)
            anim.SetTrigger("Hitted");
        Color color = sr.color;

        sr.color = color;
        for (int i = 0; i < 3; i++)
        {
            while (color.r >= 0.5f)
            {
                color.r -= (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                color.g -= (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                color.b -= (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                color.a = sr.color.a;
                sr.color = color;
                yield return null;
            }
            while (color.r <= 1f)
            {
                color.r += (Time.deltaTime / 0.5f);
                color.g += (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                color.b += (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                color.a = sr.color.a;
                sr.color = color;
                yield return null;
            }
        }
        sr.color = originColor;
    }

    // 죽음시 
    void Die()
    {
        stat.HP = stat.MaxHp;
        playerUI.PlayerUIUpdate();
        playerUI.PopupPlayerDieUI();
        die = true;
        gameObject.SetActive(false);
    }

}