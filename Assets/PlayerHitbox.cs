using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    List<GameObject> Monsters = new List<GameObject>();
    Move Player;
    bool isDamagedRecent = false;

    private void Start()
    {
        Player = transform.parent.GetComponent<Move>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("충돌!");
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
        Debug.Log("아야!");
        isDamagedRecent = true;
        Player.GetDamage(Damage);
        yield return new WaitForSeconds(2.0f);
        if(Monsters.Count > 0)
            StartCoroutine("GetHurtPlayer", Monsters[0].transform.GetComponent<Status>().AttackPower);
        else
            isDamagedRecent = false;
    }
}
