using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScenarioStarter : MonoBehaviour
{
    UIManager officialUI;
    [SerializeField]
    public List<GameObject> SpawnSomeThing;
    public float SpawnPlusX;
    public float SpawnPlusY;
    public float SpawnLandomPlusAll;
    public string SpawnedDieAndStartStory;


    public float StartDelay = 0;
    public bool MonsterFreeze = true;
    public string CutScene;

    public List<string> CheckCondition;
    public string IfSpawnedDieCheckAcheivePlease;

    void Start()
    {
        officialUI = GameObject.Find("UI").GetComponent<UIManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            bool Checking = true;
            AcheiveList acheiveList = GameObject.Find("CheckList").GetComponent<AcheiveList>();
            foreach (string condition in CheckCondition)
            {
                if (!acheiveList.CheckPlease(condition))
                    Checking = false;
            }

            if (Checking)
            {
                if (SpawnSomeThing.Count > 0)
                {
                    foreach (GameObject n in SpawnSomeThing)
                    {
                        GameObject Spawned = Instantiate(n, transform.position + new Vector3(SpawnPlusX + Random.Range(-SpawnLandomPlusAll, SpawnLandomPlusAll), SpawnPlusY + Random.Range(0, SpawnLandomPlusAll), 0), Quaternion.identity);
                        if (n != null && (n.tag == "Enemy" || n.tag == "Neutrality"))
                        {
                            if (Spawned.tag == "Enemy" || Spawned.tag == "Neutrality")
                            {
                                Spawned.GetComponent<EnemyAI>().SetAttackTarget(GameObject.FindGameObjectWithTag("Player"));
                                Spawned.GetComponent<EnemyAI>().DieAndStartStory = SpawnedDieAndStartStory;
                                Spawned.GetComponent<EnemyAI>().IfIDieCheckAcheivePlease = IfSpawnedDieCheckAcheivePlease;
                            }
                        }
                    }
                }
                officialUI.StartCoroutine(officialUI.StartScenario(name, StartDelay, MonsterFreeze, CutScene));
                Destroy(gameObject);
            }
        }
            
    }


}
