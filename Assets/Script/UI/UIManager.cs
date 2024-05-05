using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    Text DieCount;

    private void Start()
    {
        DieCount = transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }

    public void PlayerDieEvent()
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
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<Spawner>().PlayerReSpawn();
    }

    public void SetScenarioUIText(string who, string content)
    {
        GameObject ScenarioTeller = transform.GetChild(1).gameObject;

        Text whoistelling = ScenarioTeller.transform.GetChild(1).GetComponent<Text>();
        Text SayWhat = ScenarioTeller.transform.GetChild(2).GetComponent<Text>();

        whoistelling.text = who;
        SayWhat.text = content;

        ScenarioTeller.SetActive(true);
        SetPlayerUIActive(false);
    }
    public void SetScenarioUIText_Teller(string who)
    {
        GameObject ScenarioTeller = transform.GetChild(1).gameObject;

        Text whoistelling = ScenarioTeller.transform.GetChild(1).GetComponent<Text>();

        whoistelling.text = who;


        ScenarioTeller.SetActive(true);
        SetPlayerUIActive(false);
    }

    public void SetScenarioUIText_Content(string content)
    {
        GameObject ScenarioTeller = transform.GetChild(1).gameObject;

        Text SayWhat = ScenarioTeller.transform.GetChild(2).GetComponent<Text>();

        SayWhat.text = content;

        ScenarioTeller.SetActive(true);
        SetPlayerUIActive(false);
    }

    public void SetScenarioUIImage(string what)
    {
        var illustObject = transform.GetChild(1).GetChild(4);
        SpriteRenderer illust = illustObject.GetComponent<SpriteRenderer>();
        Animator illustAnim = illustObject.GetComponent<Animator>();

        illustAnim.enabled = true;

        if(!illustAnim.GetCurrentAnimatorStateInfo(0).IsName(what))
            illustAnim.SetTrigger(what);

        //Debug.Log("이미지를 바꿉니다" + what);
    }
    public void SetScenarioUIFalse()
    {
        GameObject ScenarioTeller = transform.GetChild(1).gameObject;
        transform.GetComponentInChildren<Animator>().enabled = false;
        ScenarioTeller.SetActive(false);
    }
    public void SetPlayerUIActive(bool active)
    {
        var player = GameManager.Instance.Player;

        player.transform.GetChild(0).gameObject.SetActive(active);
        player.transform.GetChild(1).gameObject.SetActive(active);
    }

    public void SetDieMessage(string say)
    {
        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = say;
    }

    public void SetPlayerMovable(bool tf)
    {
        GameManager.Instance.Player.GetComponent<Move>().Movable = tf;
        GameManager.Instance.Player.GetComponent<PlayerSkill>().isMumchit = !tf;
    }

}
