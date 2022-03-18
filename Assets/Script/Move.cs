using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    Status stat;
    bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stat = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-stat.MoveSpeed, rb.velocity.y);
            sr.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(stat.MoveSpeed, rb.velocity.y);
            sr.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(isJumping == false)
            {
                isJumping = true;
                rb.AddForce(new Vector2(0, 2) * stat.JumpPower, ForceMode2D.Impulse);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name == "Tilemap")
        {
            isJumping = false;
        }
    }
}
