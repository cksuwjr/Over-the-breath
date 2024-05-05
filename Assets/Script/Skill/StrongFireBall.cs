
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
        description += " [패시브:불덩이 강화]" + "\n";
        description += "</color>";
        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"불덩이가 강화되었습니다!\n";
        description += $"전방에 불덩이를 발사하여 적중시 최소, 공격력의 {10}%의 피해에서 차징게이지에 따라 최대, 공격력의 {values.ratio}% 피해를 입힙니다.\n";
        description += $"최대치까지 차징시 크리티컬이 발생하여 그 두 배인 {values.ratio * 2}% 피해를 입힙니다.\n";
        description += $"쿨타임 : {values.cooldownTime}초\n";

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

