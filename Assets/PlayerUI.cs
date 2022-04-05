using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Status PlayerStat;      // 플레이어 스탯 가져오기   (스탯 변경에 따른 UI변화)

    Image HPbar;            // 체력바 HPbar UI          (체력UI 변경)             
    GameObject UI;          // Official Scene common UI (사망UI, 스토리 등등 띄우기)

    void Start()
    {
        UI = GameObject.Find("UI");
        HPbar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        PlayerStat = gameObject.GetComponentInParent<Status>();
    }

    // Player UI 업데이트 (체력바 등등)
    public void PlayerUIUpdate()
    {
        if (PlayerStat.HP > PlayerStat.MaxHp)
            PlayerStat.HP = PlayerStat.MaxHp;

        HPbar.fillAmount = PlayerStat.HP / PlayerStat.MaxHp;
    }

    // Player 사망시 공식 UI 띄우기
    public void PopupPlayerDieUI()
    {
        UI.GetComponent<UIManager>().PlayerDie(PlayerStat);
    }
}
