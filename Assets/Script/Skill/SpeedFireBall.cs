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
        if (skillBook.GetComponentInChildren<DrawFire>().SkillLevel < skillBook.GetComponentInChildren<DrawFire>().info.values.Length) return false;
        if (skillBook.GetComponentInChildren<StrongFireBall>().SkillLevel > 0) return false;
        return true;
    }
    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [�нú�:�ҵ��� ��ȭ]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"�ҵ��̰� ��ȭ�Ǿ����ϴ�!\n";
        description += $"���濡 �ҵ��̸� ���� �߻��Ͽ� ���߽ø��� ���ݷ��� {values.ratio}%�� ���ظ� �����ϴ�.\n";
        description += $"�ִ� {values.count}�� ��ŭ �߻� �����ϸ� �߻�ø��� �ҵ����� �������� ���ݷ��� 1%��ŭ �����մϴ�.\n";
        description += $"���� �ּҵ����� {values.ratio}% ~ �ִ뵥���� {values.ratio + (values.count < 70 ? values.count : 70)}%\n";
        description += $"��Ÿ�� : {values.cooldownTime}��\n";
        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[���ǵ� ����] �ִ� ����\n";
            description += "[���� �� ���ϰ�] �̽���\n";
            description += "</color>";
        }

        return description;
    }
}
