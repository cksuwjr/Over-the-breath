using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour            
{                                                      
    private List<GameObject> Slimes;

    public GameObject King;
    

    private void Start()
    {
        Slimes = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Neutrality" && col.name == "Slime")
        {
            ChangeKing();

            Slimes.Add(col.gameObject);
            col.transform.GetChild(2).GetComponent<Slime>().MySwamp = gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Neutrality" && col.name == "Slime")
        {
            Slimes.Remove(col.gameObject);

            ChangeKing();
        }
    }

    void ChangeKing()
    {
        if (Slimes.Count > 0)
            King = Slimes[0].gameObject;
    }

    public void KillOther(GameObject otherSlime)
    {
        Slimes.Remove(otherSlime);
    }
}
