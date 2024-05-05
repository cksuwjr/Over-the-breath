
public class StrongFireBall : Skill
{
    StrongFireBall() : base()
    {

    }
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<FireBall>().SkillLevel < 1) return false;
        if (skillBook.GetComponentInChildren<FireForce>().SkillLevel < skillBook.GetComponentInChildren<FireForce>().info.values.Length) return false;
        if (skillBook.GetComponentInChildren<SpeedFireBall>().SkillLevel > 0) return false;
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
        description += $"���濡 �ҵ��̸� �߻��Ͽ� ���߽� �ּ�, ���ݷ��� {10}%�� ���ؿ��� ��¡�������� ���� �ִ�, ���ݷ��� {values.ratio}% ���ظ� �����ϴ�.\n";
        description += $"�ִ�ġ���� ��¡�� ũ��Ƽ���� �߻��Ͽ� �� �� ���� {values.ratio * 2}% ���ظ� �����ϴ�.\n";
        description += $"��Ÿ�� : {values.cooldownTime}��\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[ȭ�� ���� ��ȭ] �ִ� ����\n";
            description += "[���� �� ������] �̽���\n";
            description += "</color>";
        }

        return description;
    }
}

