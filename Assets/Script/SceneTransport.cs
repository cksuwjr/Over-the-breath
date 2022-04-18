using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class SceneTransport : MonoBehaviour
{
    public string NextScene;
    public string StartStory;
    public float startdelay;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(NewSceneStart());
            Debug.Log("코루틴시작");
            Debug.Log(name);
        }
    }
    IEnumerator NewSceneStart()
    {
        AsyncOperation isComplete = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);// Additive);
        isComplete.allowSceneActivation = false;
        while (!isComplete.isDone)
        {
            yield return null;
            Debug.Log(isComplete.progress);
            if (isComplete.progress >= 0.9f)
            {
                isComplete.allowSceneActivation = true;
                yield return new WaitForSeconds(0.1f);
                HiNewScene();
                break;
            }
        }

        
    }
    void HiNewScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(NextScene));
        GameObject.Find("CAM").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;

        if (StartStory != null)
            GameObject.Find("UI").GetComponent<UIManager>().StartCoroutine(GameObject.Find("UI").GetComponent<UIManager>().StartScenario(StartStory,startdelay));
        if (GameObject.Find("Director") != null)
        {
            GameObject.Find("Director").GetComponent<TimelineControl>().Play();
        }
        Destroy(gameObject);
    }
}
