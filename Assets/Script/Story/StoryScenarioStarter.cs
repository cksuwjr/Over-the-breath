using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StoryScenarioStarter : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> SpawnedThing;

    [SerializeField] private PlayableDirector dir;

    private void Awake()
    {
        foreach (GameObject n in SpawnedThing)
        {
            n.GetComponent<Monster>().binded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.StoryManager.nowStoryReading) return;
        if (DataManager.Instance.data != null)
            if(DataManager.Instance.data.isReadStory(name)) return;

        if (collision.tag == "Player")
        {
            if (dir) dir.Play();

            foreach (GameObject n in SpawnedThing)
            {
                n.GetComponent<Monster>().binded = false;
                n.GetComponent<Rigidbody2D>().velocity = new Vector2(0, n.GetComponent<Rigidbody2D>().velocity.y);
                n.GetComponent<Monster>().SetAttackTarget(GameManager.Instance.Player.gameObject);
            }
            GameManager.Instance.StoryManager.StartScenario(name);
            GameManager.Instance.SaveGame();

            gameObject.SetActive(false);
        }  
    }


}
