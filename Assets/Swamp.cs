using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : MonoBehaviour
{
    private List<GameObject> Slimes;

    private void Start()
    {
        Slimes = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Neutrality" && col.name == "Slime")
        {
            Slimes.Add(col.gameObject);
            Debug.Log("슬라임추가됨");
            Debug.Log("현재 " + Slimes.Count + "마리");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Neutrality" && col.name == "Slime")
        {
            Slimes.Remove(col.gameObject);
            Debug.Log("슬라임나감");
            Debug.Log("현재 " + Slimes.Count + "마리");
        }
    }
}
