using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject AttackTarget = null;

    enum State
    {
        Idle,
        Move,
        Jump,
        SeeO
    }
    State state;
    float WaitingTime;
    float HittedTime;

    bool isGround;
    int Direction = 1;


    const float MinActionTime = 1f;
    const float MaxActionTime = 3f;
    const int StateCount = 4;
    const float MoveSpeed = 2f;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        WaitingTime = 0f;
        state = State.Idle;    
        
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
        rayFrontGroundCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0) * 1.4f, 1f, layerMask);

        // 레이저가 무언가 발견했을때 + 슬라임이 앞에 땅에 있다면
        if (isGround && rayFrontGroundCheck.collider != null)
            rb.velocity = new Vector2(MoveSpeed * 2f * Direction, 7f);

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

                        if (WaitingTime < 0)
                        {
                            SetAction();
                            SetWait();
                        }
                        break;

                    // 움직임
                    case State.Move:
                        rb.velocity = new Vector2(0, rb.velocity.y);

                        anim.SetBool("Idle", false);
                        anim.SetBool("Walk", true);

                        rb.velocity = new Vector2(MoveSpeed * Direction, rb.velocity.y);


                        if (WaitingTime < 0)
                        {
                            SetAction();
                            SetWait();
                        }
                        break;
                    // 점프
                    case State.Jump:
                        if (isGround)
                            rb.velocity = new Vector2(rb.velocity.x, 7);
                        SetAction();
                        break;
                    // 방향 전환
                    case State.SeeO:
                        sr.flipX = (bool)(Random.value > 0.5f); // flipX를 랜덤으로 true false 부여
                        SetAction();
                        break;
                }


            // 공격 대상이 있다면
            else
            {
                if (HittedTime < 0)
                { // 피격시간 끝나면
                    sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    rb.velocity = new Vector2(MoveSpeed * 1.2f * Direction, rb.velocity.y);
                }

                if(HittedTime < 0 && Mathf.Abs(transform.position.x - AttackTarget.transform.position.x ) < 2f)
                {
                    float dinstanceGap = transform.position.y - AttackTarget.transform.position.y;
                    if(Mathf.Abs(dinstanceGap) > 0.5f && dinstanceGap < 0 && isGround)
                        rb.velocity = new Vector2(rb.velocity.x, 7);

            }


            if (WaitingTime < 0)
                    AttackTarget = null;
            }


        // 대기시간(상태) 타이머
        if(WaitingTime >= 0)
            WaitingTime -= Time.deltaTime;


        // 피격시간(멈칫) 타이머
        if (HittedTime >= 0)
            HittedTime -= Time.deltaTime;

    }

    // 상태 랜덤 부여  
    void SetAction()
    {
        if(isGround)
            switch (Random.Range(0, StateCount))
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

    // 대기시간(상태) 랜덤값 부여
    void SetWait()
    {
        WaitingTime = Random.Range(MinActionTime, MaxActionTime); ;
    }


    // Trigger 시작시
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            rb.velocity = Vector2.zero;

            Destroy(col.gameObject);
            AttackTarget = GameObject.FindGameObjectWithTag("Player");
            sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정
            rb.velocity = new Vector2(0.8f * -Direction, rb.velocity.y);

            anim.SetTrigger("Hitted");
            HittedTime = 0.5f;
            WaitingTime = 10f;


        }
    }
}
