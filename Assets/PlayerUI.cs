using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Status PlayerStat;      // �÷��̾� ���� ��������   (���� ���濡 ���� UI��ȭ)

    GameObject HPUI;            // ü�¹� HPbar UI          (ü��UI ����)             
    GameObject UI;          // Official Scene common UI (���UI, ���丮 ��� ����)

    void Awake()
    {
        UI = GameObject.Find("UI");
        HPUI = transform.GetChild(0).gameObject;

        PlayerStat = gameObject.GetComponentInParent<Status>();
    }

    // Player UI ������Ʈ (ü�¹� ���)
    public void PlayerUIUpdate()
    {
        

        HPUIUpdate();

    }

    void HPUIUpdate()
    {
        if (PlayerStat.HP > PlayerStat.MaxHp)
            PlayerStat.HP = PlayerStat.MaxHp;

        HPUI.transform.GetChild(1).GetComponent<Image>().fillAmount = PlayerStat.HP / PlayerStat.MaxHp; // ü�¹� �̹��� ��ȯ
        string hpvalue = "";
        hpvalue += PlayerStat.HP.ToString();
        hpvalue += " / ";
        hpvalue += PlayerStat.MaxHp.ToString();
        HPUI.transform.GetChild(2).GetComponent<Text>().text = hpvalue;
    }
    // Player ����� ���� UI ����
    public void PopupPlayerDieUI()
    {
        UI.GetComponent<UIManager>().PlayerDie();
    }
}
