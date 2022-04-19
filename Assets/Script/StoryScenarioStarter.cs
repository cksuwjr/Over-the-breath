using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScenarioStarter : MonoBehaviour
{
    UIManager officialUI;
    public GameObject SpawnSomeThing;
    public float SpawnPlusX;
    public float SpawnPlusY;
    public string SpawnedDieAndStartStory;
    void Start()
    {
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            if (SpawnSomeThing != null && SpawnSomeThing.tag == "Enemy")
            {
                GameObject Spawned = Instantiate(SpawnSomeThing, transform.position + new Vector3(SpawnPlusX, SpawnPlusY, 0), Quaternion.identity);
                Spawned.GetComponent<EnemyAI>().SetAttackTarget(GameObject.FindGameObjectWithTag("Player"));
                Spawned.GetComponent<EnemyAI>().DieAndStartStory = SpawnedDieAndStartStory;
            }
            officialUI.StartCoroutine(officialUI.StartScenario(name,0));
            Destroy(gameObject);
        }
            
    }


}
