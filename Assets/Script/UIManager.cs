using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Text DieCount;

    GameObject player;
    string[] Story;
    public bool isStoryTelling = false;
    Coroutine ProceedingStoryCoroutine;
    int StorySequence;
    string whoisTelling;
    GameObject UnActivedSpawner;

    Color originillustColor;
    private void Start()
    {
        DieCount = transform.GetChild(0).GetChild(2).GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        // 스토리 데이터베이스(txt) 경로로 텍스트 모두 읽어오기
        //Story
        Story = System.IO.File.ReadAllLines(System.IO.Path.Combine(Application.streamingAssetsPath ,"Story/Story.txt"));
        originillustColor = transform.GetChild(1).GetChild(4).GetComponent<SpriteRenderer>().color;

    }
    int FindStoryStart(string storyname)
    {
        int StoryStartNum;
        for (StoryStartNum = 0; StoryStartNum < Story.Length; StoryStartNum++)
        {
            if (Story[StoryStartNum] == storyname)
                break;
        }
        return StoryStartNum + 1;
    }
    public int FindStoryEnd()
    {
        int StoryEndNum;
        for (StoryEndNum = StorySequence; StoryEndNum < Story.Length; StoryEndNum++)
        {
            if (Story[StoryEndNum] == "End")
                break;
             
            
        }
        return StoryEndNum;

    }
    public IEnumerator StartScenario(string storyname, float startdelay = 0, bool MonsterFreeze = true, string CutScene = null)
        // startdelay는 컷씬 시작만 늦출겁니다.
    {
        isStoryTelling = true;
        StorySequence = 0;
        if (MonsterFreeze)
        {
            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                a.GetComponent<EnemyAI>().enabled = false;
                a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                a.GetComponent<Rigidbody2D>().Sleep();
            }
            foreach (GameObject a in GameObject.FindGameObjectsWithTag("Neutrality"))
            {
                a.GetComponent<EnemyAI>().enabled = false;
                a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                a.GetComponent<Rigidbody2D>().Sleep();
            }
            if (GameObject.FindGameObjectWithTag("SpawnerGroup") != null)
                foreach (EnemySpawner a in GameObject.FindGameObjectWithTag("SpawnerGroup").GetComponentsInChildren<EnemySpawner>())
                {
                    a.stopSpawn();
                }
        }

        string CAMname = storyname + "CAM";

        CAMON(CAMname);
        yield return new WaitForSeconds(startdelay);
        isStoryTelling = false; 

        if (CutScene != null && CutScene != "" && GameObject.Find("Director") != null)
        {
            GameObject.Find("Director").GetComponent<TimelineControl>().Play();
        }
        ScenarioTell(FindStoryStart(storyname), "Slow");
    }
    void Update()
    {
        if (isStoryTelling)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (FindStoryEnd() > StorySequence)
                {
                    if (ProceedingStoryCoroutine != null)
                        ScenarioTell(0, "Fast");
                    else
                        ScenarioTell(0, "Slow");
                }
                else
                {
                    if (ProceedingStoryCoroutine != null)
                        ScenarioTell(0, "Fast");
                    else
                    {
      
                        isStoryTelling = false;
                        SetStoryUISee(false);
                        CAMOFF();
                        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Enemy"))
                        {
                            a.GetComponent<EnemyAI>().enabled = true;
                            a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            a.GetComponent<Rigidbody2D>().WakeUp();
                        }
                        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Neutrality"))
                        {
                            a.GetComponent<EnemyAI>().enabled = true;
                            a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                            a.GetComponent<Rigidbody2D>().WakeUp();

                        }
                        if (GameObject.FindGameObjectWithTag("SpawnerGroup") != null)
                            foreach (EnemySpawner a in GameObject.FindGameObjectWithTag("SpawnerGroup").GetComponentsInChildren<EnemySpawner>())
                            {
                                a.resumeSpawn();
                            }
                    }
                }
            }


            // 완성시 삭제 예정
            if (Input.GetKeyDown(KeyCode.Return)) // 엔터키
            {
                    isStoryTelling = false;
                    SetStoryUISee(false);
                    CAMOFF();
                    foreach (GameObject a in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        a.GetComponent<EnemyAI>().enabled = true;
                        a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                        a.GetComponent<Rigidbody2D>().WakeUp();
                    }
                    foreach (GameObject a in GameObject.FindGameObjectsWithTag("Neutrality"))
                    {
                        a.GetComponent<EnemyAI>().enabled = true;
                        a.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                        a.GetComponent<Rigidbody2D>().WakeUp();

                    }
                    if (GameObject.FindGameObjectWithTag("SpawnerGroup") != null)
                        foreach (EnemySpawner a in GameObject.FindGameObjectWithTag("SpawnerGroup").GetComponentsInChildren<EnemySpawner>())
                        {
                            a.resumeSpawn();
                        }
            }
        }
    }
    public void PlayerDie()
    {
        StartCoroutine("CountingDieCount");
    }
    IEnumerator CountingDieCount()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        for (int second = 10; second > 0; second--)
        {
            DieCount.text = second.ToString();
            yield return new WaitForSeconds(1f);
        }
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject.Find("Spawner").GetComponent<Spawner>().PlayerReSpawn();
    }

    public void SetStoryUISee(bool TorF)
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
        player.transform.GetChild(0).gameObject.SetActive(!TorF);
        player.transform.GetChild(1).gameObject.SetActive(!TorF);
        if (TorF)
            player.transform.GetChild(1).GetComponent<PlayerHitbox>().init();
        transform.GetChild(1).gameObject.SetActive(TorF);
    }

    public void ScenarioTell(int startNum = 0, string SloworFast = null)
    {
        if(!isStoryTelling)
        {
            StorySequence = startNum;
            SetStoryUISee(true);
            isStoryTelling = true;
        }
        else
        {
            if(SloworFast == "Fast")
                StorySequence -= 1;
        }

        GameObject ScenarioTeller = transform.GetChild(1).gameObject;

        Text whoistelling = ScenarioTeller.transform.GetChild(1).GetComponent<Text>();
        Text SayWhat = ScenarioTeller.transform.GetChild(2).GetComponent<Text>();
        try
        {
        string who = Story[StorySequence].Substring(0, Story[StorySequence].LastIndexOf(":"));
        whoistelling.text = who;

            string Imotion = Story[StorySequence].Substring(Story[StorySequence].LastIndexOf(":") + 1, Story[StorySequence].LastIndexOf(",") - Story[StorySequence].LastIndexOf(":") - 1);
            SetImage(Imotion);
        }
        catch { Debug.LogError(":이 빠졌잖아 바보야 메모장 고쳐라"); }
        if (ProceedingStoryCoroutine != null)
            StopCoroutine(ProceedingStoryCoroutine);

        try
        {
            string whatSaying = Story[StorySequence].Substring(Story[StorySequence].LastIndexOf(",") + 1);
            ProceedingStoryCoroutine = StartCoroutine(Tell(SayWhat, whatSaying, SloworFast));
        }
        catch { Debug.LogError(",이 빠졌잖아 바보야 메모장 고쳐라"); }
        StorySequence++;
    }
    IEnumerator Tell(Text SayWhat, string whatSaying, string SloworQuick)
    {
        Text sayWhat = SayWhat;
        if (SloworQuick == "Slow")
        {
            for (int i = 0; i <= whatSaying.Length; i++)
            {
                sayWhat.text = whatSaying.Substring(0, i);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
        else
        {
            sayWhat.text = whatSaying;
            yield return null;
        }
        ProceedingStoryCoroutine = null;
    }

    void SetImage(string what)
    {
        SpriteRenderer illust = transform.GetChild(1).GetChild(4).GetComponent<SpriteRenderer>();
        Animator illustAnim = transform.GetChild(1).GetChild(4).GetComponent<Animator>();
        illust.color = originillustColor;

        // 만약 사진이 안뜬다면 UI에서 RenderCamera를 PlayerCamera로 두고 다시 확인해보시오"

        switch (what) {
            case "":
                break;
            default:
                illustAnim.SetTrigger(what);
                break;

        }
        //switch (who)
        //{
        //    case "???":
        //        illustAnim.SetTrigger("Slime");
        //        illust.color = new Color(0, 0, 0, 1f);
        //        break;
        //    case "슬라임":
        //        illustAnim.SetTrigger("Slime");
        //        break;
        //    case "모험가":
        //        illustAnim.SetTrigger("Warrior");
        //        break;
        //    default:
        //        illust.color = new Color(0, 0, 0, 0f);
        //        break;
        //}
    }
    void CAMOFF()
    {
        GameObject UnActiveCAMS = GameObject.Find("UnActiveCAMS");
        if (UnActiveCAMS != null)
        {
            for (int i = 0; i < UnActiveCAMS.transform.childCount; i++)
                UnActiveCAMS.transform.GetChild(i).gameObject.SetActive(false);
        }
        else
            Debug.Log("현재Scene에 UnActiveCAMS가 없어 끌 다른 카메라가 없습니다.");
    }
    void CAMON(string CAMname)
    {
        GameObject UnActiveCAMS = GameObject.Find("UnActiveCAMS");
        if (UnActiveCAMS != null)
        {
            for (int i = 0; i < UnActiveCAMS.transform.childCount; i++)
                if (UnActiveCAMS.transform.GetChild(i).name == CAMname)
                {
                    UnActiveCAMS.transform.GetChild(i).gameObject.SetActive(true);
                    break;
                }
        }
        else
            Debug.Log("현재Scene에 UnActiveCAMS가 없어 카메라 전환하지 않습니다.");
    }

}
