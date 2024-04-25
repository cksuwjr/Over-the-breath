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
        description += " [패시브:강화]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"기본공격이 강화됩니다. 차징하여 기본공격을 연발합니다. 기본공격의 설명을 참고하세요.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[불화살 세례] 최대 레벨\n";
            description += "[더욱 더 강하게] 미습득\n";
            description += "</color>";
        }

        return description;
    }
}
