using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    SpriteRenderer sr;
    
    Status stat;

    public bool isMumchit; // 멈칫                 // Skill, public

    Coroutine BuffCoroutine;

    CastableSkill Q;
    CastableSkill W;
    CastableSkill E;
    CastableSkill R;

    [SerializeField] SkillCooltimeUI Q_UI;
    [SerializeField] SkillCooltimeUI W_UI;
    [SerializeField] SkillCooltimeUI E_UI;
    [SerializeField] SkillCooltimeUI R_UI;

    public Transform skillBook;

    int skillPoint = 1;
    public int SkillPoint { get { return skillPoint; } set { skillPoint = value; } }


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        stat = GetComponent<Status>();
    }
    private void OnEnable()
    {
        isMumchit = false;

        if (Q)
        {
            Q.isCasting = false;
            Q.castable = true;
            Q_UI.Castable(Q.castable);
        }
        if (W)
        {
            W.isCasting = false;
            W.castable = true;
            W_UI.Castable(W.castable);
        }
        if (E)
        {
            E.isCasting = false;
            E.castable = true;
            E_UI.Castable(E.castable);
        }
        if (R)
        {
            R.isCasting = false;
            R.castable = true;
            R_UI.Castable(R.castable);
        }
    }

    private void Update()
    {
        if (!(isMumchit || GameManager.Instance.StoryManager.nowStoryReading)) PlayerKeyboardInput();
    }

    // Player 키보드 입력 (스킬)
    void PlayerKeyboardInput()
    {
        // Q 스킬 [기본공격] (불덩이 발사 등등..)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Q == null) return;
            if (Q.SkillLevel < 1) return;
            if (!(Q.castable && !Q.isCasting)) return;
            Q.Casting(gameObject, transform.position, new Vector3(sr.flipX ? -1 : 1, 0));
            StartCoroutine(Return_Castable(Q, Q_UI));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (W == null) return;
            if (W.SkillLevel < 1) return;
            if (!(W.castable && !W.isCasting)) return;
            W.Casting(gameObject, transform.position, new Vector3(sr.flipX ? -1 : 1, 0));
            StartCoroutine(Return_Castable(W, W_UI));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (E == null) return;
            if (E.SkillLevel < 1) return;
            if (!(E.castable && !E.isCasting)) return;
            E.Casting(gameObject, transform.position, new Vector3(sr.flipX ? -1 : 1, 0));
            StartCoroutine(Return_Castable(E, E_UI));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (R == null) return;
            if (R.SkillLevel < 1) return;
            if (!(R.castable && !R.isCasting)) return;
            R.Casting(gameObject, transform.position, new Vector3(sr.flipX ? -1 : 1, 0));
            StartCoroutine(Return_Castable(R, R_UI));
        }
    }

    public void KeySkillApply(CastableSkill skill, KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Q:
                Q = skill;
                Q_UI.skillImage.sprite = skill.info.icon;
                Q_UI.skillImage.gameObject.SetActive(true);
                Q_UI.Castable(Q.castable);
                break;
            case KeyCode.W:
                W = skill;
                W_UI.skillImage.sprite = skill.info.icon;
                W_UI.skillImage.gameObject.SetActive(true);
                W_UI.Castable(W.castable);
                break;
            case KeyCode.E:
                E = skill;
                E_UI.skillImage.sprite = skill.info.icon;
                E_UI.skillImage.gameObject.SetActive(true);
                E_UI.Castable(E.castable);
                break;
            case KeyCode.R:
                R = skill;
                R_UI.skillImage.sprite = skill.info.icon;
                R_UI.skillImage.gameObject.SetActive(true);
                R_UI.Castable(R.castable);
                break;
        }
        skill.keycode = key;
        KeyOvelapCheck(skill, key);
    }

    private void KeyOvelapCheck(CastableSkill skill, KeyCode key)
    {
        if (Q == skill && key != KeyCode.Q)
        {
            Q = null;
            Q_UI.skillImage.sprite = null;
            Q_UI.skillImage.gameObject.SetActive(false);
            Q_UI.Castable(false);
        }
        if (W == skill && key != KeyCode.W)
        {
            W = null;
            W_UI.skillImage.sprite = null;
            W_UI.skillImage.gameObject.SetActive(false);
            W_UI.Castable(false);
        }
        if (E == skill && key != KeyCode.E)
        {
            E = null;
            E_UI.skillImage.sprite = null;
            E_UI.skillImage.gameObject.SetActive(false);
            E_UI.Castable(false);
        }
        if (R == skill && key != KeyCode.R)
        {
            R = null;
            R_UI.skillImage.sprite = null;
            R_UI.skillImage.gameObject.SetActive(false);
            R_UI.Castable(false);
        }
    }


    public string[] GetKeySetting()
    {
        string[] keys = new string[4];
        
        keys[0] = Q ? Q.info.name : null;
        keys[1] = W ? W.info.name : null;
        keys[2] = E ? E.info.name : null;
        keys[3] = R ? R.info.name : null;

        return keys;
    }


    // 재사용대기시간 후 반환
    IEnumerator Return_Castable(CastableSkill skill, SkillCooltimeUI ui = null)
    {
        float waitTime = 0;
        skill.castable = false;
        if(ui) ui.Castable(false);
        waitTime = skill.info.values[skill.SkillLevel - 1].cooldownTime;

        skill.coolTimer = 0;
        while (skill.coolTimer < waitTime)
        {
            skill.coolTimer += Time.fixedDeltaTime;
            if (ui) ui.Fill(1 - (skill.coolTimer / waitTime));
            yield return new WaitForFixedUpdate();
        }
        if (ui) ui.Fill(0);
        if (ui) ui.Castable(true);
        skill.castable = true;
    }

    public void SkillLevelUp(Skill skill, int value)
    {
        for (int i = 0; i < value; i++) {
            if (skillPoint <= 0) return;
            if(skill.LevelUp())
                skillPoint -= 1;
        }
    }

    // 드래곤 변화 조건 설정 및 변화시작 설정
    public void CheckEvent()
    {
        if (GameManager.Instance.Player.ChangeMode == "default")
        {
            if (skillBook.GetComponentInChildren<FireForce>().SkillLevel == skillBook.GetComponentInChildren<FireForce>().info.values.Length
                && skillBook.GetComponentInChildren<DrawFire>().SkillLevel == skillBook.GetComponentInChildren<DrawFire>().info.values.Length)
            {
                stat.MaxHp = 64 + ((stat.Level - 1) * 10);
                GameManager.Instance.Player.ChangeDragon("fire");

                string storyName = "ChangeFireDragon";
                if (DataManager.Instance.data.isReadStory(storyName)) return;
                GameManager.Instance.StoryManager.StartScenario(storyName);
            }

            if (skillBook.GetComponentInChildren<IronPunch>().SkillLevel > 0)
            {
                stat.MaxHp = 172 + ((stat.Level - 1) * 20);
                GameManager.Instance.Player.ChangeDragon("iron");

                string storyName = "ChangeIronDragon";
                if (DataManager.Instance.data.isReadStory(storyName)) return;
                GameManager.Instance.StoryManager.StartScenario(storyName);
            }

            if (skillBook.GetComponentInChildren<FireBall>().SkillLevel > 0)
            {
                string storyName = "SkillDutorial";
                if (DataManager.Instance.data.isReadStory(storyName)) return;
                GameManager.Instance.StoryManager.StartScenario(storyName);
            }
        }


    }

    // 데미지로부터 랜덤 데미지 산출 (* 배수 / ex 0.8배 ~ 1.2배)
    int GetRandomDamageValue(int OriginDamage, float minX, float maxX)
    {
        int Damage;
        Damage = (int)(OriginDamage * UnityEngine.Random.Range(minX, maxX));
        return Damage;
    }

    // 스킬 사용시 모션 멈칫, (스킬 사용 및 움직임 불가 설정)
    IEnumerator GetMumchit(float time)
    {
        isMumchit = true;
        yield return new WaitForSeconds(time);
        isMumchit = false;
    }

    // 버프 설정 (현재 이동속도만 추가)
    IEnumerator Buff(string what, float how, float time)
    {
        switch (what)
        {
            case "Speed":
                stat.MoveSpeed = stat.BasicSpeed + how;
                break;
        }
        yield return new WaitForSeconds(time);
        stat.MoveSpeed = stat.BasicSpeed;
    }
}
