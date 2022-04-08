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

    private void Start()
    {
        DieCount = transform.GetChild(0).GetChild(2).GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        // 스토리 데이터베이스(txt) 경로로 텍스트 모두 읽어오기
        Story = System.IO.File.ReadAllLines(@"C:\Users\cksuw\Desktop\UnityProject\Over-the-breath\Assets\Resources\Story\Story.txt");
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
    int FindStoryEnd()
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



}
