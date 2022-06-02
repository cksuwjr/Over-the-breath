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
    bool isJumpRecent; // 점프 너무 자주해서 제한하려고
    


    Coroutine CheckingTopographyCoroutine;
    Coroutine AppearHPUICoroutine;
    Coroutine ChaseCoroutine;
    Coroutine HittedStunCoroutine;
    Coroutine ActCoroutine;
    

    GameObject AttackTarget;
    GameObject mySpawner;

    void Start()
    {
        // 기본 초기화
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();

        // UI 관련
        HPUI = transform.GetChild(0).gameObject;
        HPbar = HPUI.transform.GetChild(1).gameObject.GetComponent<Image>();

        if (DamageText == null)
            DamageText = Resources.Load("Prefab/DamageUim") as GameObject;


        // 만든것 초기화
        stat = GetComponent<Status>();

        // 실행
        ActCoroutine = StartCoroutine("Act");
        //StartCoroutine("CheckingTopography");
        //Debug.Log(((cc.size.y / 2) + 0.15f));
    }


    IEnumerator CheckingTopography()
    {
        // 본인기준 주변 지형파악 위한 레이저 체크
        RaycastHit2D rayFrontGroundCheck, rayFrontWallCheck, rayUnderGroundCheck, rayFloorGroundCheck;
        while (true)
        {

            //레이저 시각효과 활성화

            //앞에 땅있나(빨강)    rayFrontGroundCheck
            Debug.DrawRay(transform.position + new Vector3((transform.localScale.x * (7f / 10f) + 0.35f) * Direction, transform.localScale.y * -(3f / 7f), 0), new Vector3(0, 1, 0), Color.red);
            //앞에 벽있나(파랑)    rayFrontWallCheck
            Debug.DrawRay(transform.position, new Vector3(Direction * 0.5f * (transform.localScale.x * (10f / 7f)), 0, 0), Color.blue);
            // 바닥(검정)  rayUnderGroundCheck 
            Debug.DrawRay(transform.position + new Vector3(transform.localScale.x * (1f / 2f) * -Direction, 0, 0), new Vector3(0, transform.localScale.y * -(59f / 70f), 0), Color.black);
            // 바닥에 닿아있나(하얀) rayFloorGroundCheck
            Debug.DrawRay(transform.position, new Vector3(0, transform.localScale.y * -(59f / 70f), 0), Color.white);



            // 레이저 설정
            rayFrontGroundCheck = Physics2D.Raycast(transform.position + new Vector3((transform.localScale.x * (7f / 10f) + 0.35f) * Direction, transform.localScale.y * -(3f / 7f), 0), Vector2.up, 1f, 1 << LayerMask.NameToLayer("Ground"));
            rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f * (transform.localScale.x * (10f / 7f)), 1 << LayerMask.NameToLayer("UnPassableWall"));
            rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x * (1f / 2f) * -Direction, 0, 0), new Vector3(0, -1, 0), transform.localScale.y * (59f / 70f), 1 << LayerMask.NameToLayer("Ground"));
            rayFloorGroundCheck = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0), transform.localScale.y * (59f / 70f), 1 << LayerMask.NameToLayer("Ground"));
            // 앞쪽에 땅(점프지형)이 있다면 점프로 도약, 앞쪽에 땅이 없다면(땅이 끝나면) 도약 
            if (isGround && rayFrontGroundCheck.collider != null || (rayFloorGroundCheck.collider == null && isGround))
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);
                rb.velocity = new Vector2((stat.MoveSpeed - 1f + 0.6f * (transform.localScale.y / 0.7f)) * Direction, stat.JumpPower + ((transform.localScale.y * 1f) / 7f));
            }

            // 앞에 벽이 있다면 방향전환
            if (isGround && rayFrontWallCheck.collider != null)
            {
                sr.flipX = !sr.flipX;
                Direction *= -1;
                rb.velocity = new Vector2(Direction * stat.MoveSpeed, rb.velocity.y);
            }

            // 바닥 체크후 isGround 전환
            isGround = (rayUnderGroundCheck.collider == null) ? false : true;

            yield return null;
        }
    }

    // 평상시 활동
    IEnumerator Act()
    {
        int Actnum = isJumpRecent ? Random.Range(1, 2) : Random.Range(0, 3);
        if (!isGround)
            Actnum = 1;
        // 행동 정의
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

        // 지형체크 코루틴 제어
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
                if (isGround) // 점프 아닐때만 방향바꾸게 (자꾸 점프 도중 방향바꿔서 사나워보임)
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
        yield return new WaitForSeconds(Random.Range(2, 3)); // 활동 시간
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
                sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정
                Direction = sr.flipX ? -1 : 1;

                anim.SetBool("Idle", false);
                anim.SetBool("Walk", true);

                rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
            }

            float distanceXGap = Mathf.Abs(transform.position.x - AttackTarget.transform.position.x);
            if (distanceXGap < 2f) // 가까울때 적이 
            {
                float dinstanceYGap = transform.position.y - AttackTarget.transform.position.y;
                if (Mathf.Abs(dinstanceYGap) > 0.5f && dinstanceYGap < 0) // 나보다 높이 위치하면 점프
                {
                    if (isGround)
                        rb.velocity = new Vector2(rb.velocity.x, 7);
                }

                // 공격패턴
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

    // Trigger 시작시
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerAttack")
        {
            GetDamaged(GetRandomDamageValue(col.GetComponent<Fire>().Damage, 0.8f, 1.2f), GameObject.FindGameObjectWithTag("Player"));
            Destroy(col.gameObject);
        }
    }

    // 데미지 입음
    public void GetDamaged(float damage, GameObject Fromwho)
    {
        stat.HP -= damage;                          // 체력감소
        HPbar.fillAmount = stat.HP / stat.MaxHp;    // UI 업데이트

        if (AppearHPUICoroutine != null)
            StopCoroutine(AppearHPUICoroutine);
        AppearHPUICoroutine = StartCoroutine("AppearHPUI");     // HP UI 보이기

        PopUpDamageText(damage);

        

        if (HittedStunCoroutine != null)
            StopCoroutine(HittedStunCoroutine);
        HittedStunCoroutine = StartCoroutine("HittedStunned", Fromwho);
 



        if (stat.HP <= 0)                           // 체력 다달면
        {
            Die(Fromwho);
        }
    }

    IEnumerator HittedStunned(GameObject Fromwho)
    {
        anim.SetTrigger("Hitted");

        // 타겟 설정
        if (Fromwho.tag != "trap")
        {
            AttackTarget = Fromwho;

            if (ChaseCoroutine != null)             // 기존 추적 제거
                StopCoroutine(ChaseCoroutine);
            if (ActCoroutine != null)               // 기본 활동 제거
                StopCoroutine(ActCoroutine);

            // 넉백
            sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true;
            Direction = sr.flipX ? -1 : 1;
            rb.velocity = new Vector2(-0.2f * Direction, rb.velocity.y);
            yield return new WaitForSeconds(0.5f);  // 스턴

            ChaseCoroutine = StartCoroutine("ChaseEnemy"); // 쫒기 시작

            yield return new WaitForSeconds(10f); // 10초 후 어그로 풀림
            StopCoroutine(ChaseCoroutine);

            // 기본 활동 재개
            if (ActCoroutine != null)
                StopCoroutine(ActCoroutine);
            ActCoroutine = StartCoroutine("Act");
        }
        else
        {
            if (ChaseCoroutine != null)             // 기존 추적 제거
                StopCoroutine(ChaseCoroutine);
            if (ActCoroutine != null)               // 기본 활동 제거
                StopCoroutine(ActCoroutine);

            yield return new WaitForSeconds(0.5f);  // 스턴
            // 기본 활동 재개
            if (ActCoroutine != null)
                StopCoroutine(ActCoroutine);
            ActCoroutine = StartCoroutine("Act");
        }
    }

    public void Die(GameObject Fromwho)
    {
        //if(Fromwho != null){
        //      Fromwho.GetComponent<Status>().
        //}
        if (mySpawner != null)
            mySpawner.GetComponent<EnemySpawnManager>().AdjustEnemyCount(-1);

        Destroy(gameObject);
    }

    // 랜덤 숫자 발급, Ontrigger의 GetDamage에 쓰임
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    // 체력바 UI 6초간 보이기
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
    
    public bool AreYouGround()
    {
        return isGround;
    }

    public void MySpawner(GameObject spawner)
    {
        mySpawner = spawner;
    }
}
