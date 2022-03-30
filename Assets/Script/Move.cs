using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Move : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Status stat;
    bool isJumping;
    int JumpCount = 2;
    public GameObject Fire;
    public GameObject TeleportCalculator;
    public GameObject Skill_iron_Effect;
    string ChangeMode;
    bool isMumchit; // 멈칫

    int basicAttackSequence = 0;

    Coroutine ProcessingCoroutine;
    
    GameObject PlayerAttackRange;
    BoxCollider2D boxsize;



    Image HPbar;
    Color originColor;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();

        PlayerAttackRange = gameObject.transform.GetChild(3).gameObject;
        boxsize = PlayerAttackRange.GetComponent<BoxCollider2D>();

        HPbar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        originColor = sr.color;

        if (Fire == null)
            Fire = Resources.Load("Prefab/Fire") as GameObject;
        if (TeleportCalculator == null)
            TeleportCalculator = Resources.Load("Prefab/TeleportCalculator") as GameObject;
        if (Skill_iron_Effect == null)
            Skill_iron_Effect = Resources.Load("Prefab/Player_iron_Skill1Effect") as GameObject;
        ChangeDragon("default");

    }

    // Update is called once per frame
    void Update()
    {
        AttackBoxDirectionAsync();
        if (!isMumchit)
            PlayerKeyboardInput();
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
    void PlayerKeyboardInput()
    {
        // 키보드 입력!1
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float direction = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * stat.MoveSpeed, rb.velocity.y);
            if(direction == -1)
                sr.flipX = true;
            else if(direction == 1)
                sr.flipX = false;
                
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= stat.MoveSpeed)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("Idle", true);
                anim.SetBool("Walk", false);
            }
        }


        // 점프!!
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (JumpCount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                JumpCount--;
            }
        }

        // 제자리!!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector2(0, 0);
        }

        // 기본공격(불덩이 발사 OR 펀치)!
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("BasicAttack");
            switch (ChangeMode)
            {
                case "default":
                    StartCoroutine("GetMumchit", 0.3f);
                    Vector3 SpawnPosition;
                    if (sr.flipX)
                        SpawnPosition = tr.position + new Vector3(-1f, 0);
                    else
                        SpawnPosition = tr.position + new Vector3(1f, 0);
                    anim.SetTrigger("BasicAttack");
                    Instantiate(Fire, SpawnPosition, Quaternion.identity);
                    break;
                case "iron":
                    StartCoroutine("GetMumchit", 0.4f + 0.05 * basicAttackSequence);
                    ChangeHitbox("default");
                    //DamageAllinHitBox(stat.AttackPower * 2);
                    anim.SetInteger("Iron_BasicSequence", basicAttackSequence);
                    if (basicAttackSequence < 2)
                    {
                        StartCoroutine(DamageDelay(stat.AttackPower * (1 + basicAttackSequence), 0.04f));
                        basicAttackSequence++;
                    }
                    else // 마지막타격
                    {
                        StartCoroutine(DamageDelay(stat.AttackPower * 3, 0.04f, null, "Airborne"));
                        basicAttackSequence = 0;
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (ChangeMode)
            {
                case "iron":
                    StartCoroutine("GetMumchit", 0.3f);
                    ChangeHitbox("ironSkill1");
                    DamageAllinHitBox(stat.AttackPower * 4, Skill_iron_Effect);
                    Instantiate(TeleportCalculator, tr.position, Quaternion.identity);
                    anim.SetTrigger("Skill1");
                    if (basicAttackSequence == 0)
                        basicAttackSequence = 1;
                    StartCoroutine(Buff("Speed", 4, 3f));
                    break;
            }
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeDragon("iron");
            transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        }
    }

    private void DamageAllinHitBox(int damage, GameObject effect = null, string CCtype = null)
    {
        List<GameObject> Enemys = PlayerAttackRange.GetComponent<AttackBox>().GetAttackableTargets();
        for (int i = 0; i < Enemys.Count; i++)
            if (Enemys[i] != null)
            {
                GameObject Spawnedeffect;
                if (effect != null)
                {
                    Spawnedeffect = Instantiate(effect, Enemys[i].transform.position, Quaternion.identity);
                    Spawnedeffect.transform.SetParent(Enemys[i].transform);
                }

                EnemyAI ai = Enemys[i].GetComponent<EnemyAI>();
                if (ai != null)
                    ai.GetDamaged(damage, gameObject);

                if (CCtype != null)
                    switch (CCtype)
                    {
                        case "Airborne":
                            int dir = sr.flipX ? -1 : 1;
                            ai.GetAirborne(new Vector2(dir * 5f, 10f));
                            break;
                    }
            }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name == "Ground")
        {
            if (JumpCount != 2)
                JumpCount = 2;
            anim.SetBool("Jump", false);
        }
    }
    void ChangeHitbox(string attacktype)
    {
        switch (attacktype) {
            case "default":
                PlayerAttackRange = transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
                break;
            case "ironSkill1":
                PlayerAttackRange = transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
                break;
        }
    }
    void ChangeDragon(string dragontype)
    {
        ChangeMode = dragontype;
        switch (dragontype)
        {
            case "default":
                anim.SetInteger("ChangeMode", 0);
                break;
            case "iron":
                anim.SetInteger("ChangeMode", 1);
                anim.SetTrigger("Change");
                break;
        }
    }
    void AttackBoxDirectionAsync()
    {
        transform.GetChild(3).GetComponentInChildren<Transform>().localScale = sr.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
    }
    public void TeleportByCalcul(Vector2 pos)
    {
        rb.position = pos;
    }
    IEnumerator GetMumchit(float time)
    {
        isMumchit = true;
        yield return new WaitForSeconds(time);
        isMumchit = false;
    }

    IEnumerator Buff(string what, float how, float time)
    {
        Debug.Log("코루틴");
        switch (what)
        {
            case "Speed":
                stat.MoveSpeed += how;
                break;
        }
        yield return new WaitForSeconds(time);
        stat.MoveSpeed -= how;
    }

    IEnumerator DamageDelay(int damage, float time, GameObject Effect = null, string CCtype = null)
    {

        yield return new WaitForSeconds(time);
        DamageAllinHitBox(damage, Effect, CCtype);
    }

    public void GetDamage(int damage)
    {
        stat.HP -= damage;
        HPbar.fillAmount = stat.HP / stat.MaxHp;
        StartCoroutine("BeShadowing");
    }

    IEnumerator BeShadowing()
    {
        Color color = sr.color;
        
        color.a = sr.color.a / 2;
        sr.color = color;
        for (int i = 0; i < 3; i++)
        {
            while (color.a >= 0.2f)
            {
                color.a -= (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
                sr.color = color;
                yield return null;
            }
            while (color.a <= 0.8f)
            {
                color.a += (Time.deltaTime / 0.5f);
                sr.color = color;
                yield return null;
            }
        }
        sr.color = originColor;
    }
}
