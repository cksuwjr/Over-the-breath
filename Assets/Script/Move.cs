using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Transform tr;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Status stat;
    bool isJumping;
    int JumpCount = 2;

    GameObject Fire;



    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();

        Fire = Resources.Load("Prefab/Fire") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {

        // 키보드 입력!1
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-stat.MoveSpeed, rb.velocity.y);
            sr.flipX = true;
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(stat.MoveSpeed, rb.velocity.y);
            sr.flipX = false;
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
        }


        // 점프!!
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(JumpCount > 0)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                JumpCount--;
                Debug.Log(JumpCount);
            }
        }

        // 제자리!!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector2(0, 0);
        }

        // 불덩이 발사!
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("BasicAttack");
            Vector3 SpawnPosition;
            if (sr.flipX)
                SpawnPosition = tr.position + new Vector3(-1f, 0);
            else
                SpawnPosition = tr.position + new Vector3(1f, 0);

            Instantiate(Fire, SpawnPosition, Quaternion.identity);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name == "Ground")
        {
            if (JumpCount != 2)
                JumpCount = 2;
            anim.SetBool("Jump", false);
        }
    }
}
