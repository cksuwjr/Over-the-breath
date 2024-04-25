using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    Status PlayerStat;      // 플레이어 스탯 가져오기   (스탯 변경에 따른 UI변화)

    GameObject HPUI;            // 체력바 HPbar UI          (체력UI 변경)             
    GameObject StatUI;
    GameObject UI;          // Official Scene common UI (사망UI, 스토리 등등 띄우기)
    GameObject ExpUI;
    GameObject SkillUI;
    GameObject PauseUI;

    [SerializeField] Image Gage;
    void Awake()
    {
        UI = GameObject.Find("UI");

        HPUI = transform.GetChild(0).GetChild(0).gameObject;
        ExpUI = transform.GetChild(1).GetChild(0).gameObject;
        StatUI = transform.GetChild(2).GetChild(0).gameObject;
        SkillUI = transform.GetChild(3).gameObject;
        PauseUI = transform.GetChild(5).gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SkillUI.SetActive(!SkillUI.activeSelf);
        else if (SkillUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            SkillUI.SetActive(false);
        else if (!SkillUI.activeSelf && !PauseUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PauseUI.SetActive(true);
        }
        else if (PauseUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            PauseUI.SetActive(false);
        }
    }

    // Player UI 업데이트 (체력바 등등)
    public void PlayerUIUpdate()
    {
        HPUIUpdate();
        ExpUIUpdate();
        StatUIUpdate();
    }

    void HPUIUpdate()
    {
        if (HPUI == null) return;

        if(PlayerStat == null)
            PlayerStat = gameObject.GetComponentInParent<Status>();

        HPUI.transform.GetChild(2).GetComponent<Image>().fillAmount = PlayerStat.HP / PlayerStat.MaxHp; // 체력바 이미지 변환
        string hpvalue = "";
        hpvalue += PlayerStat.HP.ToString();
        hpvalue += " / ";
        hpvalue += PlayerStat.MaxHp.ToString();
        HPUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = hpvalue;
    }
    void StatUIUpdate()
    {
        if (StatUI == null) return;

        if (PlayerStat == null)
            PlayerStat = gameObject.GetComponentInParent<Status>();

        StatUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerStat.AttackPower.ToString();
    }
    void ExpUIUpdate()
    {
        if (ExpUI == null) return;

        if (PlayerStat == null)
            PlayerStat = gameObject.GetComponentInParent<Status>();

        ExpUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerStat.Level.ToString();

        ExpUI.transform.GetChild(3).GetComponent<Image>().fillAmount = PlayerStat.Exp / PlayerStat.MaxExp; // 체력바 이미지 변환
        string hpvalue = "";
        hpvalue += PlayerStat.Exp.ToString();
        hpvalue += " / ";
        hpvalue += PlayerStat.MaxExp.ToString();
        ExpUI.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hpvalue;
    }

    public void SkillUIUpdate()
    {
        SkillUI.GetComponentInChildren<SkillTree>().AllSkillUIUpdate();
    }

    // Player 사망시 공식 UI 띄우기
    public void PopupPlayerDieUI()
    {
        UI = GameObject.Find("UI");
        UI.GetComponent<UIManager>().PlayerDieEvent();
    }

    public void SetGage(float value)
    {
        Gage.fillAmount = value;
    }

    public void OnClick_Resume()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
    }

    public void OnClick_Exit()
    {
        Application.Quit();
    }
}
