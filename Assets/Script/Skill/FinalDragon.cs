using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDragon : CastableSkill
{
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;

        if (skillBook.GetComponentInChildren<Critical>().SkillLevel < skillBook.GetComponentInChildren<Critical>().info.values.Length) return false;
        if (skillBook.GetComponentInChildren<Assassination>().SkillLevel < skillBook.GetComponentInChildren<Assassination>().info.values.Length) return false;
        return true;
    }

    public override void Casting(GameObject attacker, Vector3 position, Vector3 direction)
    {
        GameManager.Instance.StartCoroutine(Cast(attacker, position, direction));
    }

    public virtual IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;

        isCasting = true;
        var anim = attacker.GetComponent<Animator>();


        float time = info.values[SkillLevel - 1].count;

        var stat = attacker.GetComponent<Status>();

        int plusAttackPower = (int)(stat.AttackPower * (info.values[SkillLevel - 1].ratio / 100f));
        float plusSpeed = stat.MoveSpeed * (info.values[SkillLevel - 1].basicValue / 100f);

        stat.AttackPower += plusAttackPower;
        stat.MoveSpeed += plusSpeed;
        anim.speed = 1 + (info.values[SkillLevel - 1].ratio / 100f);

        float timer = 0;
        float dot = stat.MaxHp * 0.02f;
        while (timer < time && stat.HP > (dot * 2))
        {
            if (attacker == null) break;
            attacker.GetComponent<Player>().GetDamage((int)dot);
            timer++;
            yield return new WaitForSeconds(1);
        }
        stat.AttackPower -= plusAttackPower;
        stat.MoveSpeed = stat.BasicSpeed;
        attacker.GetComponent<Player>().GetDamage((int)dot);
        anim.speed = 1;

        isCasting = false;
        yield return null;
    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [시전가능스킬]" + "\n";
        description += "</color>";

        var skillBook = GameManager.Instance.Player.skill.skillBook;

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];
        description += $"{values.count}초간 {values.ratio}% 만큼 공격력과 공격속도가 증가하고 이동속도가 {values.basicValue}%만큼 증가합니다.\n";
        description += $"단 매 초 최대체력의 2%만큼 피해를 입으며 체력이 여유치 않을시 효과가 일찍 종료됩니다.";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[암살] 최대 레벨\n";
            description += "[치명타] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }
}
