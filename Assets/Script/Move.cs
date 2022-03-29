using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    GameObject PlayerAttackRange;
    BoxCollider2D boxsize;
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
        UnderComponentDirectionAsync();
        PlayerKeyboardInput();

    }
    void PlayerKeyboardInput()
    {
        // 키보드 입력!1
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-stat.MoveSpeed, rb.velocity.y);
            sr.flipX = true;
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(stat.MoveSpeed, rb.velocity.y);
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
                    Vector3 SpawnPosition;
                    if (sr.flipX)
                        SpawnPosition = tr.position + new Vector3(-1f, 0);
                    else
                        SpawnPosition = tr.position + new Vector3(1f, 0);

                    Instantiate(Fire, SpawnPosition, Quaternion.identity);
                    break;
                case "iron":
                    ChangeHitboxSize(0.7f);
                    DamageAllinHitBox(stat.AttackPower * 2);
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (ChangeMode)
            {
                case "iron":
                    ChangeHitboxSize(2.0f);
                    DamageAllinHitBox(stat.AttackPower, Skill_iron_Effect);
                    Instantiate(TeleportCalculator, tr.position, Quaternion.identity);
                    anim.SetTrigger("Skill1");

                    break;
            }
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeDragon("iron");
        }
    }

    private void DamageAllinHitBox(int damage, GameObject effect = null)
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
    void ChangeHitboxSize(float size)
    {
        boxsize.offset = new Vector2(size, 0);
        boxsize.size = new Vector2(size * 2, 1);
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
    void UnderComponentDirectionAsync()
    {
        if (sr.flipX)
        {
            PlayerAttackRange.transform.localScale = new Vector2(-1 ,1);
        }
        else
        {
            PlayerAttackRange.transform.localScale = new Vector2(1, 1);
        }


    }
    public void TeleportByCalcul(Vector2 pos)
    {
        rb.position = pos;
    }
}
