using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCalculator : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player").gameObject;
        StartCoroutine("Move", Player.GetComponent<SpriteRenderer>().flipX);
    }

    IEnumerator Move(bool dir)
    {
        if (dir)
        {
            rb.velocity = new Vector2(-40, 0);
        }
        else
        {
            rb.velocity = new Vector2(40, 0);
        }
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<Move>().TeleportByCalcul(transform.localPosition);
        Destroy(gameObject);
    }
}
