using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    SpriteRenderer sr;  // Move, Skill
    Animator anim;  // Move, Skill

    [SerializeField] LevelUpExpData levelUpExpData;

    public Status stat;    // Move, Skill
    public PlayerSkill skill;
    PlayerUI playerUI;

    bool die = false;

    public string ChangeMode;                      // DragonMode, public

    public GameObject DamageText;           // Skill
    public GameObject HealText;           // Skill
    public GameObject CriticalDamageText;


    public Color originColor;                      // Skill

    public LayerMask groundLayer;

    private GameObject nowDamageEffect;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        stat = GetComponent<Status>();
        skill = GetComponent<PlayerSkill>();
        
        playerUI = GetComponentInChildren<PlayerUI>();

        originColor = sr.color;
        
        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUip") as GameObject;
        if (HealText == null)
            HealText = Resources.Load("Prefab/HealUip") as GameObject;

        ChangeDragon("default");
    }
    private void Start()
    {
        playerUI.PlayerUIUpdate();
    }
    private void OnEnable()
    {
        stat.HP = stat.MaxHp;
        playerUI.PlayerUIUpdate();

        sr.color = originColor;
        
        die = false;

        ChangeDragon(ChangeMode);

    }
    // Player 스탯 변화
    public void GetExp(float value)
    {
        stat.Exp += value;
        if (stat.Exp > stat.MaxExp)
        {
            var excess = stat.Exp - stat.MaxExp;
            LevelUp();
            GetExp(excess);
        }
        playerUI.PlayerUIUpdate();
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
                break;
            case "fire":
                anim.SetInteger("ChangeMode", 2);
                anim.SetTrigger("Change");
                break;
        }
        playerUI.PlayerUIUpdate();
    }

    void LevelUp()
    {
        stat.Level += 1;
        switch (ChangeMode)
        {
            case "default":
                stat.MaxHp += 12;
                break;
            case "iron":
                stat.MaxHp += 20;
                break;
            case "fire":
                stat.MaxHp += 10;
                break;
        }
        stat.HP = stat.MaxHp;
        stat.BasicAttackPower += 6;
        stat.AttackPower += 6;
        skill.SkillPoint += 5;
        stat.Exp = 0;
        if(stat.Level - 1 < levelUpExpData.expValues.Length)
            stat.MaxExp = levelUpExpData.expValues[stat.Level - 1];
        else
            stat.MaxExp = levelUpExpData.expValues[stat.Level - 1];
        PoolManager.Instance.Get(9).transform.position = transform.position - new Vector3(0,0.25f);
        playerUI.SkillUIUpdate();
    }

    // 피해 입을시
    public void GetDamage(int damage, GameObject who = null, bool stackable = false)
    {
        if (!die)
        {

            stat.HP -= damage;
            playerUI.PlayerUIUpdate();

            StartCoroutine("BeShadowing");
            PopUpDamageText(damage, stackable);

            if (stat.HP <= 0)
                Die(who);
        }
    }

    public void GetHeal(int value)
    {
        if (!die)
        {
            var lostHp = (stat.MaxHp - stat.HP);
            float healValue = 0;
            if (lostHp < value)
            {
                healValue = lostHp;
                stat.HP = stat.MaxHp;
            }
            else
            {
                healValue = value;
                stat.HP += value;
            }
            if((int)healValue != 0)
                PopUpHealText(healValue, false);
            playerUI.PlayerUIUpdate();
        }
    }
    public void GetAirborne(Vector2 force)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    // 데미지 수치 UI 띄우기
    void PopUpDamageText(float damage, bool stackable, bool critical = false)
    {
        if (nowDamageEffect && stackable)
        {
            nowDamageEffect.GetComponentInChildren<Text>().text = (int)damage + "\n" + nowDamageEffect.GetComponentInChildren<Text>().text;
            nowDamageEffect.transform.position += new Vector3(0, 0.25f);
        }
        else
        {
            var UI = critical ? CriticalDamageText : DamageText;
            var newEffect = Instantiate(DamageText, transform.localPosition, Quaternion.identity);
            var spawnPos = transform.localPosition + new Vector3(0, GetComponent<Collider2D>().bounds.size.y);
            newEffect.GetComponentInChildren<DamageUI>().Spawn((int)damage, spawnPos);
            nowDamageEffect = newEffect;
            //Debug.Log(nowDamageEffect.GetComponentInChildren<Text>().text);
        }
    }

    void PopUpHealText(float damage, bool stackable)
    {
        if (nowDamageEffect && stackable)
        {
            nowDamageEffect.GetComponentInChildren<Text>().text = (int)damage + "\n" + nowDamageEffect.GetComponentInChildren<Text>().text;
            nowDamageEffect.transform.position += new Vector3(0, 0.25f);
        }
        else
        {
            var newEffect = Instantiate(HealText, transform.localPosition, Quaternion.identity);
            var spawnPos = transform.localPosition + new Vector3(0, GetComponent<Collider2D>().bounds.size.y);
            newEffect.GetComponentInChildren<DamageUI>().Spawn((int)damage, spawnPos);
            nowDamageEffect = newEffect;
            //Debug.Log(nowDamageEffect.GetComponentInChildren<Text>().text);
        }
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
    void Die(GameObject who = null)
    {
        stat.Exp = 0;
        stat.HP = stat.MaxHp;
        playerUI.PlayerUIUpdate();

        if (who != null)
        {
            if (who.GetComponent<Monster>())
                GameManager.Instance.UIManager.SetDieMessage(who.GetComponent<Monster>().playerKillMessage);
        }
        playerUI.PopupPlayerDieUI();
        die = true;
        GameManager.Instance.Player = this;
        gameObject.SetActive(false);
    }



    float CC_SustainTime = 0;
    List<GameObject> ccObjects = new List<GameObject>();

    public void CC(GameObject CCobject, float damage, float time)
    {
        CC_SustainTime = time;
        if (!GetComponent<Move>().Movable)
        {
            GetDamage((int)damage / 3, null);
            return;
        }
        GetComponent<Move>().Movable = false;
        GameObject Trap = Instantiate(CCobject, transform.position, Quaternion.identity);
        Trap.transform.SetParent(transform);
        ccObjects.Add(Trap);

        if (gameObject.activeSelf)
            StartCoroutine("CCclear",Trap);
    }
    IEnumerator CCclear(GameObject CCObject)
    {
        var rb = GetComponent<Rigidbody2D>();
        while (CC_SustainTime >= 0) {
            rb.velocity= new Vector2(0, rb.velocity.y);
            CC_SustainTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        GetComponent<Move>().Movable = true;
        CCObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine("CCclear");
        foreach (var n in ccObjects)
        {
            n.SetActive(false);
        }
        GetComponent<Move>().Movable = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            LevelUp();
            playerUI.PlayerUIUpdate();
        }

    }
}