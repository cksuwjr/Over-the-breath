using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class TrapDamage : Monster
{
    public GameObject bindTrap;
    float sustainTime = 0.6f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Deleter")
        {
            gameObject.SetActive(false);
        }
        if (col.tag == "PlayerHitbox")
        {
            if (col.GetComponentInParent<Player>())
                col.GetComponentInParent<Player>().CC(bindTrap, GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f), sustainTime);
            else if (col.GetComponentInParent<Monster>())
                col.GetComponentInParent<Monster>().CC(bindTrap, GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f), sustainTime);
            col.GetComponent<CrashHitbox>().ContactHitByTrap(GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f));
            GameManager.Instance.UIManager.SetDieMessage(playerKillMessage);
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Neutrality")
        {
            col.gameObject.GetComponent<Monster>().CC(bindTrap, GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f), sustainTime);
            col.gameObject.GetComponent<Monster>().GetDamaged(GetRandomDamageValue(stat.AttackPower, 0.8f, 1.2f), col.gameObject);
            gameObject.SetActive(false);
        }
        
    }
    void Start()
    {
        StartCoroutine(CheckingGravity());
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CheckingGravity());
    }

    IEnumerator CheckingGravity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (true)
        {
            if (rb.velocity.y < -10)
                rb.velocity = new Vector2(rb.velocity.x, -10);
            yield return null;
        }
    }

    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }
}
