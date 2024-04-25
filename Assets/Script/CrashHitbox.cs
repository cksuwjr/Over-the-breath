using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashHitbox : MonoBehaviour
{
    List<GameObject> Monsters = new List<GameObject>();

    bool isDamagedRecent = false;

    public void ContactHitByTrap(float damage)
    {
        if (GameManager.Instance.StoryManager.nowStoryReading) return;
        if (!isDamagedRecent && gameObject.activeSelf)
            StartCoroutine("GetHurt", damage);
    }
    public void ContactHitByMonster(GameObject attacker, float damage)
    {
        if (GameManager.Instance.StoryManager.nowStoryReading) return;

        Monsters.Add(attacker);
        if (!isDamagedRecent && gameObject.activeSelf)
            StartCoroutine("GetHurt", damage);
    }

    public void ContactOutByMonster(GameObject attacker)
    {
        if (Monsters.Contains(attacker))
            Monsters.Remove(attacker);
    }

    IEnumerator GetHurt(int Damage)
    {
        isDamagedRecent = true;
        if (transform.parent.GetComponent<Player>())
        {
            if (Monsters.Count > 0)
                transform.parent.GetComponent<Player>().GetDamage(Damage, Monsters[0]);
            else
            {
                transform.parent.GetComponent<Player>().GetDamage(Damage);
            }
        }
        else if (transform.parent.GetComponent<Monster>())
        {
            if (Monsters.Count > 0)
                transform.parent.GetComponent<Monster>().GetDamaged(Damage, Monsters[0]);
            else
            {
                transform.parent.GetComponent<Monster>().GetDamaged(Damage);
            }
            
        }
        yield return new WaitForSeconds(1.7f);
        if(Monsters.Count > 0)
            StartCoroutine("GetHurt", Monsters[0].GetComponent<Status>().AttackPower);
        else
            isDamagedRecent = false;
    }

    public void init()
    {
        isDamagedRecent = false;        
    }

    private void OnEnable()
    {
        init();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Monsters.Clear();
    }
}
