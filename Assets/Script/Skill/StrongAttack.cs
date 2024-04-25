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
        description += " [�нú�:��ȭ]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"�� ����° ������ ���ط��� {values.ratio}%�� ���ظ� ������ ���� ���ϴ�.\n";


        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[��ö �ָ�] �ִ� ����\n";
            description += "</color>";
        }

        return description;
    }
}
