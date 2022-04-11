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
    public void StartScenario(string storyname)
    {
        isStoryTelling = false;
        StorySequence = 0;
        ScenarioTell(FindStoryStart(storyname));
    }
    void Update()
    {
        if (isStoryTelling)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (FindStoryEnd() > StorySequence)
                    ScenarioTell();
                else
                {
                    isStoryTelling = false;
                    SetStoryUISee(false);
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
        player.transform.GetChild(0).gameObject.SetActive(!TorF);
        player.transform.GetChild(1).gameObject.SetActive(!TorF);
        if (TorF)
            player.transform.GetChild(1).GetComponent<PlayerHitbox>().init();
        transform.GetChild(1).gameObject.SetActive(TorF);
    }

    public void ScenarioTell(int startNum = 0)
    {
        if(!isStoryTelling)
        {
            StorySequence = startNum;
            SetStoryUISee(true);
            isStoryTelling = true;
        }

        GameObject ScenarioTeller = transform.GetChild(1).gameObject;

        Text whoistelling = ScenarioTeller.transform.GetChild(1).GetComponent<Text>();
        Text SayWhat = ScenarioTeller.transform.GetChild(2).GetComponent<Text>();
        string who = Story[StorySequence].Substring(0, Story[StorySequence].LastIndexOf(","));
        whoistelling.text = who;
        SetImage(who);
        if (ProceedingStoryCoroutine != null)
            StopCoroutine(ProceedingStoryCoroutine);

        string whatSaying = Story[StorySequence].Substring(Story[StorySequence].LastIndexOf(",") + 1);
        ProceedingStoryCoroutine = StartCoroutine(TellSlowly(SayWhat, whatSaying));

        StorySequence++;
    }
    IEnumerator TellSlowly(Text SayWhat, string whatSaying)
    {
        Text sayWhat = SayWhat;
        for (int i = 0; i <= whatSaying.Length; i++)
        {
            sayWhat.text = whatSaying.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void SetImage(string who)
    {
        SpriteRenderer illust = transform.GetChild(1).GetChild(4).GetComponent<SpriteRenderer>();
        Animator illustAnim = transform.GetChild(1).GetChild(4).GetComponent<Animator>();
        illust.color = originillustColor;
        switch (who)
        {
            case "???":
                illustAnim.SetTrigger("Slime");
                illust.color = new Color(0, 0, 0, 1f);
                break;
            case "친절한 슬라임":
                illustAnim.SetTrigger("Slime");
                break;
            case "지나가던 전사":
                illustAnim.SetTrigger("Warrior");
                break;
            default:
                illust.color = new Color(0, 0, 0, 0f);
                break;
        }
    }


}
