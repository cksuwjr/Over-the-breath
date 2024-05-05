using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPunch : CastableSkill
{
    protected bool stackable;
    int basicAttackSequence = 0;

    protected IronPunch()
    {
        stackable = false;
    }

    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<FireBall>().SkillLevel > 0) return false;
        return true;
    }

    public override void Casting(GameObject attacker, Vector3 position, Vector3 direction)
    {
        attacker.GetComponent<PlayerSkill>().StartCoroutine(Cast(attacker, position, direction));
    }
    public virtual IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        isCasting = true;

        var anim = attacker.GetComponent<Animator>();
        var stat = attacker.GetComponent<Status>();

        anim.SetInteger("Iron_BasicSequence", basicAttackSequence);
        anim.SetTrigger("BasicAttack");

        float time = 0.1f;
        attacker.GetComponent<PlayerSkill>().isMumchit = true;
        if (attacker.GetComponent<Move>().Movable)
        {
            Collider2D collider = Physics2D.OverlapBox(attacker.transform.position + new Vector3(direction.x * 0.5f, -0.81f), new Vector2(0.46f, 0.06f), 0, attacker.GetComponent<Player>().groundLayer);
            while (time > 0 && collider)
            {
                collider = Physics2D.OverlapBox(attacker.transform.position + new Vector3(direction.x * 0.5f, -0.81f), new Vector2(0.46f, 0.06f), 0, attacker.GetComponent<Player>().groundLayer);
                attacker.GetComponent<Rigidbody2D>().velocity = new Vector2(0, attacker.GetComponent<Rigidbody2D>().velocity.y);
                attacker.transform.Translate(direction * 3f * Time.fixedDeltaTime);
                time -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attacker.transform.position + new Vector3(direction.x * 0.7f, 0.03f), new Vector2(1.4f, 0.34f), 0);

        var skillBook = GameManager.Instance.Player.skill.skillBook;
        var strongAttack = skillBook.GetComponentInChildren<StrongAttack>();
        var bloodHeart = skillBook.GetComponentInChildren<BloodHeart>();
        var critical = skillBook.GetComponentInChildren<Critical>();

        float damage = 0;
        if (basicAttackSequence == 0)
            damage = info.values[SkillLevel - 1].basicValue + stat.AttackPower * info.values[SkillLevel - 1].ratio / 100f;
        else if (basicAttackSequence == 1)
        {
            damage = info.values[SkillLevel - 1].basicValue + stat.AttackPower * info.values[SkillLevel - 1].ratio / 100f;
            damage *= 1.5f;
        }
        else if (basicAttackSequence == 2)
            damage = strongAttack.info.values[strongAttack.SkillLevel - 1].basicValue + stat.AttackPower * strongAttack.info.values[strongAttack.SkillLevel - 1].ratio / 100f;


        bool isCritical = false;
        if (critical.SkillLevel > 0)
            if (critical.isCritical())
            {
                damage *= 2.5f;
                isCritical = true;
            }

        foreach (Collider2D collider in colliders)
        {
            var monster = collider.GetComponentInChildren<Monster>();
            if (!monster) monster = collider.GetComponentInParent<Monster>();
            if (monster)
            {
                if (!monster.die)
                {
                    monster.GetDamaged(damage, attacker, stackable, isCritical);
                    if (basicAttackSequence == 2) monster.GetAirborne(new Vector2(direction.x * 5f, 10f));
                    if (bloodHeart.SkillLevel > 0) attacker.GetComponent<Player>().GetHeal((int)(damage * (bloodHeart.info.values[bloodHeart.SkillLevel - 1].ratio / 100f)));
                }
            }
        }
        if (basicAttackSequence == 0 || basicAttackSequence == 1)
        {
            basicAttackSequence++;
            if (strongAttack.SkillLevel < 1 && basicAttackSequence > 1)
                basicAttackSequence = 0;
            if(basicAttackSequence == 0)
                yield return new WaitForSeconds(0.28f * (1 / anim.speed));
            else if(basicAttackSequence == 1)
                yield return new WaitForSeconds(0.305f * (1 / anim.speed));
        }
        else if (basicAttackSequence == 2)
        {
            basicAttackSequence = 0;
            yield return new WaitForSeconds(0.39f * (1 / anim.speed));
        }
        SoundManager.Instance.Sound("Magic Spell_Simple Swoosh_6", 3f, 0.43f);
        attacker.GetComponent<PlayerSkill>().isMumchit = false;
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

        var skillBook = GameManager.Instance.Player.skill.skillBook;

        values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

        description += $"�������� �����ϸ� ��ι����� ������ ���ݷ��� {values.ratio}%�� ���ظ� �����ϴ�.\n";
        description += $"�� �ι�° ������ ��ι����� ������ ���ݷ��� {values.ratio * 1.5f}%�� ���ظ� �����ϴ�.\n";
        description += $"��Ÿ�� : {values.cooldownTime}��\n";

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">���� ����\n";
            description += "[�ҵ��� �߻�] �̽���\n";
            description += "</color>";
        }

        return description;
    }
}
