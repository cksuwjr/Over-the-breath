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
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().flipX)
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
        if ((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Neutrality") && col.GetComponent<Status>().MoveSpeed != 0)
        {
            col.GetComponent<EnemyAI>().GetDamaged(GetRandomDamageValue(GameObject.FindGameObjectWithTag("Player").GetComponent<Status>().AttackPower, 0.8f, 1.2f), GameObject.FindGameObjectWithTag("Player"));
            Destroy(gameObject);
        }
    }

    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }
}
