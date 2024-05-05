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
        description += " [�������ɽ�ų]" + "\n";
        description += "</color>";

        var skillBook = GameManager.Instance.Player.skill.skillBook;

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];
        description += $"{values.count}�ʰ� {values.ratio}% ��ŭ ���ݷ°� ���ݼӵ��� �����ϰ� �̵��ӵ��� {values.basicValue}%��ŭ �����մϴ�.\n";
        description += $"�� �� �� �ִ�ü���� 2%��ŭ ���ظ� ������ ü���� ����ġ ������ ȿ���� ���� ����˴ϴ�.";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[�ϻ�] �ִ� ����\n";
            description += "[ġ��Ÿ] �ִ� ����\n";
            description += "</color>";
        }

        return description;
    }
}
