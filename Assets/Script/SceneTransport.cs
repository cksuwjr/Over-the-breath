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

    GameObject NowVirtualCAM;
    GameObject NowCamera;

    bool OnOff = true;
    GameObject beforePlayer;
    GameObject beforeCheckList;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //NowCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //NowVirtualCAM = GameObject.Find("CAM");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(OnOff)
            if (collision.transform.tag == "Player")
            {
                StartCoroutine(NewSceneStart());
                beforePlayer = GameObject.FindGameObjectWithTag("Player");
                beforeCheckList = GameObject.FindGameObjectWithTag("AcheiveList");
                OnOff = false;
            }
    }
    IEnumerator NewSceneStart()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;
        
        
        
        NowCamera = GameObject.FindGameObjectWithTag("MainCamera");
        DontDestroyOnLoad(NowCamera);


        NowVirtualCAM = GameObject.Find("CAM");
        DontDestroyOnLoad(NowVirtualCAM);




        AsyncOperation isComplete = SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);// Additive);
        isComplete.allowSceneActivation = false;
        while (!isComplete.isDone)
        {
            yield return null;
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

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(player != beforePlayer)
                Destroy(player);
        }
        foreach (GameObject Acheivelist in GameObject.FindGameObjectsWithTag("AcheiveList"))
        {
            if (Acheivelist != beforeCheckList)
                Destroy(Acheivelist);
        }
        NowCamera.tag = "Untagged";
        NowVirtualCAM.name = "BeforeCAM";
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = NowCamera.transform.position;
        Destroy(NowCamera);
        Destroy(NowVirtualCAM);
        NowCamera = GameObject.FindGameObjectWithTag("MainCamera");
        NowVirtualCAM = GameObject.Find("CAM");
        NowVirtualCAM.GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Move>().SceneChangeUIChanged();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Skill>().SceneChangeUIChanged();


        if (StartStory != "")
            GameObject.Find("UI").GetComponent<UIManager>().StartCoroutine(GameObject.Find("UI").GetComponent<UIManager>().StartScenario(StartStory,startdelay));

        
        Destroy(gameObject);
    }
}
