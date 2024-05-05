using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Monster : MonoBehaviour
{
    protected Status stat;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    protected Collider2D col;

    [SerializeField] LevelUpExpData levelUpExpData;

    protected Color originColor;

    GameObject HPUI;
    Image HPbar;
    GameObject DamageText;
    GameObject CriticalDamageText;

    Coroutine AppearHPUICoroutine;

    protected Coroutine ActCoroutine;

    public UnityEvent hitEvent;
    public UnityEvent dieEvent;


    //
    protected GameObject AttackTarget = null;
    protected int Direction = 1;

    protected bool isActing;
    protected bool isHitStunned;
    protected bool isMumchit;
    public bool binded = false;

    protected Coroutine ProceedingCoroutine;
    Coroutine HittedCoroutine;

    // Attackable Enemy Layer
    [SerializeField] protected LayerMask enemyLayer;

    public string HittedAndStartStory;
    public string DieAndStartStory;
    public List<string> CheckCondition;
    public string IfIDieCheckAcheivePlease;

    protected const float MinActionTime = 1f;
    protected const float MaxActionTime = 3f;

    protected List<GameObject> Monsters = new List<GameObject>();
    protected bool isDamagedRecent = false;

    [SerializeField] public bool FixedType = false;
    [SerializeField] protected float DamageReceivePercent = 1f;

    public bool isGround;

    public bool die = false;
    public bool isTransparentObject = true;
    public bool isHittableObject = true;

    public GameObject Body; // 본체

    private GameObject nowDamageEffect;

    [Multiline(3)]
    public string playerKillMessage;

    private float firstExp;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        originColor = sr.color;

        if (Body == null) Body = gameObject;

        Init();

        stat = GetComponent<Status>();

        firstExp = stat.Exp;

        // UI 관련
        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;
        if(CriticalDamageText == null)
            CriticalDamageText = Resources.Load("Prefab/CriticalDamageUI") as GameObject;
    }
    private void OnEnable()
    {
        die = false;
        sr.color = originColor;

        stat.HP = stat.MaxHp;
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI 업데이트

        if (isTransparentObject)
            StartCoroutine(BeTransparent(1));
        ActCoroutine = StartCoroutine("Act");

        OnEnableInit();
    }
    private void FixedUpdate()
    {
        if (!isHitStunned)
        {
            if (ActCoroutine == null) ActCoroutine = StartCoroutine("Act");
        }
    }
    protected virtual void Init() { }
    protected virtual void OnEnableInit() 
    { 
        stat.Exp = firstExp;
        AttackTarget = null;
        isHitStunned = false;
        isMumchit = false;
        //binded = false;
        isGround = true;
        isActing = false;
        stat.MoveSpeed = stat.BasicSpeed;
        ActCoroutine = StartCoroutine("Act");
    }

    protected virtual IEnumerator Act()
    {
        yield return null;
        ActCoroutine = StartCoroutine("Act");
    }
    public virtual void GetDamaged(float damage, GameObject Fromwho, bool stackable = false, bool isCritical = false)
    {
        if (Fromwho != null)
        {
            AttackTarget = Fromwho;
            if (!FixedType && damage > (stat.MaxHp * 0.1f))
                Knockback();
        }
        float trueDamage = 0;
        int levelGap = stat.Level <= Fromwho.GetComponent<Status>().Level ? 0 : stat.Level - Fromwho.GetComponent<Status>().Level;

        trueDamage = (damage * (DamageReceivePercent - (0.01f * levelGap)));

        stat.HP -= trueDamage;                          // 체력감소
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI 업데이트
        
        PopUpDamageText(trueDamage, stackable, isCritical);

        try
        {
            if (AppearHPUICoroutine != null)
                StopCoroutine(AppearHPUICoroutine);
            AppearHPUICoroutine = StartCoroutine("AppearHPUI");     // HP UI 보이기

            if (ProceedingCoroutine != null)
                StopCoroutine(ProceedingCoroutine);
            ProceedingCoroutine = StartCoroutine("SetActingTrue", 10f);
        }
        catch { }

        hitEvent.Invoke();


        if (stat.HP <= 0)                           // 체력 다달면
        {
            Die(Fromwho);
        }
    }

    public virtual void GetDamaged(float damage)
    {
        float trueDamage = 0;
        trueDamage = (damage * (DamageReceivePercent));
        stat.HP -= trueDamage;                          // 체력감소
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI 업데이트

        PopUpDamageText(trueDamage, false, false);

        try
        {
            if (AppearHPUICoroutine != null)
                StopCoroutine(AppearHPUICoroutine);
            AppearHPUICoroutine = StartCoroutine("AppearHPUI");     // HP UI 보이기

            if (ProceedingCoroutine != null)
                StopCoroutine(ProceedingCoroutine);
            ProceedingCoroutine = StartCoroutine("SetActingTrue", 10f);
        }
        catch { }

        hitEvent.Invoke();


        if (stat.HP <= 0)                           // 체력 다달면
        {
            Die(null);
        }
    }

    protected virtual void Knockback()
    {
        sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정
        ChangeDirection();

        rb.velocity = new Vector2(0.8f * -Direction, rb.velocity.y);
        // 애니메이션
        anim.SetTrigger("Hitted");

        if (HittedCoroutine != null)
            StopCoroutine(HittedCoroutine);
        HittedCoroutine = StartCoroutine("GetHittedStun", 0.5f);
    }
    protected virtual void Die(GameObject Fromwho)
    {
        if (die) return;

        die = true;
        if(!FixedType)
            rb.velocity = Vector2.zero;
        dieEvent.Invoke();
        if (Fromwho == null) { }
        else if (Fromwho.GetComponent<Player>())
        {
            var exp = stat.Exp;
            //if (levelUpExpData && stat.Level - 2 > 0) exp += levelUpExpData.expValues[stat.Level - 2];
            AttackTarget.GetComponent<Player>().GetExp(exp);
        }
        else if (Fromwho.GetComponent<Monster>())
        {
            var exp = stat.Exp;
            //if (levelUpExpData && stat.Level - 2 > 0) exp += levelUpExpData.expValues[stat.Level - 2];
            AttackTarget.GetComponent<Monster>().GetExp(exp);
        }

        if (isTransparentObject)
            StartCoroutine(BeTransparent(-1));
        else
            gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (die) return;
        if (col.tag == gameObject.tag) return;
        if (col.GetComponent<TrapDamage>()) return;

        var monster = col.GetComponent<Monster>();
        if (monster)
        {
            if (!monster.isHittableObject) return;
            if (monster.Body == gameObject) return;
            if (monster.Body.tag == gameObject.tag) return;
            if (monster.die) return;

            if(col.tag != gameObject.tag)
                monster.GetDamaged(GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f), Body);

        }

        if (col.tag == "PlayerHitbox")
        {
            col.GetComponent<CrashHitbox>().ContactHitByMonster(Body, GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f));
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "PlayerHitbox")
        {
            col.GetComponent<CrashHitbox>().ContactOutByMonster(Body);
        }
    }

    // Event Scenario Monster
    public virtual void HitEvent()
    {
        if (AttackTarget.tag == "Player" && HittedAndStartStory != "" && HittedAndStartStory != null)
        {
            bool Checking;
            Checking = true;


            if (Checking)
            {
                GameManager.Instance.StoryManager.StartScenario(HittedAndStartStory);
                HittedAndStartStory = "";
            }
        }
    }
    public virtual void DieEvent()
    {
        if (DieAndStartStory != "" && DieAndStartStory != null)
            GameManager.Instance.StoryManager.StartScenario(DieAndStartStory);
    }

    public void GetExp(float value)
    {
        stat.Exp += value;
        if (stat.Exp > stat.MaxExp)
        {
            var excess = stat.Exp - stat.MaxExp;
            LevelUp();
            stat.Exp = excess;
        }
    }

    public void SetAttackTarget(GameObject target)
    {
        AttackTarget = target;
        if (ProceedingCoroutine != null)
            StopCoroutine(ProceedingCoroutine);
        if(gameObject.activeSelf)
            ProceedingCoroutine = StartCoroutine("SetActingTrue", 300f);
    }
    // Damage
    protected virtual void LevelUp()
    {
        stat.Level += 1;
        stat.MaxHp += 60;
        stat.HP = stat.MaxHp;
        stat.BasicAttackPower += 6;
        stat.AttackPower += 6;
        if (levelUpExpData)
            stat.MaxExp = levelUpExpData.expValues[stat.Level - 1];
        else
            stat.MaxExp = stat.Exp * 10;
        stat.Exp = 0;
        PoolManager.Instance.Get(9).transform.position = transform.position;
    }
    // 랜덤 숫자 발급, Ontrigger의 GetDamage에 쓰임
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }


    protected void ChangeDirection()
    {
        // flipX에 따른 방향 설정(1/-1)
        Direction = sr.flipX ? -1 : 1;
    }

    IEnumerator BeTransparent(int n = 1)
    {
        Color color = sr.color;
        sr.color = color;
        if (n == -1)
        {
            color.a = originColor.a;
            while (color.a >= 0)
            {
                color.a -= ((Time.deltaTime / 0.5f));
                sr.color = color;
                yield return null;
            }
        }
        else if(n == 1)
        {
            color.a = 0;
            while (color.a <= originColor.a)
            {
                color.a += ((Time.deltaTime / 0.5f));
                sr.color = color;
                yield return null;
            }
        }
        if (n == -1)
        {
            gameObject.SetActive(false);
            sr.color = originColor;
        }
    }

    // CC

    protected IEnumerator GetMumchit(float time)
    {
        isMumchit = true;
        yield return new WaitForSeconds(time);
        isMumchit = false;
    }

    IEnumerator SetActingTrue(float time)
    {
        isActing = true;
        yield return new WaitForSeconds(time);
        isActing = false;
    }

    IEnumerator GetHittedStun(float time)
    {
        isHitStunned = true;
        stat.MoveSpeed = 0.01f;
        yield return new WaitForSeconds(time);
        isHitStunned = false;
        stat.MoveSpeed = stat.BasicSpeed;
    }


    public void GetAirborne(Vector2 force)
    {
        if (!rb) return;
        isGround = false;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }



    // UI


    // 체력바 UI 6초간 보이기
    IEnumerator AppearHPUI()
    {
        HPUI.SetActive(true);
        yield return new WaitForSeconds(6f);
        HPUI.SetActive(false);
    }

    void PopUpDamageText(float damage, bool stackable = false, bool critical = false)
    {
        if (nowDamageEffect && stackable)
        {
            nowDamageEffect.GetComponentInChildren<Text>().text = (int)damage + "\n" + nowDamageEffect.GetComponentInChildren<Text>().text;
            nowDamageEffect.transform.position += new Vector3(0, 0.25f);
        }
        else
        {
            var UI = critical ? CriticalDamageText : DamageText;
            var newEffect = Instantiate(UI, transform.localPosition, Quaternion.identity);
            var spawnPos = transform.localPosition + new Vector3(0, GetComponent<Collider2D>().bounds.size.y);
            newEffect.GetComponentInChildren<DamageUI>().Spawn((int)damage, spawnPos);
            nowDamageEffect = newEffect;
            //Debug.Log(nowDamageEffect.GetComponentInChildren<Text>().text);
        }
    }

    float CC_SustainTime = 0;
    List<GameObject> ccObjects = new List<GameObject>();
    public void CC(GameObject CCobject, float damage, float time)
    {
        CC_SustainTime = time;
        if (binded)
            return;
        binded = true;
        GameObject Trap = PoolManager.Instance.Get(3);
        Trap.transform.position = transform.position;
        Trap.transform.SetParent(transform);
        ccObjects.Add(Trap);

        if(gameObject.activeSelf)
            StartCoroutine("CCclear",Trap);
    }
    IEnumerator CCclear(GameObject ccObject)
    {
        var rb = GetComponent<Rigidbody2D>();
        while (CC_SustainTime >= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            CC_SustainTime -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        binded = false;
        ccObject.SetActive(false);
    }
    private void OnDisable()
    {
        StopCoroutine("CCclear");
        CC_SustainTime = 0;
        sr.color = originColor;
        foreach(var n in ccObjects)
        {
            if(n == null) continue;
            if(n.activeSelf)
                n.SetActive(false);
        }
        binded = false;
        HPUI.SetActive(false);
    }
}
