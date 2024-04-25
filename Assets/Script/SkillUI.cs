using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Skill skill;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI textmesh;


    [SerializeField] Image levelPanel;
    [SerializeField] Sprite Active;
    [SerializeField] Sprite nonActive;
    [SerializeField] Color activeColor;
    [SerializeField] Color nonActiveColor;

    [SerializeField] GameObject Description;
    [SerializeField] GameObject Buttons;
    bool descriptionONOFF = false;
    bool keySetMode = false;

    private void Awake()
    {
        image.sprite = skill.info.icon;
    }

    private void Update()
    {
        if (descriptionONOFF)
            Description.transform.position = Input.mousePosition + new Vector3(176, 76);

        if (keySetMode)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.Q);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "Q");
                Buttons.SetActive(false);
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.W);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "W");
                Buttons.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.E);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "E");
                Buttons.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.R);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "R");
                Buttons.SetActive(false);
            }
        }

    }

    public void UIUpdate()
    {
        string text = "";
        text += skill.SkillLevel;
        text += " / ";
        text += skill.info.values.Length;
        textmesh.text = text;

        image.color = skill.SkillLevel > 0 ? Color.white : nonActiveColor;
        levelPanel.sprite = skill.AcquisitionCondition() == true ? Active : nonActive;
        textmesh.color = skill.AcquisitionCondition() == true ? activeColor : nonActiveColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Description.SetActive(false);
        Buttons.SetActive(false);
        keySetMode = false;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.parent.GetComponentInParent<PlayerSkill>().SkillLevelUp(skill, 1);
            GetComponentInParent<SkillTree>().AllSkillUIUpdate();
            GameManager.Instance.Player.skill.CheckEvent();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Mouse Click Button : Middle");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!skill.GetComponent<CastableSkill>()) return;
            if (skill.SkillLevel < 1) return;

            keySetMode = true;

            Buttons.transform.position = eventData.position + new Vector2(65, 65);
            Buttons.SetActive(true);
            foreach (Button button in Buttons.GetComponentsInChildren<Button>())
                button.onClick.RemoveAllListeners();
            Buttons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.Q);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "Q");
                Buttons.SetActive(false);
            });
            Buttons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.W);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "W");
                Buttons.SetActive(false);
            });
            Buttons.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.E);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "E");
                Buttons.SetActive(false);
            });
            Buttons.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(delegate
            {
                GameManager.Instance.Player.skill.KeySkillApply((CastableSkill)skill, KeyCode.R);
                Debug.Log("키 등록 완료" + skill.info.name + "/" + "R");
                Buttons.SetActive(false);
            });
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Buttons.activeSelf) return;

        descriptionONOFF = true;
        Description.GetComponentInChildren<TextMeshProUGUI>().text = skill.GetDescription();
        Description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionONOFF = false;
        Description.SetActive(false);
    }
}
