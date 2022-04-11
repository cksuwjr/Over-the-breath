using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Status PlayerStat;      // 플레이어 스탯 가져오기   (스탯 변경에 따른 UI변화)

    GameObject HPUI;            // 체력바 HPbar UI          (체력UI 변경)             
    GameObject StatUI;
    GameObject UI;          // Official Scene common UI (사망UI, 스토리 등등 띄우기)

    void Awake()
    {
        UI = GameObject.Find("UI");

        HPUI = transform.GetChild(0).gameObject;
        StatUI = transform.GetChild(1).gameObject;

        PlayerStat = gameObject.GetComponentInParent<Status>();

    }

    // Player UI 업데이트 (체력바 등등)
    public void PlayerUIUpdate()
    {
        HPUIUpdate();
        StatUIUpdate();
    }

    void HPUIUpdate()
    {
        if (PlayerStat.HP > PlayerStat.MaxHp)
            PlayerStat.HP = PlayerStat.MaxHp;

        HPUI.transform.GetChild(1).GetComponent<Image>().fillAmount = PlayerStat.HP / PlayerStat.MaxHp; // 체력바 이미지 변환
        string hpvalue = "";
        hpvalue += PlayerStat.HP.ToString();
        hpvalue += " / ";
        hpvalue += PlayerStat.MaxHp.ToString();
        HPUI.transform.GetChild(2).GetComponent<Text>().text = hpvalue;
    }
    void StatUIUpdate()
    {
        StatUI.transform.GetChild(2).GetComponent<Text>().text = PlayerStat.AttackPower.ToString();
    }
    // Player 사망시 공식 UI 띄우기
    public void PopupPlayerDieUI()
    {
        UI.GetComponent<UIManager>().PlayerDie();
    }
}
