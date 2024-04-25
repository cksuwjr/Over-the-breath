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
        description += " [�нú�]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"���ݿ� ġ��Ÿ�� ����Ǹ� ġ��Ÿ Ȯ���� {values.ratio}%��ŭ �����մϴ�. ġ��Ÿ ����� �������� 250%�� ����˴ϴ�.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[������ �ѹ�] �ִ� ����\n";
            description += "</color>";
        }

        return description;
    }

    public bool isCritical(int amplification = 1)
    {
        return Random.Range(0, 101) < info.values[SkillLevel - 1].ratio * amplification;
    }
}
