using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI2 : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        Jump
    }
    State ActState;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    CapsuleCollider2D cc;

    GameObject HPUI;
    Image HPbar;
    public GameObject DamageText;


    Status stat;

    int Direction = 1;
    bool isGround;
    bool isJumpRecent; // ���� �ʹ� �����ؼ� �����Ϸ���



    Coroutine CheckingTopographyCoroutine;
    Coroutine AppearHPUICoroutine;
    Coroutine ChaseCoroutine;
    Coroutine HittedStunCoroutine;
    Coroutine ActCoroutine;
    

    GameObject AttackTarget;


    void Start()
    {
        // �⺻ �ʱ�ȭ
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();

        // UI ����
        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;


        // ����� �ʱ�ȭ
        stat = GetComponent<Status>();

        // ����
        ActCoroutine = StartCoroutine("Act");
        //StartCoroutine("CheckingTopography");
    }

    
    IEnumerator CheckingTopography()
    {
        // ���α��� �ֺ� �����ľ� ���� ������ üũ
        RaycastHit2D rayFrontGroundCheck, rayFrontWallCheck, rayUnderGroundCheck, rayFloorGroundCheck;
        while (true)
        {

            //������ �ð�ȿ�� Ȱ��ȭ
            //��
            Debug.DrawRay(transform.position + new Vector3(Direction, 0, 0), new Vector3(0, 1, 0), new Color(0, 1, 0));
            //�ٴ�
            Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * ((cc.size.y / 2) + 0.15f), new Color(0, 1, 0));

            rayFrontGroundCheck = Physics2D.Raycast(transform.position + new Vector3(Direction, 0, 0), Vector2.up, 1f, 1 << LayerMask.NameToLayer("Ground"));
            rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f, 1 << LayerMask.NameToLayer("UnPassableWall"));
            rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), (cc.size.y / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
            rayFloorGroundCheck = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0), (cc.size.y / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
            // �տ� ���� �ִٸ� ������ ����
            if (isGround && rayFrontGroundCheck.collider != null || (rayFloorGroundCheck.collider == null && isGround))
                rb.velocity = new Vector2(stat.MoveSpeed * Direction, stat.JumpPower);

            // �տ� ���� �ִٸ� ������ȯ
            if (isGround && rayFrontWallCheck.collider != null)
            {
                sr.flipX = !sr.flipX;
                Direction *= -1;
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }

            // �ٴ� üũ�� isGround ��ȯ
            isGround = (rayUnderGroundCheck.collider == null) ? false : true;

            yield return null;
        }
    }

    // ���� Ȱ��
    IEnumerator Act()
    {
        int Actnum = isJumpRecent ? Random.Range(1, 2) : Random.Range(0, 3);
        if (!isGround)
            Actnum = 1;
        // �ൿ ����
        switch (Actnum)
        {
            case 0:
                ActState = State.Idle;
                break;
            case 1:
                ActState = State.Move;
                break;
            case 2:
                ActState = State.Jump;
                break;
        }

        if (isJumpRecent)
            isJumpRecent = false;

        // ����üũ �ڷ�ƾ ����
        if(CheckingTopographyCoroutine != null)
            StopCoroutine("CheckingTopography");
        if (ActState != State.Idle)
            StartCoroutine("CheckingTopography");

        switch (ActState)
        {
            case State.Idle:

                anim.SetBool("Idle", true);
                anim.SetBool("Walk", false);

                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case State.Move:
                if (isGround) // ���� �ƴҶ��� ����ٲٰ� (�ڲ� ���� ���� ����ٲ㼭 �糪������)
                {
                    Direction = (bool)(Random.value > 0.5f) ? 1 : -1;
                    sr.flipX = Direction == 1 ? false : true;
                }
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);

                rb.velocity = new Vector2(Direction * stat.MoveSpeed, rb.velocity.y);
                break;
            case State.Jump:
                if (isGround)
                    rb.velocity = new Vector2(rb.velocity.x, stat.JumpPower);
                isJumpRecent = true;
                ActCoroutine = StartCoroutine("Act");
                yield break;
                //break;
        }
        yield return new WaitForSeconds(Random.Range(2, 3)); // Ȱ�� �ð�
        ActCoroutine = StartCoroutine("Act");
    }

    IEnumerator ChaseEnemy()
    {
        if (CheckingTopographyCoroutine != null)
            StopCoroutine(CheckingTopographyCoroutine);
        CheckingTopographyCoroutine = StartCoroutine("CheckingTopography");
        while (true) {
            if(AttackTarget == null)
            {
                ActCoroutine = StartCoroutine("Act");
                StopCoroutine(CheckingTopographyCoroutine);
                yield break;
            }


            if (isGround)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (isGround)
            {
                sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // ���� ����
                Direction = sr.flipX ? -1 : 1;

                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);

                rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
            }

            float distanceXGap = Mathf.Abs(transform.position.x - AttackTarget.transform.position.x);
            if (distanceXGap < 2f) // ����ﶧ ���� 
            {
                float dinstanceYGap = transform.position.y - AttackTarget.transform.position.y;
                if (Mathf.Abs(dinstanceYGap) > 0.5f && dinstanceYGap < 0) // ������ ���� ��ġ�ϸ� ����
                {
                    if (isGround)
                        rb.velocity = new Vector2(rb.velocity.x, 7);
                }

                // ��������
                //float AttackableDistance;
                //switch (MyEnemyType)
                //{
                //    case "Warrior":
                //        AttackableDistance = 1f;
                //        if (distanceXGap < AttackableDistance)
                //        {
                //            anim.SetTrigger("Attack");
                //            StartCoroutine("GetMumchit", 0.5f);
                //            Attack();
                //        }
                //        break;
                //    default:
                //        break;
                //}

            }
            yield return null;
        }
    }

    // Trigger ���۽�
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            GetDamaged(GetRandomDamageValue(col.GetComponent<Fire>().Damage, 0.8f, 1.2f), GameObject.FindGameObjectWithTag("Player"));
            Destroy(col.gameObject);
        }
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

        

        if (HittedStunCoroutine != null)
            StopCoroutine(HittedStunCoroutine);
        HittedStunCoroutine = StartCoroutine("HittedStunned", Fromwho);
 



        if (stat.HP <= 0)                           // ü�� �ٴ޸�
        {
            Die(Fromwho);
        }
    }

    IEnumerator HittedStunned(GameObject Fromwho)
    {
        anim.SetTrigger("Hitted");

        // Ÿ�� ����
        if (Fromwho.tag != "trap")
        {
            AttackTarget = Fromwho;

            if (ChaseCoroutine != null)             // ���� ���� ����
                StopCoroutine(ChaseCoroutine);
            if (ActCoroutine != null)               // �⺻ Ȱ�� ����
                StopCoroutine(ActCoroutine);

            // �˹�
            sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true;
            Direction = sr.flipX ? -1 : 1;
            rb.velocity = new Vector2(-0.2f * Direction, rb.velocity.y);
            yield return new WaitForSeconds(0.5f);  // ����

            ChaseCoroutine = StartCoroutine("ChaseEnemy"); // �i�� ����

            yield return new WaitForSeconds(10f); // 10�� �� ��׷� Ǯ��
            StopCoroutine(ChaseCoroutine);

            // �⺻ Ȱ�� �簳
            if (ActCoroutine != null)
                StopCoroutine(ActCoroutine);
            ActCoroutine = StartCoroutine("Act");
        }
        else
        {
            if (ChaseCoroutine != null)             // ���� ���� ����
                StopCoroutine(ChaseCoroutine);
            if (ActCoroutine != null)               // �⺻ Ȱ�� ����
                StopCoroutine(ActCoroutine);

            yield return new WaitForSeconds(0.5f);  // ����
            // �⺻ Ȱ�� �簳
            if (ActCoroutine != null)
                StopCoroutine(ActCoroutine);
            ActCoroutine = StartCoroutine("Act");
        }
    }

    void Die(GameObject Fromwho)
    {
        //if(Fromwho != null){
        //      Fromwho.GetComponent<Status>().
        //}
        Destroy(gameObject);
    }

    // ���� ���� �߱�, Ontrigger�� GetDamage�� ����
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
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

    
}
