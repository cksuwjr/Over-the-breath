using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcheiveList : MonoBehaviour
{

    public bool isDutorialWarrior1Kill;
    public bool isDutorialWarrior2Kill;
    public bool isDutorialHoneySlimeKill;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool CheckPlease(string What)
    {
        switch (What)
        {
            case "isDutorialWarrior1Kill":
                return isDutorialWarrior1Kill;
            case "isDutorialWarrior2Kill":
                return isDutorialWarrior2Kill;
            case "isDutorialHoneySlimeKill":
                return isDutorialHoneySlimeKill;
        }
        Debug.Log("해당하는 체크 리스트가 없습니다.");
        return false;
    }
    public void Add(string What)
    {
        switch (What)
        {
            case "isDutorialWarrior1Kill":
                isDutorialWarrior1Kill = true;
                break;
            case "isDutorialWarrior2Kill":
                isDutorialWarrior2Kill = true;
                break;
            case "isDutorialHoneySlimeKill":
                isDutorialHoneySlimeKill = true;
                break;
        }
    }
}
