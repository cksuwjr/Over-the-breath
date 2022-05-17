using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public GameObject Me;
    List<GameObject> others = new List<GameObject>();

    public float PlusDamage;
    public float MulDamage = 1;


    private void Start()
    {
        if (Me == null)
            Me = GameObject.Find("TreeOfDesire");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality" || collision.tag == "Player")
            others.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality" || collision.tag == "Player")
            if (others.Contains(collision.gameObject))
                others.Remove(collision.gameObject);
    }
    void HitAll()
    {
        List<GameObject> beHitGroup = new List<GameObject>();
        foreach(GameObject n in others)
        {
            if (n != null)
                beHitGroup.Add(n);
        }
        foreach (GameObject n in beHitGroup)
        {
            if (n == null)
                continue;
            if (n.tag == "Enemy" || n.tag == "Neutrality")
            {
                if (n.GetComponent<EnemyAI2>().AreYouGround())
                {
                    n.GetComponent<EnemyAI2>().GetDamaged(GetRandomDamageValue(Me.GetComponent<Status>().AttackPower * MulDamage + PlusDamage, 0.8f, 1.2f), Me);
                    n.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5f));
                }
            }
            else if (n.tag == "Player")
            {
                if (n.GetComponent<Move>().AreYouGround())
                {
                    n.GetComponent<Player>().GetDamage(GetRandomDamageValue(Me.GetComponent<Status>().AttackPower * MulDamage + PlusDamage, 0.8f, 1.2f));
                    n.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5f));
                }
            }
        }
    }
    int GetRandomDamageValue(float OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    void AddCollider()
    {
        foreach(CapsuleCollider2D col in GetComponents<CapsuleCollider2D>())
        {
            if (col.isTrigger)
                Destroy(col);
            else
                col.enabled = true;
        }
    }
}
