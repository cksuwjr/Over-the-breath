using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;     
    SpriteRenderer sr;  
    Animator anim;  

    Status stat;   
    Skill skill;

    int JumpCount = 2; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        stat = GetComponent<Status>();
        skill = GetComponent<Skill>();
    }

    void Update()
    {
        if (skill.isMumchit)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            PlayerKeyboardInput();
    }

    // Player 키보드 입력 (움직임)
    void PlayerKeyboardInput()
    {
        // 키보드 입력!1
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            float direction = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * stat.MoveSpeed, rb.velocity.y);
            if (direction == -1)
                sr.flipX = true;
            else if (direction == 1)
                sr.flipX = false;

            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            if (Mathf.Abs(rb.velocity.x) <= stat.MoveSpeed || !skill.isMumchit)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("Idle", true);
                anim.SetBool("Walk", false);
            }
        }


        // 점프!!
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (JumpCount > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                JumpCount--;
            }
        }

        // 제자리!! [완성시 삭제예정]
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector2(0, 0);
            Debug.Log("눌림");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            stat.AttackPower += 30;
            GetComponent<Player>().CheckEvent();
        }


    }

    // 충돌 설정
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Ground")
        {
            if (JumpCount != 2)
                JumpCount = 2;
            anim.SetBool("Jump", false);
        }
    }

}
