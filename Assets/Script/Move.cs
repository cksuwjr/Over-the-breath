using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    Status stat;
    bool isJumping;
    int JumpCount = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        stat = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector2(0, 0);
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
