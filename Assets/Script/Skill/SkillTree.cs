using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    SkillUI[] skillUIs;

    [SerializeField] TextMeshProUGUI skillPointUI;
    //private void Start()
    //{
    //    AllSkillUIUpdate();
    //}

    public void AllSkillUIUpdate()
    {
        skillPointUI.text = GameManager.Instance.Player.skill.SkillPoint.ToString();
        skillUIs = GetComponentsInChildren<SkillUI>();
        foreach (SkillUI skillUI in skillUIs)
            skillUI.UIUpdate();
    }
}
