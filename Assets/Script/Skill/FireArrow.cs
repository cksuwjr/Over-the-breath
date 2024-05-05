using System.Collections;
using UnityEngine;
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

        if(attacker.GetComponent<Move>().isGround)
            attacker.GetComponent<PlayerSkill>().isMumchit = true;

        while (fireCount > 0)
        {
            GameObject fire = PoolManager.Instance.Get(prefab_Id);
            fire.transform.position = position + new Vector3(direction.x, Random.Range(0f, -0.2f));
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
        attacker.GetComponent<PlayerSkill>().isMumchit = false;

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

        description += $"전방에 불덩이 {values.count}개를 발사하여 적중시 각각 {values.basicValue} + 공격력의 {values.ratio}%의 피해를 입힙니다.\n";
        description += $"발사수 : {values.count}\n";
        description += $"쿨타임 : {values.cooldownTime}초\n";

        if(SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[스피드 증가] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }
}

