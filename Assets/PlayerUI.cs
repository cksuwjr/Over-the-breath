using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Status PlayerStat;      // �÷��̾� ���� ��������   (���� ���濡 ���� UI��ȭ)

    Image HPbar;            // ü�¹� HPbar UI          (ü��UI ����)             
    GameObject UI;          // Official Scene common UI (���UI, ���丮 ��� ����)

    void Start()
    {
        UI = GameObject.Find("UI");
        HPbar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        PlayerStat = gameObject.GetComponentInParent<Status>();
    }

    // Player UI ������Ʈ (ü�¹� ���)
    public void PlayerUIUpdate()
    {
        if (PlayerStat.HP > PlayerStat.MaxHp)
            PlayerStat.HP = PlayerStat.MaxHp;

        HPbar.fillAmount = PlayerStat.HP / PlayerStat.MaxHp;
    }

    // Player ����� ���� UI ����
    public void PopupPlayerDieUI()
    {
        UI.GetComponent<UIManager>().PlayerDie(PlayerStat);
    }
}
