using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    
    UIManager officialUI;

    AudioSource audioSource;
    EffectAudioManager audioManager;

    Status stat;
    Player player;

    public GameObject Fire;                 // ��ų Q [default] ����ü
    public GameObject TeleportCalculator;   // ��ų E [iron] �ڷ���Ʈ��ġ �˻� ����ü
    public GameObject Skill_iron_Effect;    // ��ų E [iron] ������ ����(������) ����Ʈ

    public bool isMumchit; // ��ĩ                 // Skill, public

    int basicAttackSequence = 0;            // ��ų Q [iron] �⺻���� ��� ������ 

    string KeyReservation;                  // ��ų ���� (�������� ������ ������� ��� �� ����)

    GameObject PlayerAttackBox;
    GameObject PlayerAttackRange;           // Player ���� ����


    Coroutine BuffCoroutine;
    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();


        audioSource = GetComponent<AudioSource>();
        audioManager = GetComponent<EffectAudioManager>();


        stat = GetComponent<Status>();
        player = GetComponent<Player>();

        PlayerAttackBox = gameObject.transform.GetChild(2).gameObject;
        

        if (Fire == null)
            Fire = Resources.Load("Prefab/Fire") as GameObject;
        if (TeleportCalculator == null)
            TeleportCalculator = Resources.Load("Prefab/TeleportCalculator") as GameObject;
        if (Skill_iron_Effect == null)
            Skill_iron_Effect = Resources.Load("Prefab/Player_iron_Skill1Effect") as GameObject;
        
    }
    private void Update()
    {
        AttackBoxDirectionAsync();
        if (isMumchit || officialUI.isStoryTelling)
        {
            if (Input.inputString != "")
                KeyReservation = Input.inputString;
        }
        else
            PlayerKeyboardInput();
    }

    // Player ���ݹ��� ���� ����
    void AttackBoxDirectionAsync()
    {
        PlayerAttackBox.GetComponentInChildren<Transform>().localScale = sr.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
    }

    // Player Ű���� �Է� (��ų)
    void PlayerKeyboardInput()
    {
        // Q ��ų [�⺻����] (�ҵ��� �߻� ���..)
        if (Input.GetKeyDown(KeyCode.Q) || (KeyReservation == "q" || KeyReservation == "Q"))
        {
            if (KeyReservation != null)
                KeyReservation = null;
            anim.SetTrigger("BasicAttack");
            switch (player.ChangeMode)
            {
                case "default":
                    if (KeyReservation != null)
                        StartCoroutine("GetMumchit", 0.3f);
                    else
                        StartCoroutine("GetMumchit", 0.17f);
                    Vector3 SpawnPosition;
                    if (sr.flipX)
                        SpawnPosition = tr.position + new Vector3(-1f, 0);
                    else
                        SpawnPosition = tr.position + new Vector3(1f, 0);
                    anim.SetTrigger("BasicAttack");
                    GameObject fire = Instantiate(Fire, SpawnPosition, Quaternion.identity);
                    fire.GetComponent<Fire>().Damage = stat.AttackPower;
                    break;
                case "iron":
                    audioSource.clip = audioManager.Esound; // ȿ���� ����
                    audioSource.pitch = 3.0f;
                    audioSource.volume = 0.43f;
                    audioSource.Play();                     // ȿ���� ���
                    if (sr.flipX)
                        rb.transform.Translate(new Vector3(-0.3f, 0));
                    else
                        rb.transform.Translate(new Vector3(0.3f, 0));
                    if (KeyReservation != null)
                        StartCoroutine("GetMumchit", 0.4f + 0.05 * basicAttackSequence);
                    else
                        StartCoroutine("GetMumchit", 0.2f + 0.15 * basicAttackSequence);
                    ChangeHitbox("default");
                    anim.SetInteger("Iron_BasicSequence", basicAttackSequence);
                    if (basicAttackSequence < 2)
                    {
                        StartCoroutine(DamageDelay(GetRandomDamageValue(stat.AttackPower * (1 + basicAttackSequence), 0.8f, 1.2f), 0.04f));
                        basicAttackSequence++;
                    }
                    else
                    {
                        StartCoroutine(DamageDelay(GetRandomDamageValue(stat.AttackPower * 3, 0.9f, 1.3f), 0.04f, null, "Airborne"));
                        basicAttackSequence = 0;
                    }
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) || (KeyReservation == "e" || KeyReservation == "E"))
        {
            if (KeyReservation != null)
                KeyReservation = null;
            switch (player.ChangeMode)
            {
                case "iron":
                    audioSource.clip = audioManager.Esound; // ȿ���� ����
                    audioSource.pitch = 1.0f;
                    audioSource.volume = 1.0f;
                    audioSource.Play();                     // ȿ���� ���
                    Color None = player.originColor;
                    None.a = 0;
                    sr.color = None;
                    StartCoroutine("GetMumchit", 0.3f);
                    ChangeHitbox("ironSkill1");
                    DamageAllinHitBox(GetRandomDamageValue(stat.AttackPower * 4, 1.0f, 1.5f), Skill_iron_Effect);
                    if (player.GetComponent<Move>().isWall)
                        TeleportByCalcul(tr.position);
                    else
                        Instantiate(TeleportCalculator, tr.position, Quaternion.identity);
                    anim.SetTrigger("Skill1");
                    if (basicAttackSequence == 0)
                        basicAttackSequence = 1;
                    if (BuffCoroutine != null)
                        StopCoroutine(BuffCoroutine);
                    BuffCoroutine = StartCoroutine(Buff("Speed", 7, 0.5f));

                    break;
            }
        }
    }
    // �������κ��� ���� ������ ���� (* ��� / ex 0.8�� ~ 1.2��)
    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    // Player ���ݹ��� ���� ������ ��� ����������
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
    // ��ų ���� ��� ��ĩ, (��ų ��� �� ������ �Ұ� ����)
    IEnumerator GetMumchit(float time)
    {
        isMumchit = true;
        yield return new WaitForSeconds(time);
        isMumchit = false;
    }

    // Player ���� ���� ���� (��Ʈ �ڽ� ��ü)
    void ChangeHitbox(string attacktype)
    {
        switch (attacktype)
        {
            case "default":
                PlayerAttackRange = PlayerAttackBox.transform.GetChild(0).GetChild(0).gameObject;
                break;
            case "ironSkill1":
                PlayerAttackRange = PlayerAttackBox.transform.GetChild(0).GetChild(1).gameObject;
                break;
        }
    }
    // ��ų E �� ���� �����̵�
    public void TeleportByCalcul(Vector2 pos)
    {
        rb.position = pos;
        Invoke("Appear", 0.1f);
    }
    public void Appear()
    {
        sr.color = player.originColor;
    }
    // ���� ���� (���� �̵��ӵ��� �߰�)
    IEnumerator Buff(string what, float how, float time)
    {
        switch (what)
        {
            case "Speed":
                stat.MoveSpeed = stat.BasicSpeed + how;
                break;
        }
        yield return new WaitForSeconds(time);
        stat.MoveSpeed = stat.BasicSpeed;
    }

    // ������ ���� �� ������. (��ų �ִϸ��̼ǰ� ��ũ�� ���߱� ����) 

    IEnumerator DamageDelay(int damage, float time, GameObject Effect = null, string CCtype = null)
    {

        yield return new WaitForSeconds(time);
        DamageAllinHitBox(damage, Effect, CCtype);
    }

    public void SceneChangeUIChanged()
    {
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();
    }
}
