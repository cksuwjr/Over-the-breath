using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;
public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    public static string sceneText;

    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI tipText;

    float remainTime = 6f;

    public static bool Loading = false;

    private void Start()
    {
        //if(GameManager.Instance)
        //    GameManager.Instance.UIManager.SetPlayerUIActive(false);

        progressBar.fillAmount = 0;
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void ReloadScene()
    {
        nextScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
    }
    public static void SetTipText(string text)
    {
        sceneText = text;
    }
    IEnumerator LoadScene()
    {
        Loading = true;
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        if (sceneText != null && sceneText != "")
            tipText.text = sceneText;

        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer / remainTime);
                if (progressBar.fillAmount == 1.0f)
                {
                    Loading = false;
                    op.allowSceneActivation = true;
                    //if(GameManager.Instance)
                    //    GameManager.Instance.UIManager.SetPlayerUIActive(true);
                    yield break;
                }
            }
        }
    }
}