using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : Skill
{
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<StrongAttack>().SkillLevel < skillBook.GetComponentInChildren<StrongAttack>().info.values.Length) return false;
        return true;
    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [패시브]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"공격에 치명타가 적용되며 치명타 확률이 {values.ratio}%만큼 증가합니다. 치명타 적용시 데미지가 250%로 적용됩니다.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[강력한 한방] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }

    public bool isCritical(int amplification = 1)
    {
        return Random.Range(0, 101) < info.values[SkillLevel - 1].ratio * amplification;
    }
}
