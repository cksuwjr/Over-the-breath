using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCalculator : MonoBehaviour
{
    GameObject Player;
    GameObject Effect;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        StartCoroutine("Move", Player.GetComponent<SpriteRenderer>().flipX);
        Effect = Resources.Load("Prefab/Player_iron_Skill1Effect") as GameObject;
        StartCoroutine("MakeShadowEffect");
        
    }

    IEnumerator Move(bool dir)
    {
        if (dir)
        {
            rb.velocity = new Vector2(-30, 0);
        }
        else
        {
            rb.velocity = new Vector2(30, 0);
        }
        yield return new WaitForSeconds(0.15f);
        Player.GetComponent<Skill>().TeleportByCalcul(transform.localPosition);
        Destroy(gameObject);
    }
    IEnumerator MakeShadowEffect()
    {
        Instantiate(Effect, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.0425f);
        StartCoroutine("MakeShadowEffect");
    }
}
