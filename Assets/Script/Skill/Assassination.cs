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
                            // �� �ʱ�ȭ
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
        description += " [�������ɽ�ų]" + "\n";
        description += "</color>";

        var skillBook = GameManager.Instance.Player.skill.skillBook;
        var critical = skillBook.GetComponentInChildren<Critical>();

        Values values;
        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"�ٶ󺸴� �������� �����Ͽ� ó�� ����ģ ������ ���ݷ��� {values.ratio}%�� �ش��ϴ� ū ���ظ� �����ϴ�.\n";
        description += $"�� ��ų�� ��� óġ�� ������ ��ų�� ���� ���ð��� �ʱ�ȭ�˴ϴ�.\n";
        if(critical.SkillLevel > 0)
            description += $"�� ��ų���� ġ��Ÿ Ȯ���� 4�� ����Ǿ� {(int)(critical.info.values[critical.SkillLevel-1].ratio * 4)}% Ȯ���� ġ��Ÿ�� �߻��˴ϴ�.\n";
        description += $"��Ÿ�� : {values.cooldownTime}��\n";


        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[������ �ѹ�] �ִ� ����\n";
            description += "[���� ����] �ִ� ����\n";
            description += "</color>";
        }

        return description;
    }

}
