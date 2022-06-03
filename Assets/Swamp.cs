using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour            
{                                                      
    private List<GameObject> Slimes;
    public List<string> AbsorbableTarget;

    private void Start()
    {
        Slimes = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool Absorbable = false;
        foreach (string target in AbsorbableTarget)
        {
            if (col.name == target)
                Absorbable = true;
        }
        if (col.tag == "Neutrality" && Absorbable)
        {
            Slimes.Add(col.gameObject);
            col.transform.GetChild(2).GetComponent<Slime>().MySwamp = gameObject;
        }

        if(col.tag == "Player")
        {
            col.GetComponent<Move>().ApplyDebuff("Speed", 2.5f);
            col.GetComponent<Move>().ApplyDebuff("Jump", 1.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        bool Absorbable = false;
        foreach (string target in AbsorbableTarget)
        {
            if (col.name == target)
                Absorbable = true;
        }
        if (col.tag == "Neutrality" && Absorbable)
        {
            col.transform.GetChild(2).GetComponent<Slime>().MySwamp = null;
            Slimes.Remove(col.gameObject);

        }

        if (col.tag == "Player")
        {
            col.GetComponent<Move>().RemoveDebuff("Speed");
            col.GetComponent<Move>().RemoveDebuff("Jump");
        }
    }
    public void KillOther(GameObject otherSlime)
    {
        Slimes.Remove(otherSlime);
    }
}
