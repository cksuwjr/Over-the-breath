using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    Status stat;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject AttackTarget = null;

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
        SeeO
    }

    State state;

    bool isGround;
    int Direction = 1;


    const float MinActionTime = 1f;
    const float MaxActionTime = 3f;
    const int StateCount = 4;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stat = GetComponent<Status>();

        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        state = State.Idle;

        stat.StatInit(500, 30, 500, 2, 7);

    }

    void Update()
    {
        // //레이저 시각효과 활성화
        // 앞
        //Debug.DrawRay(transform.position, new Vector3(1.4f * Direction, 0, 0), new Color(0, 1, 0));
        // 바닥
        //Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * 0.5f, new Color(0, 1, 0));





        // flipX에 따른 방향 설정(1/-1)
        Direction = sr.flipX ? -1 : 1; 


        // 본인보다 높은지형이면 점프하기 위한 레이저 체크
        int layerMask = 1 << LayerMask.NameToLayer("Ground");  // Ground 레이어만 충돌 체크함
        RaycastHit2D rayFrontGroundCheck;
        rayFrontGroundCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 1f, layerMask);

        // 레이저가 무언가 발견했을때 + 슬라임이 앞에 땅에 있다면
        if (isGround && rayFrontGroundCheck.collider != null)
            rb.velocity = new Vector2(stat.MoveSpeed * 2f * Direction, stat.JumpPower);

        // 레이저로 앞쪽 벽 체크
        RaycastHit2D rayFrontWallCheck;
        rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f, 1 << LayerMask.NameToLayer("Wall"));
        if(isGround && rayFrontWallCheck.collider != null)
            sr.flipX = (sr.flipX) ? false : true;
        
        // 레이저로 바닥 체크
        RaycastHit2D rayUnderGroundCheck;
        rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), 0.5f, layerMask);
        isGround = (rayUnderGroundCheck.collider == null) ? false : true;

            // 공격 대상이 없다면
            if (AttackTarget == null)
                switch (state)
                {
                    // 멈춤
                    case State.Idle:
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        anim.SetBool("Idle", true);
                        anim.SetBool("Walk", false);

                        rb.velocity = new Vector2(0, rb.velocity.y);
                        
                        if (!isActing)
                            SetAction();
                        break;

                    // 움직임
                    case State.Move:
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        anim.SetBool("Idle", false);
                        anim.SetBool("Walk", true);

                        rb.velocity = new Vector2(stat.MoveSpeed * Direction, rb.velocity.y);

                        if (!isActing)
                            SetAction();
                    break;
                    // 점프
                    case State.Jump:
                        if (isGround)
                            rb.velocity = new Vector2(rb.velocity.x, stat.JumpPower);
                        SetAction();
                        break;
                    // 방향 전환
                    case State.SeeO:
                        sr.flipX = (bool)(Random.value > 0.5f); // flipX를 랜덤으로 true false 부여
                        SetAction(1, StateCount - 2);
                        break;
                }


            // 공격 대상이 있다면
            else
            {
                if (!isHitStunned)
                { // 피격시간 끝나면
                    sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
                }

                if(!isHitStunned && Mathf.Abs(transform.position.x - AttackTarget.transform.position.x ) < 2f)
                {
                    float dinstanceGap = transform.position.y - AttackTarget.transform.position.y;
                    if(Mathf.Abs(dinstanceGap) > 0.5f && dinstanceGap < 0 && isGround)
                        rb.velocity = new Vector2(rb.velocity.x, 7);

                }


                if (!isActing)
                    AttackTarget = null;
            }
    }

    IEnumerator SetActingTrue(float time)
    {
        isActing = true;
        yield return new WaitForSeconds(time);
        isActing = false;
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
                    state = State.SeeO;
                    break;
            }
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
        HittedCoroutine = StartCoroutine("GetHittedStun", 0.5f);

        if (stat.HP < 0)
        {
            AttackTarget.GetComponent<Status>().MaxHp++;
            Debug.Log("체력증가! : " + AttackTarget.GetComponent<Status>().MaxHp);
            Destroy(gameObject);
        }
    }

  


    // Trigger 시작시
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            rb.velocity = Vector2.zero;

            Destroy(col.gameObject);

            AttackTarget = GameObject.FindGameObjectWithTag("Player");

            SetAction(0, 1, 10f);
            GetDamaged(AttackTarget.GetComponent<Status>().AttackPower);

        }
    }
    
}
