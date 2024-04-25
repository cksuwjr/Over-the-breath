using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassination : CastableSkill
{
    protected Assassination() { }

    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;

        if (skillBook.GetComponentInChildren<StrongAttack>().SkillLevel < skillBook.GetComponentInChildren<StrongAttack>().info.values.Length) return false;
        if (skillBook.GetComponentInChildren<BloodHeart>().SkillLevel < skillBook.GetComponentInChildren<BloodHeart>().info.values.Length) return false;

        return true;
    }

    public override void Casting(GameObject attacker, Vector3 position, Vector3 direction)
    {
        attacker.GetComponent<PlayerSkill>().StartCoroutine(Cast(attacker, position, direction));
    }

    public virtual IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        var bloodHeart = skillBook.GetComponentInChildren<BloodHeart>();
        var critical = skillBook.GetComponentInChildren<Critical>();

        isCasting = true;
        SoundManager.Instance.Sound("Magic Spell_Simple Swoosh_6", 1f, 1f);
        var anim = attacker.GetComponent<Animator>();


        float time = 0.15f;

        attacker.GetComponent<PlayerSkill>().isMumchit = true;
        anim.SetTrigger("Skill1");
        bool attack = false;
        while (time > 0)
        {
            //attacker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, attacker.GetComponent<Rigidbody2D>().velocity.y);
            if (!attacker.GetComponent<Move>().isWall)
                attacker.GetComponent<Rigidbody2D>().MovePosition(attacker.transform.position + direction * 25 * Time.fixedDeltaTime);
            time -= Time.fixedDeltaTime;
            if (!attack)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(attacker.transform.position + new Vector3(-direction.x * 0.7f, 0.03f), new Vector2(1.4f, 0.34f), 0);
                float damage = info.values[SkillLevel - 1].basicValue + attacker.GetComponent<Status>().AttackPower * info.values[SkillLevel - 1].ratio / 100f;
                bool isCritical = false;
                if (critical.SkillLevel > 0)
                    if (critical.isCritical(4)) { damage *= 2.5f; isCritical = true; }
                foreach (Collider2D collider in colliders)
                {
                    var monster = collider.GetComponentInChildren<Monster>();
                    if (!monster) monster = collider.GetComponentInParent<Monster>();
                    if (monster)
                    {
                        if (!monster.die)
                        {
                            if (bloodHeart.SkillLevel > 0) attacker.GetComponent<Player>().GetHeal((int)(damage * (bloodHeart.info.values[bloodHeart.SkillLevel - 1].ratio / 100f)));
                            monster.GetDamaged(damage, attacker, true, isCritical);
                            attack = true;
                            // 쿨 초기화
                            if(monster.die) { coolTimer = info.values[SkillLevel - 1].cooldownTime; }
                            break;
                        }
                    }
                }
            }

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
        var critical = skillBook.GetComponentInChildren<Critical>();

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"바라보는 방향으로 돌진하여 처음 마주친 적에게 공격력의 {values.ratio}%에 해당하는 큰 피해를 입힙니다.\n";
        description += $"이 스킬로 대상 처치에 성공시 스킬의 재사용 대기시간이 초기화됩니다.\n";
        if(critical.SkillLevel > 0)
            description += $"이 스킬에는 치명타 확률이 4배 적용되어 {(int)(critical.info.values[critical.SkillLevel-1].ratio * 4)}% 확률로 치명타가 발생됩니다.\n";
        description += $"쿨타임 : {values.cooldownTime}초\n";


        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[강력한 한방] 최대 레벨\n";
            description += "[피의 심장] 최대 레벨\n";
            description += "</color>";
        }

        return description;
    }

}
