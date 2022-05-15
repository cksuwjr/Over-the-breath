using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public float trapDamage = 10;
    public string traptype = "Fixed";
    public GameObject bindTrap;


    void Start()
    {
        if(traptype != "Fixed")
        {
            StartCoroutine(CheckingGravity());
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Neutrality" || col.gameObject.tag == "Enemy")
        {
             if(traptype == "Bind")
            {
                GameObject Trap = Instantiate(bindTrap, col.transform.position, Quaternion.identity);
                Trap.transform.SetParent(col.transform);
                StartCoroutine(BindTrap(col.gameObject, Trap));
            }
        }
    }

    IEnumerator BindTrap(GameObject target, GameObject SpawnedTrap)
    {
        GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        GetComponent<CircleCollider2D>().isTrigger = true;

        Status targetStat = target.GetComponent<Status>();
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        targetRb.velocity = new Vector2(0, targetRb.velocity.y);




        targetStat.MoveSpeed = 0;
        targetStat.JumpPower = 1f;

        for (int i = 0; i < 4; i++)
        {
            if (target.tag == "Player")
                target.GetComponent<Player>().GetDamage((int)trapDamage);
            else
                target.GetComponent<EnemyAI2>().GetDamaged(trapDamage, gameObject);

            if (GameObject.Find("TreeOfDesire") != null) // ¿å¸ÁÀÇ³ª¹«°¡ ÀÖ´Ù¸é 
            {
                GameObject.Find("TreeOfDesire").GetComponent<Status>().MaxHp += trapDamage;
                GameObject.Find("TreeOfDesire").GetComponent<Status>().HP += trapDamage;
            }
            yield return new WaitForSeconds(0.65f);
            if (target == null)
            {
                Destroy(SpawnedTrap);
                Destroy(gameObject);
                yield break;
            }
        }

        targetStat.MoveSpeed = targetStat.BasicSpeed;
        targetStat.JumpPower = targetStat.BasicJumpPower;
        Destroy(SpawnedTrap);
        Destroy(gameObject);
    }

    IEnumerator CheckingGravity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (true)
        {
            if (rb.velocity.y < -13)
                rb.velocity = new Vector2(rb.velocity.x, -13);
            yield return null;
        }
    }
}
