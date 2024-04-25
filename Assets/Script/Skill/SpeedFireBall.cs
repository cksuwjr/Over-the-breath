using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedFireBall : Skill
{
    SpeedFireBall() : base()
    {

    }
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<FireBall>().SkillLevel < 1) return false;
        if (skillBook.GetComponentInChildren<FireArrow>().SkillLevel < skillBook.GetComponentInChildren<FireArrow>().info.values.Length) return false;
        if (skillBook.GetComponentInChildren<StrongFireBall>().SkillLevel > 0) return false;
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

        description += $"�⺻������ ��ȭ�˴ϴ�. ��¡�Ͽ� �⺻������ �����մϴ�. �⺻������ ������ �����ϼ���.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[��ȭ�� ����] �ִ� ����\n";
            description += "[���� �� ���ϰ�] �̽���\n";
            description += "</color>";
        }

        return description;
    }
}
