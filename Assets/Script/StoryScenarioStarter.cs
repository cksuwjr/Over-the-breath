using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScenarioStarter : MonoBehaviour
{
    UIManager officialUI;
    void Start()
    {
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            officialUI.StartCoroutine(officialUI.StartScenario(name,0));
            Destroy(gameObject);
        }
            
    }
}
