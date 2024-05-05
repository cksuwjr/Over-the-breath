using System;
using System.Collections;
using UnityEngine;


public class FireBall : CastableSkill, ISpawnableSkill
{
    protected float speed;
    protected float duration;
    protected bool stackable;
    public int prefab_Id { get { return 0; } set { } }

    protected FireBall()
    {
        speed = 15;
        duration = 0.3f;
        stackable = false;
    }

    public override bool AcquisitionCondition()
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        if (skillBook.GetComponentInChildren<IronPunch>().SkillLevel > 0) return false;
        return true;
    }

    public override void Casting(GameObject attacker, Vector3 position, Vector3 direction)
    {
        GameManager.Instance.StartCoroutine(Cast(attacker, position, direction));
    }

    public virtual IEnumerator Cast(GameObject attacker, Vector3 position, Vector3 direction)
    {
        var skillBook = GameManager.Instance.Player.skill.skillBook;
        var strongFireBall = skillBook.GetComponentInChildren<StrongFireBall>();
        var speedFireBall = skillBook.GetComponentInChildren<SpeedFireBall>();
        var backStep = skillBook.GetComponentInChildren<BackStep>();
        isCasting = true;
        

        if (strongFireBall.SkillLevel < 1 && speedFireBall.SkillLevel < 1)
        {
            if (attacker.GetComponent<Move>().isGround)
                attacker.GetComponent<PlayerSkill>().isMumchit = true;

            attacker.GetComponent<Animator>().SetTrigger("BasicAttack");

            GameObject fire = PoolManager.Instance.Get(prefab_Id);
            fire.transform.position = position + new Vector3(direction.x, 0);
            fire.GetComponent<SpriteRenderer>().flipX = direction.x == -1 ? true : false;

            float damage = info.values[SkillLevel - 1].basicValue + attacker.GetComponent<Status>().AttackPower * info.values[SkillLevel - 1].ratio / 100f;

            float plusSpeed = 0;
            float plusDamage = 0;
            var fireForce = skillBook.GetComponentInChildren<FireForce>();
            var drawFire = skillBook.GetComponentInChildren<DrawFire>();
            if (fireForce.SkillLevel > 0) plusDamage += (damage * fireForce.info.values[fireForce.SkillLevel - 1].ratio / 100f);
            if (drawFire.SkillLevel > 0) plusSpeed += (speed * drawFire.info.values[drawFire.SkillLevel - 1].ratio / 100f);

            fire.GetComponent<Fire>().Init(attacker, direction, damage + plusDamage, 0, speed + plusSpeed, duration, stackable);

            yield return new WaitForSeconds(0.16f);
            attacker.GetComponent<PlayerSkill>().isMumchit = false;
            isCasting = false;
        }
        else if(strongFireBall.SkillLevel > 0)
        {
            float gage = 0;
            float maxTime = 1.25f;

            var playerUI = GameManager.Instance.Player.GetComponentInChildren<PlayerUI>();

            while (Input.GetKey(keycode))
            {
                if (gage < 1)
                    gage += (Time.fixedDeltaTime / maxTime);
                else
                    gage = 1;
                playerUI.SetGage(gage);

                if(backStep.SkillLevel > 0 && gage < 1)
                    backStep.coolTimer += (2 * (Time.fixedDeltaTime / maxTime));

                yield return new WaitForFixedUpdate();
            }
            attacker.GetComponent<Animator>().SetTrigger("BasicAttack");
            attacker.GetComponent<PlayerSkill>().isMumchit = true;

            float attackDamage = (attacker.GetComponent<Status>().AttackPower * 0.9f) * strongFireBall.info.values[strongFireBall.SkillLevel - 1].ratio / 100f;
            if (gage == 1)
                attackDamage *= 2;
            else
                attackDamage *= gage;

            attackDamage += strongFireBall.info.values[strongFireBall.SkillLevel - 1].basicValue + (attacker.GetComponent<Status>().AttackPower * 0.1f);

            float plusSpeed = 0;
            float plusDamage = 0;
            var fireForce = skillBook.GetComponentInChildren<FireForce>();
            var drawFire = skillBook.GetComponentInChildren<DrawFire>();
            if (fireForce.SkillLevel > 0) plusDamage += (attackDamage * fireForce.info.values[fireForce.SkillLevel - 1].ratio / 100f);

            GameObject fire = PoolManager.Instance.Get(prefab_Id);
            int dir = attacker.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            float spe = (speed * 0.75f) < speed * (gage * 1.2f) ? speed * (gage * 1.2f) : (speed * 0.75f);
            if (drawFire.SkillLevel > 0) plusSpeed += (spe * drawFire.info.values[drawFire.SkillLevel - 1].ratio / 100f);
            fire.transform.position = attacker.transform.position + new Vector3(dir, 0);
            fire.GetComponent<Fire>().Init(attacker, new Vector3(dir, 0), attackDamage + plusDamage, 0, spe + plusSpeed, duration, stackable);
            fire.GetComponent<SpriteRenderer>().flipX = attacker.GetComponent<SpriteRenderer>().flipX ? true : false;
            playerUI.SetGage(0);

            yield return new WaitForSeconds(0.16f);

            attacker.GetComponent<PlayerSkill>().isMumchit = false;
            isCasting = false;

        }
        else if(speedFireBall.SkillLevel > 0)
        {
            float gage = 1;

            var playerUI = GameManager.Instance.Player.GetComponentInChildren<PlayerUI>();


            float plusSpeed = 0;
            float plusDamage = 0;
            var fireForce = skillBook.GetComponentInChildren<FireForce>();
            var drawFire = skillBook.GetComponentInChildren<DrawFire>();
            if (drawFire.SkillLevel > 0) plusSpeed += (speed * drawFire.info.values[drawFire.SkillLevel - 1].ratio / 100f);

            int maxCount = speedFireBall.info.values[speedFireBall.SkillLevel - 1].count;
            int count = 0;
            float cooldown = speedFireBall.info.values[speedFireBall.SkillLevel - 1].cooldownTime;

            attacker.GetComponent<PlayerSkill>().isMumchit = true;

            while (Input.GetKey(keycode) && count < maxCount)
            {
                gage = 1 - (count / (float)maxCount);
                playerUI.SetGage(gage);

                int countDamage = count < 70 ? count : 70;

                

                float attackDamage = (attacker.GetComponent<Status>().AttackPower * 1) * ((speedFireBall.info.values[speedFireBall.SkillLevel - 1].ratio + countDamage) / 100f);

                if (fireForce.SkillLevel > 0) plusDamage = (attackDamage * fireForce.info.values[fireForce.SkillLevel - 1].ratio / 100f);
                Debug.Log(attackDamage + "/" + ((speedFireBall.info.values[speedFireBall.SkillLevel - 1].ratio + countDamage) / 100f) + "/" + plusDamage);

                GameObject fire = PoolManager.Instance.Get(prefab_Id);
                int dir = attacker.GetComponent<SpriteRenderer>().flipX ? -1 : 1;

                fire.transform.position = attacker.transform.position + new Vector3(dir, 0);
                fire.GetComponent<Fire>().Init(attacker, new Vector3(dir, 0), attackDamage + plusDamage, 0, speed + plusSpeed, duration, stackable);
                fire.GetComponent<SpriteRenderer>().flipX = attacker.GetComponent<SpriteRenderer>().flipX ? true : false;
                attacker.GetComponent<Animator>().SetTrigger("BasicAttack");
                count++;
                yield return new WaitForSeconds(cooldown);
            }
            playerUI.SetGage(0);
            Debug.Log("발사수 : " + count);
            yield return new WaitForSeconds(0.16f);

            attacker.GetComponent<PlayerSkill>().isMumchit = false;
            isCasting = false;
        }
    }

    public override string GetDescription()
    {
        string description = "";
        description += "<color=\"yellow\">";
        description += info.skillName;
        description += " [시전가능스킬]" + "\n";
        description += "</color>";

        Values values;

        var skillBook = GameManager.Instance.Player.skill.skillBook;
        var strongFireBall = skillBook.GetComponentInChildren<StrongFireBall>();
        var speedFireBall = skillBook.GetComponentInChildren<SpeedFireBall>();

        if (strongFireBall.SkillLevel > 0)
        {
            description += $"이 스킬이 강력하게 차징할 수 있도록 강화되었습니다!!\n";
        }
        else if(speedFireBall.SkillLevel > 0)
        {
            description += $"이 스킬이 빠르게 연사할 수 있도록 강화되었습니다!!\n";
        }
        else 
        {
            values = SkillLevel > 0 ? info.values[SkillLevel - 1] : info.values[0];

            description += $"전방에 불덩이를 발사하여 적중시 공격력의 {values.ratio}%의 피해를 입힙니다.\n";
            description += $"쿨타임 : {values.cooldownTime}초\n";
        }

        if (SkillLevel < 1)
        {
            description += "\n\n<color=\"red\">습득 조건\n";
            description += "[강철 주먹] 미습득\n";
            description += "</color>";
        }

        return description;
    }
}