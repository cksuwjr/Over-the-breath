using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    Status stat;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject AttackTarget = null;
    CapsuleCollider2D cc;


    GameObject HPUI;
    Image HPbar;
    public GameObject DamageText;

    Coroutine AppearHPCoroutine;
    Coroutine ProceedingCoroutine;
    Coroutine HittedCoroutine;

    bool isActing;
    bool isHitStunned;

    [SerializeField]
    public List<string> plus_stat;
    [SerializeField]
    public List<float> plus_value;

    enum State
    {
        Idle,
        Move,
        Jump,
        Change
    }

    State state;
    const int StateCount = 4;

    bool isGround;
    int Direction = 1;


    const float MinActionTime = 1f;
    const float MaxActionTime = 3f;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stat = GetComponent<Status>();
        cc = GetComponent<CapsuleCollider2D>();

        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;

        state = State.Idle;

    }


    void Update()
    {
        ChangeDirection();
        CheckingTopography();
        MovingPattern();
    }
    void ChangeDirection()
    {
        // flipX�� ���� ���� ����(1/-1)
        Direction = sr.flipX ? -1 : 1;
    }
    void CheckingTopography()
    {
        // ���α��� �ֺ� �����ľ� ���� ������ üũ
        RaycastHit2D rayFrontGroundCheck, rayFrontWallCheck, rayUnderGroundCheck, rayFloorGroundCheck;

        //    //������ �ð�ȿ�� Ȱ��ȭ
        //    //��
        //Debug.DrawRay(transform.position + new Vector3(Direction, 0, 0), new Vector3(0, 1, 0), new Color(0, 1, 0));
        //    //�ٴ�
        //Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * ((cc.size.y / 2) + 0.15f), new Color(0, 1, 0));

        rayFrontGroundCheck = Physics2D.Raycast(transform.position + new Vector3(Direction, 0, 0), Vector2.up, 1f, 1 << LayerMask.NameToLayer("Ground"));
        rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f, 1 << LayerMask.NameToLayer("UnPassableWall"));
        rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), (cc.size.y / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
        rayFloorGroundCheck = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0), (cc.size.y / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
        // �տ� ���� �ִٸ� ������ ����
        if (isGround && rayFrontGroundCheck.collider != null || (rayFloorGroundCheck.collider == null && isGround))
            rb.velocity = new Vector2(stat.MoveSpeed * Direction, stat.JumpPower);

        // �տ� ���� �ִٸ� ������ȯ
        if (isGround && rayFrontWallCheck.collider != null)
            sr.flipX = (sr.flipX) ? false : true;

        // �ٴ� üũ�� isGround ��ȯ
        isGround = (rayUnderGroundCheck.collider == null) ? false : true;



    }
    void MovingPattern()
    {
        // ���� ����� ���ٸ�
        if (AttackTarget == null && !isHitStunned)
        {
            if (!isActing)
                SetAction();
            switch (state)
            {
                // ����
                case State.Idle:
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walk", false);

                    break;

                // ������
                case State.Move:

                    rb.velocity = new Vector2(stat.MoveSpeed * Direction, rb.velocity.y);

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    break;
                // ����
                case State.Jump:
                    if (isGround)
                        rb.velocity = new Vector2(rb.velocity.x, stat.JumpPower);
                    SetAction();
                    break;
                // ���� ��ȯ
                case State.Change:
                    sr.flipX = (bool)(Random.value > 0.5f); // flipX�� �������� true false �ο�
                    SetAction(1, StateCount - 2);
                    break;
            }
        }
        else  // ���� ����� �ִٸ�
        {
            if (!isHitStunned && isGround) // �ǰݽð� ������
            {
                sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // ���� ����

                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);

                rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
            }

            if (!isHitStunned && Mathf.Abs(transform.position.x - AttackTarget.transform.position.x) < 2f) // ����ﶧ ���� ������ ���� ��ġ�ϸ� ����
            {
                float dinstanceGap = transform.position.y - AttackTarget.transform.position.y;
                if (Mathf.Abs(dinstanceGap) > 0.5f && dinstanceGap < 0 && isGround)
                    rb.velocity = new Vector2(rb.velocity.x, 7);
            }


            if (!isActing)
                AttackTarget = null;
        }
    }

    // ���� ���� �ο�  
    void SetAction(int f = 0, int s = StateCount, float FixedTime = 0)
    {
        if (ProceedingCoroutine != null)
            StopCoroutine(ProceedingCoroutine);
        ProceedingCoroutine = (FixedTime == 0) ? StartCoroutine("SetActingTrue", Random.Range(MinActionTime, MaxActionTime)) : StartCoroutine("SetActingTrue", FixedTime);
        if (isGround)
            switch (Random.Range(0, s))
            {
                case 0:
                    state = State.Idle;
                    break;
                case 1:
                    state = State.Move;
                    break;
                case 2:
                    state = State.Jump;
                    break;
                case 3:
                    state = State.Change;
                    break;
            }
        //Debug.Log(state);
        //Debug.Log(isActing);
    }

    IEnumerator SetActingTrue(float time)
    {
        isActing = true;
        yield return new WaitForSeconds(time);
        isActing = false;
    }




    // ü�¹� Ȱ��ȭ �� 6�ʵ� ��Ȱ��
    IEnumerator AppearHPUI()
    {
        HPUI.SetActive(true);
        yield return new WaitForSeconds(6f);
        HPUI.SetActive(false);
    }

    IEnumerator GetHittedStun(float time)
    {
        isHitStunned = true;
        yield return new WaitForSeconds(time);
        isHitStunned = false;
    }

    public void GetDamaged(int damage, GameObject Fromwho)
    {
        AttackTarget = Fromwho;
        if (damage == 0)
            damage = AttackTarget.GetComponent<Status>().AttackPower;


        if (AppearHPCoroutine != null)
            StopCoroutine(AppearHPCoroutine);
        AppearHPCoroutine = StartCoroutine("AppearHPUI");
        stat.HP -= damage;
        HPbar.fillAmount = stat.HP / stat.MaxHp;

        // ���� ���� �Ĵٺ��� �ڷ� �и���
        sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // ���� ����
        rb.velocity = new Vector2(1.3f * -Direction, rb.velocity.y);

        // �ִϸ��̼�
        anim.SetTrigger("Hitted");

        if (HittedCoroutine != null)
            StopCoroutine(HittedCoroutine);
        HittedCoroutine = StartCoroutine("GetHittedStun", 0.5f);

        if (ProceedingCoroutine != null)
            StopCoroutine(ProceedingCoroutine);
        ProceedingCoroutine = StartCoroutine("SetActingTrue", 10f);

        PopUpDamageText(damage);

        if (stat.HP < 0)
        {
            if (AttackTarget.tag == "Player")
            {
                Dictionary<string, float> statyouearn = new Dictionary<string, float>();
                for (int i = 0; i < plus_stat.Count; i++)
                {
                    statyouearn.Add(plus_stat[i], plus_value[i]);
                    
                }
                AttackTarget.GetComponent<Move>().PlayerStatChange(statyouearn);
            }
            StartCoroutine("Die");
        }
    }

    public void GetAirborne(Vector2 force)
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void PopUpDamageText(int damage)
    {
        GameObject DamageUI = Instantiate(DamageText, transform.localPosition, Quaternion.identity);
        DamageUI.GetComponentInChildren<DamageUI>().Spawn(damage, gameObject);
    }


    // ����
    IEnumerator Die()
    {
        anim.SetTrigger("Hitted");
        stat.MoveSpeed = 0f;
        Color color = sr.color;

        color.a = sr.color.a / 2;
        sr.color = color;

        while (color.a >= 0f) 
        {
            color.a -= (Time.deltaTime / 0.5f); // 0.5�ʿ� ���� �����
            sr.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
