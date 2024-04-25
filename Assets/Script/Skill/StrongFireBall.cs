
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
        description += " [�нú�:��ȭ]" + "\n";
        description += "</color>";
        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"�⺻������ ��ȭ�˴ϴ�. ��¡�Ͽ� �ּ� ��°� �ִ� ����� ����� �ּ� ����� �پ�� ��� ��Ÿ���� ���� �����Ͽ����ϴ�. �⺻������ ������ �����ϼ���.\n";

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

