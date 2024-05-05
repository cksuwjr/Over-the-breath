using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStep : CastableSkill
{
    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;

        if (skillBook.GetComponentInChildren<FireForce>().SkillLevel < skillBook.GetComponentInChildren<FireForce>().info.values.Length) return false;
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
        SoundManager.Instance.Sound("Magic Spell_Simple Swoosh_6", 1f, 1f);
        var anim = attacker.GetComponent<Animator>();


        float time = 0.15f;

        attacker.GetComponent<PlayerSkill>().isMumchit = true;
        while (time > 0)
        {
            //attacker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, attacker.GetComponent<Rigidbody2D>().velocity.y);
            if (!attacker.GetComponent<Move>().isWall)
                attacker.GetComponent<Rigidbody2D>().MovePosition(attacker.transform.position + direction * -15 * Time.fixedDeltaTime);
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        attacker.GetComponent<PlayerSkill>().isMumchit = false;
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
        description += "뒤로 일정거리 밀려납니다. 스킬 시전 중에 사용 가능합니다.\n";
        description += "[더욱 더 강하게] 스킬의 게이지가 차오르는 동안 이 스킬의 쿨타임이 추가로 감소합니다.\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[화염 위력 강화] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }

}
