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
        description += " [패시브:불덩이 강화]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"불덩이가 강화되었습니다!\n";
        description += $"전방에 불덩이를 연속 발사하여 적중시마다 공격력의 {values.ratio}%의 피해를 입힙니다.\n";
        description += $"최대 {values.count}개 만큼 발사 가능하며 발사시마다 불덩이의 데미지가 공격력의 1%만큼 증가합니다.\n";
        description += $"개당 최소데미지 {values.ratio}% ~ 최대데미지 {values.ratio + (values.count < 70 ? values.count : 70)}%\n";
        description += $"쿨타임 : {values.cooldownTime}초\n";
        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[스피드 증가] 최대 레벨\n";
            description += "[더욱 더 강하게] 미습득\n";
            description += "</color>";
        }

        return description;
    }
}
