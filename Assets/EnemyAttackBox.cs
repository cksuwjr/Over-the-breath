using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    List<GameObject> Targets = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Targets.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (Targets.Contains(collision.gameObject))
                Targets.Remove(collision.gameObject);
    }
    public List<GameObject> GetAttackableTargets()
    {
        return Targets;
    }
}
