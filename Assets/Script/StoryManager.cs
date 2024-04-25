using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    static string[] storys;
    public bool nowStoryReading = false;
    int Sequence;
    bool telling = false;
    bool skip = false;

    public void StoryLoad()
    {
        storys = System.IO.File.ReadAllLines(System.IO.Path.Combine(Application.streamingAssetsPath,"Story/Story.txt"));
    }
    int FindStoryStart(string storyname)
    {
        int StoryStartNum;
        for (StoryStartNum = 0; StoryStartNum < storys.Length; StoryStartNum++)
        {
            if (storys[StoryStartNum] == storyname)
                break;
        }
        return StoryStartNum + 1;
    }
    public int FindStoryEnd()
    {
        int StoryEndNum;
        for (StoryEndNum = Sequence; StoryEndNum < storys.Length; StoryEndNum++)
        {
            if (storys[StoryEndNum] == "End")
                break;

        }
        return StoryEndNum;

    }

    void Update()
    {
        if (!nowStoryReading) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (FindStoryEnd() > Sequence)
            {
                if (telling)
                {
                    skip = true; 
                    return;
                }
                Scenario(Sequence);
            }
            else
            {
                nowStoryReading = false;
                GameManager.Instance.UIManager.SetScenarioUIFalse();
                GameManager.Instance.UIManager.SetPlayerUIActive(true);

                GameManager.Instance.CameraManager.CAMOFF();
                //GameManager.Instance.MonsterFreeze(false);
            }
        }
    }

    public void StartScenario(string storyname)
    {
        if (storys == null) StoryLoad();
        if (storyname == "StoryName") return;

        var startLine = FindStoryStart(storyname);

        DataManager.Instance.data.AddReadStory(storyname);
        DataManager.Instance.SaveGameData();
        Scenario(startLine);
    }

    public void Scenario(int startNum = 0)
    {
        if (!nowStoryReading)
        {
            Sequence = startNum;
            nowStoryReading = true;
        }

        AssignDialogue();
    }
    void AssignDialogue()
    {
        try
        {
            string who = storys[Sequence].Substring(0, storys[Sequence].LastIndexOf(":"));

            string saying = storys[Sequence].Substring(storys[Sequence].LastIndexOf(",") + 1);

            string emotion = storys[Sequence].Substring(storys[Sequence].LastIndexOf(":") + 1, storys[Sequence].LastIndexOf(",") - storys[Sequence].LastIndexOf(":") - 1);

            StartCoroutine(Tell(who, saying, emotion));
        }
        catch {
            Debug.LogError("스토리 읽어오기에 실패했어요");
            StoryLoad();
            AssignDialogue();
        }
    }

    IEnumerator Tell(string who, string saying, string emotion)
    {
        var uiManager = GameManager.Instance.UIManager;
        telling = true;
        for (int i = 0; i <= saying.Length; i++)
        {
            uiManager.SetScenarioUIText(who, saying.Substring(0, i));
            uiManager.SetScenarioUIImage(emotion);
            if(!skip)
                yield return new WaitForSecondsRealtime(0.02f);
        }
        telling = false;
        skip = false;
        Sequence++;
    }
    
}
