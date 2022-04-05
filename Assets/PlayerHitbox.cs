using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    List<GameObject> Monsters = new List<GameObject>();
    Player Player;
    bool isDamagedRecent = false;

    private void Start()
    {
        Player = transform.parent.GetComponent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Monsters.Add(collision.gameObject);
            if (!isDamagedRecent)
                StartCoroutine("GetHurtPlayer", collision.transform.GetComponent<Status>().AttackPower);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
            if (Monsters.Contains(collision.gameObject))
                Monsters.Remove(collision.gameObject);
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
    }

    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }
}
