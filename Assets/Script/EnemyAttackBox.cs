using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    List<GameObject> Targets = new List<GameObject>();
    GameObject me;
    private void Start()
    {
        me = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Targets.Add(collision.gameObject);
        else if (collision.tag == "Neutrality")
            if(collision.gameObject != me)
                Targets.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Targets.Contains(collision.gameObject))
                Targets.Remove(collision.gameObject);
        }
        else if (collision.tag == "Neutrality")
        {
            if (Targets.Contains(collision.gameObject))
                Targets.Remove(collision.gameObject);
        }
    }
    public List<GameObject> GetAttackableTargets()
    {
        return Targets;
    }
}
