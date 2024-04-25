
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
        description += " [패시브:강화]" + "\n";
        description += "</color>";
        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"기본공격이 강화됩니다. 차징하여 최소 출력과 최대 출력이 생기고 최소 출력이 줄어든 대신 쿨타임이 대폭 감소하였습니다. 기본공격의 설명을 참고하세요.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[화염 위력 강화] 최대 레벨\n";
            description += "[더욱 더 빠르게] 미습득\n";
            description += "</color>";
        }

        return description;
    }
}

