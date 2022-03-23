using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public float moveSpeed = 5f;      // 움직임 속도
    int movementFlag;                 // 움직임 패턴(1 2)
    bool isTracing;                   // 추적 유무(O/X)
    Vector2 moveVelocity;     // 움직임 가속도 초기화


    public Transform target;          // 타겟 위치정보 가져오기위해
    Animator anim;                    // 애니메이터
    GameObject traceTarget;           // 추적 타겟 게임오브젝트
    Rigidbody2D rb;
    SpriteRenderer sr;
    enum Dist
    {
        Left,
        Right
    }
    Dist dist;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine("ChangeMovement"); // ChangeMovement() 호출
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        
        if (isTracing)          // 추적 중이면
        {
            Vector2 playerPos = traceTarget.transform.position;     // 대상 위치정보 가져옴

            if (playerPos.x < transform.position.x)                 // 나보다 뒤면(왼쪽) 방향설정(왼)  
                dist = Dist.Left;
            else if (playerPos.x > transform.position.x)            // 나보다 앞이면(오른쪽) 방향설정(오)
                dist = Dist.Right;
        }
        else                   // 추적 중이 아니면
        {
            if (movementFlag == 1)          // 움직임 패턴이 1이면 방향 설정(왼)
                dist = Dist.Left;
            else if (movementFlag == 2)     // 움직임 패턴이 2이면 방향 설정(오)
                dist = Dist.Right;
        }

        if (dist == Dist.Left)              // 방향(왼)이면 움직임가속도(왼쪽) 설정 new Vector(-1,0) , 왼쪽 바라보게(flipX)
        {
            moveVelocity = Vector2.left;
            sr.flipX = false;
        }
        else        // 방향(오)이면 움직임가속도(왼쪽) 설정 new Vector(+1,0) , 오른쪽 바라보게(flipX)
        {
            moveVelocity = Vector2.right;
            sr.flipX = true;
        };

        rb.velocity = moveVelocity * moveSpeed;        //    Vector(+-1, 0) * 1f * (프레임으로 나눠진 실제시간별 계산)/ 즉 플레이어 실제 움직임
        Debug.Log(rb.velocity);
        Debug.Log(moveVelocity);
        Debug.Log(moveSpeed);

    }

    IEnumerator ChangeMovement()            // 방향전환 메서드(행동패턴변화)
    {
        movementFlag = Random.Range(0, 3);      // 0~2까지 랜덤숫자 산출해서 movementFlag에 저장

        if (movementFlag == 0)                     // 플레이어애니메이션 조절(멈춤)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);


        yield return new WaitForSeconds(3f);        // 3초 대기후 

        StartCoroutine("ChangeMovement");           // 다시 이 메서드 실행
    }
    void OnTriggerEnter2D(Collider2D other)         // 뭔가가 닿을경우
    {
        if (other.gameObject.tag == "Player")       // 플레이어면
        {
            traceTarget = other.gameObject;         // 추적대상에 플레이어 넣음
            StopCoroutine("ChangeMovement");        // 위의 메서드(행동패턴변화) 재실행
        }
    }
    void OnTriggerStay2D(Collider2D other)          // 닿고 난후 닿아있는동안
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;                       //  추적중인가? => 예
            anim.SetBool("isWalking", true);        //  애니메이션 작동(걷기활성화)
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;                     // 추적중인가? => 아니오
            StartCoroutine("ChangeMovement");       // 행동패턴변화
        }
    }
}

