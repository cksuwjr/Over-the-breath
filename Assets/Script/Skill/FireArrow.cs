using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;
public class FireArrow : FireBall, ISpawnableSkill
{
    protected FireArrow() : base()
    {
        stackable = true;
    }

    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if(skillBook.GetComponentInChildren<FireBall>().SkillLevel < 1) return false;
        if (skillBook.GetComponentInChildren<DrawFire>().SkillLevel < skillBook.GetComponentInChildren<DrawFire>().info.values.Length) return false;

        return true;
    }

    public override IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        isCasting = true;

        int fireCount = info.values[SkillLevel - 1].count;
        Vector3 castingPos = position + new Vector3(0, 1f - 0.2f * fireCount);
        while (fireCount > 0)
        {
            GameObject fire = PoolManager.Instance.Get(prefab_Id);
            fire.transform.position = castingPos + new Vector3(direction.x, -1f + 0.2f * fireCount);
            fire.GetComponent<SpriteRenderer>().flipX = direction.x == -1 ? true : false;

            float damage = info.values[SkillLevel - 1].basicValue + attacker.GetComponent<Status>().AttackPower * info.values[SkillLevel - 1].ratio / 100f;

            float plusSpeed = 0;
            float plusDamage = 0;
            var skillBook = GameManager.Instance.Player.skill.skillBook;
            var fireForce = skillBook.GetComponentInChildren<FireForce>();
            var drawFire = skillBook.GetComponentInChildren<DrawFire>();
            if (fireForce.SkillLevel > 0) plusDamage += (damage * fireForce.info.values[fireForce.SkillLevel - 1].ratio / 100f);
            if (drawFire.SkillLevel > 0) plusSpeed += (speed * drawFire.info.values[drawFire.SkillLevel - 1].ratio / 100f);

            fire.GetComponent<Fire>().Init(attacker, direction, damage + plusDamage, 0, speed + plusSpeed, duration, stackable);

            fireCount--;
            yield return new WaitForSeconds(0.06f);
        }
        isCasting = false;

    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [�������ɽ�ų]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"���濡 �ҵ��� {values.count}���� �߻��Ͽ� ���߽� ���� {values.basicValue} + ���ݷ��� {values.ratio}%�� ���ظ� �����ϴ�.\n";
        description += $"�߻�� : {values.count}\n";
        description += $"��Ÿ�� : {values.cooldownTime}��\n";

        if(SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[���ǵ� ����] �ִ� ����\n";
            description += "</color>";
        }

        return description;
    }
}

