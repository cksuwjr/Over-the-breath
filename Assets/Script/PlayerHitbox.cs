using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    List<GameObject> Monsters = new List<GameObject>();
    Player Player;
    bool isDamagedRecent = false;
    GameObject Trap;
    private void Awake()
    {
        Player = transform.parent.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality")
        {
            Monsters.Add(collision.gameObject);
            if (!isDamagedRecent)
                StartCoroutine("GetHurtPlayer", collision.transform.GetComponent<Status>().AttackPower);
        }
        if(collision.tag == "trap")
        {
            TrapDamage t = collision.GetComponent<TrapDamage>();
            if (t.traptype == "Fixed")
            {
                Trap = collision.gameObject;
                if (!isDamagedRecent)
                    StartCoroutine("GetHurtPlayer", t.trapDamage);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality")
            if (Monsters.Contains(collision.gameObject))
                Monsters.Remove(collision.gameObject);
        if (collision.tag == "trap")
            Trap = null;
    }
    IEnumerator GetHurtPlayer(int Damage)
    {
        isDamagedRecent = true;
        Player.GetDamage(GetRandomDamageValue(Damage, 0.8f, 1.2f));
        yield return new WaitForSeconds(1.7f);
        if(Monsters.Count > 0)
            StartCoroutine("GetHurtPlayer", Monsters[0].transform.GetComponent<Status>().AttackPower);
        else
            isDamagedRecent = false;

        if(Trap != null)
            StartCoroutine("GetHurtPlayer", Trap.transform.GetComponent<TrapDamage>().trapDamage);
    }
    public void init()
    {
        isDamagedRecent = false;        
    }
    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }
}
