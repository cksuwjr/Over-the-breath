using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : Skill
{
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<IronPunch>().SkillLevel < skillBook.GetComponentInChildren<IronPunch>().info.values.Length) return false;
        return true;
    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [패시브:강화]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"매 세번째 공격의 피해량이 {values.ratio}%의 피해를 입히고 적을 띄웁니다.\n";


        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[강철 주먹] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }
}
