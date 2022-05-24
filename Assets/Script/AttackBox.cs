using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    List<GameObject> Monsters = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality")
            Monsters.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Neutrality")
            if (Monsters.Contains(collision.gameObject))
                Monsters.Remove(collision.gameObject);
    }
    public List<GameObject> GetAttackableTargets()
    {
        return Monsters;
    }
}
