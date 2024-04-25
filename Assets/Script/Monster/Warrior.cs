using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : GroundMonster
{
    protected override void MovingPattern()
    {
        if (isHitStunned)
        {
        }
        else if (AttackTarget && AttackTarget.activeSelf)
        {
            if (!isMumchit)
            {
                if (FindEnemy().Count > 0)
                {
                    StartCoroutine("BasicAttack", 1);
                }
                else
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
                    //if (isGround)
                    //    rb.velocity = new Vector2(rb.velocity.x, stat.JumpPower);
                    SetAction();
                    break;
                // 방향 전환
                case State.Change:
                    sr.flipX = (bool)(Random.value > 0.5f); // flipX를 랜덤으로 true false 부여
                    ChangeDirection();
                    SetAction(1, StateCount - 2);
                    break;
            }
            var enemys = FindEnemy();
            if (enemys.Count > 0)
                AttackTarget = enemys[0];
        }
    }

    List<GameObject> FindEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(0.35f * Direction, 0.03f), new Vector3(0.35f, 0.35f), 0, enemyLayer);
        Collider2D[] myCollider = GetComponentsInChildren<Collider2D>();

        List<GameObject> myEnemys = new List<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            bool isMeinclude = false;
            foreach (Collider2D mine in myCollider) if (collider == mine) isMeinclude = true;
            if (isMeinclude) continue;

            if (!myEnemys.Contains(collider.gameObject))
                 myEnemys.Add(collider.gameObject);
        }
        return myEnemys;
    }

    IEnumerator BasicAttack(int attackableCount = 1)
    {
        isMumchit = true;

        int count = 0;

        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetTrigger("Attack");

        var enemys = FindEnemy();

        foreach (GameObject enemy in enemys)
        {
            var monster = enemy.GetComponent<Monster>();
            var player = enemy.GetComponent<Player>();
            if (monster)
            {
                if (!monster.die)
                {
                    monster.GetDamaged(stat.AttackPower, gameObject);
                    count++;
                }
            }

            if (player)
            {
                player.GetDamage(stat.AttackPower, gameObject);
                count++;
            }

            if (count >= attackableCount)
                break;
        }
        yield return new WaitForSeconds(0.55f);
        isMumchit = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

    }
    void OnTriggerExit2D(Collider2D col)
    {

    }
}

