using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMonster : Monster
{
    protected enum State
    {
        Idle,
        Move,
        Jump,
        Change
    }

    protected State state;
    protected const int StateCount = 4;

    protected override void Init()
    {
        state = State.Idle;
    }

    protected override void OnEnableInit()
    {
        state = State.Idle;
        base.OnEnableInit();
    }

    protected override IEnumerator Act()
    {
        FallSpeedCheck();
        if (!binded)
        {
            MovingPattern();
            CheckingTopography();
        }
        yield return null;
        ActCoroutine = StartCoroutine("Act");
    }
    void CheckingTopography()
    {
        // 본인기준 주변 지형파악 위한 레이저 체크
        RaycastHit2D rayFrontGroundCheck, rayFrontWallCheck, rayUnderGroundCheck, rayFloorGroundCheck;

        //    //레이저 시각효과 활성화
        //    //앞
        //Debug.DrawRay(transform.position + new Vector3(Direction, 0, 0), new Vector3(0, 1, 0), new Color(0, 1, 0));
        //    //바닥
        //Debug.DrawRay(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0) * ((cc.size.y / 2) + 0.15f), new Color(0, 1, 0));

        rayFrontGroundCheck = Physics2D.Raycast(transform.position + new Vector3(Direction, 0, 0), Vector2.up, 1f, 1 << LayerMask.NameToLayer("Ground"));
        rayFrontWallCheck = Physics2D.Raycast(transform.position, new Vector3(Direction, 0, 0), 0.5f, 1 << LayerMask.NameToLayer("UnPassableWall"));

        var sizeY = col.bounds.size.y;
        rayUnderGroundCheck = Physics2D.Raycast(transform.position + new Vector3(0.35f * -Direction, 0, 0), new Vector3(0, -1, 0), (sizeY / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
        rayFloorGroundCheck = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0), (sizeY / 2) + 0.15f, 1 << LayerMask.NameToLayer("Ground"));
        // 앞에 땅이 있다면 점프로 도약
        if (isGround && rayFrontGroundCheck.collider != null || (rayFloorGroundCheck.collider == null && isGround))
        {
            if (Direction != 0 && state != State.Idle)
            {
                rb.velocity = new Vector2(stat.MoveSpeed * Direction, stat.JumpPower);
            }
        }

        // 앞에 벽이 있다면 방향전환
        if (isGround && rayFrontWallCheck.collider != null)
        {
            sr.flipX = (sr.flipX) ? false : true;
            ChangeDirection();
        }

        // 바닥 체크후 isGround 전환
        isGround = (rayUnderGroundCheck.collider == null) ? false : true;
    }

    protected virtual void MovingPattern()
    {
        if (isHitStunned)
        {
        }
        else if (AttackTarget && AttackTarget.activeSelf)
        {
            if (!isMumchit)
            {
                if (isGround)
                {
                    sr.flipX = (transform.position.x < AttackTarget.transform.position.x) ? false : true; // 방향 설정
                    ChangeDirection();

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walk", true);

                    rb.velocity = new Vector2(stat.MoveSpeed * 1.2f * Direction, rb.velocity.y);
                }
                float distanceXGap = Mathf.Abs(transform.position.x - AttackTarget.transform.position.x);
                if (distanceXGap < 2f) // 가까울때 적이 
                {
                    float dinstanceYGap = transform.position.y - AttackTarget.transform.position.y;
                    if (Mathf.Abs(dinstanceYGap) > 0.5f && dinstanceYGap < 0) // 나보다 높이 위치하면 점프
                        if (isGround) rb.velocity = new Vector2(rb.velocity.x, 7);
                }
            }
        }
        else 
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
                    ChangeDirection();
                    SetAction(1, StateCount - 2);
                    break;
            }
        }
    }
    void FallSpeedCheck()
    {
        if (rb.velocity.y < -12f)
            rb.velocity = new Vector2(rb.velocity.x, -12);
    }
    protected void SetAction(int f = 0, int s = StateCount, float FixedTime = 0)
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
}
