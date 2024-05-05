using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;

public class FireRain : FireArrow, ISpawnableSkill
{
    FireRain() : base()
    {
        speed = 12.5f;
        duration = 1.5f;
        stackable = false;
    }

    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<FireBall>().SkillLevel < 1) return false;
        if (skillBook.GetComponentInChildren<SpeedFireBall>().SkillLevel < 1) return false;
        if (skillBook.GetComponentInChildren<FireArrow>().SkillLevel < 6) return false;

        return true;
    }

    public override IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        isCasting = true;

        int fireCount = info.values[SkillLevel - 1].count;
        Vector3 castingPos = position;
        int fireDirection = (int)direction.x;
        int z = fireDirection == -1 ? -120 : -60;
        while (fireCount > 0)
        {
            GameObject fire = PoolManager.Instance.Get(prefab_Id);
            fire.transform.position = castingPos + new Vector3(fireDirection + fireDirection * UnityEngine.Random.Range(-4.5f, 4.5f), 5f);
            fire.GetComponent<SpriteRenderer>().flipX = false;
            float damage = info.values[SkillLevel - 1].basicValue + attacker.GetComponent<Status>().AttackPower * info.values[SkillLevel - 1].ratio / 100f;

            fire.GetComponent<Fire>().Init(attacker, new Vector3(Mathf.Cos(z * Mathf.Deg2Rad), Mathf.Sin(z * Mathf.Deg2Rad)), damage, z, speed, duration, stackable);

            fireCount--;
            if (fireCount % 2 == 1)
                yield return new WaitForSeconds(0.13f);
            else
                yield return null;
        }
        isCasting = false;

    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [시전가능스킬]" + "\n";
        description += "</color>";

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"하늘로부터 불덩이를 {values.count}개 소환하여 적중시 각각 {values.basicValue} + 공격력의 {values.ratio}%의 피해를 입힙니다.\n";
        description += $"발사수 : {values.count}\n";
        description += $"쿨타임 : {values.cooldownTime}초\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[더욱 더 빠르게] 1레벨 이상\n";
            description += "[불화살 세례] 5레벨 이상\n";
            description += "</color>";
        }

        return description;
    }
}

