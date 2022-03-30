using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    int direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (GameObject.Find("Player").GetComponent<SpriteRenderer>().flipX)
        {
            sr.flipX = true;
            direction = -1;
        }
        else
        {
            sr.flipX = false;
            direction = 1;
        }
        Destroy(gameObject, 0.3f);
    }
    void Update()
    {
        rb.velocity = new Vector2(direction * 15, 0);
    }

    // Trigger Ω√¿€Ω√
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" && col.GetComponent<Status>().MoveSpeed != 0)
        {
            col.GetComponent<EnemyAI>().GetDamaged(0, GameObject.FindGameObjectWithTag("Player"));
            Destroy(gameObject);
        }
    }
}
