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


    Coroutine AppearHPCoroutine;
    Coroutine ProceedingCoroutine;
    Coroutine HittedCoroutine;

    bool isActing;
    bool isHitStunned;



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

        state = State.Idle;

    }

    //    //레이저 시각효과 활성화
    //    //앞
    //Debug.DrawRay(transform.position, new Vector3(1.4f * Direction, 0, 0), new Color(0, 1, 0));
    //    //바닥
    //Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * ((cc.size.y / 2) + 0.15f), new Color(0, 1, 0));


    void Update()
    {
        ChangeDirection();
        CheckingTopography();
        MovingPattern();
    }
    void ChangeDirection()
    {
        // flipX에 따른 방향 설정(1/-1)
        Direction = sr.flipX ? -1 : 1;
    }
    void CheckingTopography()
    {
        // 본인기준 주변 지형파악 위한 레이저 체크
        RaycastHit2D rayFrontGroundCheck, rayFrontWallCheck, rayUnderGroundCheck;

        rayFrontGroundCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 1f, 1 << LayerMask.NameToLayer("Ground"));
        rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f, 1 << LayerMask.NameToLayer("Wall"));
        rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), (cc.size.y / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));

        // 앞에 땅이 있다면 점프로 도약
        if (isGround && rayFrontGroundCheck.collider != null)
            rb.velocity = new Vector2(stat.MoveSpeed * 2f * Direction, stat.JumpPower);

        // 앞에 벽이 있다면 방향전환
        if (isGround && rayFrontWallCheck.collider != null)
            sr.flipX = (sr.flipX) ? false : true;

        // 바닥 체크후 isGround 전환
        isGround = (rayUnderGroundCheck.collider == null) ? false : true;
    }
    void MovingPattern()
    {

        // 공격 대상이 없다면
        if (AttackTarget == null)
        {
            if (!isActing)
                SetAction();
            switch (state)
            {
                // 멈춤
                case State.Idle:
                    rb.velocity = new Vector2(0, rb.velocity.y);

                    anim.SetBool("Idle", true);
                    anim.SetBool("Walk", false);

                    break;

                // 움직임
                case State.Move:

                    rb.velocity = new Vector2(stat.MoveSpeed * Direction, rb.velocity.y);

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    break;
                // 점프
                case State.Jump:
                    if (isGround)
                        rb.velocity = new Vector2(rb.velocity.x, stat.JumpPower);
                    SetAction();
                    break;
                // 방향 전환
                case State.Change:
                    sr.flipX = (bool)(Random.value > 0.5f); // flipX를 랜덤으로 true false 부여
                    SetAction(1, StateCount - 2);
                    break;
            }
        }
        else  // 공격 대상이 있다면
        {
            if (!isHitStunned && isGround) // 피격시간 끝나면
            {
                sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정

                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);

                rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
            }

            if (!isHitStunned && Mathf.Abs(transform.position.x - AttackTarget.transform.position.x) < 2f) // 가까울때 적이 나보다 높이 위치하면 점프
            {
                float dinstanceGap = transform.position.y - AttackTarget.transform.position.y;
                if (Mathf.Abs(dinstanceGap) > 0.5f && dinstanceGap < 0 && isGround)
                    rb.velocity = new Vector2(rb.velocity.x, 7);
            }


            if (!isActing)
                AttackTarget = null;
        }
    }

    // 상태 랜덤 부여  
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
    }

    IEnumerator SetActingTrue(float time)
    {
        isActing = true;
        yield return new WaitForSeconds(time);
        isActing = false;
    }




    // 체력바 활성화 후 6초뒤 비활성
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

    void GetDamaged(int damage)
    {
        if (AppearHPCoroutine != null)
            StopCoroutine(AppearHPCoroutine);
        AppearHPCoroutine = StartCoroutine("AppearHPUI");
        stat.HP -= damage;
        HPbar.fillAmount = stat.HP / stat.MaxHp;

        // 맞은 방향 쳐다본후 뒤로 밀리기
        sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정
        rb.velocity = new Vector2(0.8f * -Direction, rb.velocity.y);

        // 애니메이션
        anim.SetTrigger("Hitted");

        if (HittedCoroutine != null)
            StopCoroutine(HittedCoroutine);
        HittedCoroutine = StartCoroutine("GetHittedStun", 0.3f);

        if (stat.HP < 0)
        {
            AttackTarget.GetComponent<Status>().MaxHp++;
            Debug.Log("체력증가! : " + AttackTarget.GetComponent<Status>().MaxHp);
            StartCoroutine("Die");
        }
    }

    // 죽음
    IEnumerator Die()
    {
        anim.SetTrigger("Hitted");
        stat.MoveSpeed = 0f;
        Color color = sr.color;

        color.a = sr.color.a / 2;
        sr.color = color;

        while (color.a >= 0f) 
        {
            color.a -= (Time.deltaTime / 0.5f); // 0.5초에 걸쳐 사라짐
            sr.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }



    // Trigger 시작시
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack" && stat.MoveSpeed != 0)
        {
            rb.velocity = Vector2.zero;

            Destroy(col.gameObject);

            AttackTarget = GameObject.FindGameObjectWithTag("Player");

            if (ProceedingCoroutine != null)
                StopCoroutine(ProceedingCoroutine);
            ProceedingCoroutine = StartCoroutine("SetActingTrue", 10f);

            GetDamaged(AttackTarget.GetComponent<Status>().AttackPower);

        }
    }
    
}
