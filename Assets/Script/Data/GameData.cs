using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public string sceneName;
    public Vector3 position;
    public StatData status;
    public List<string> readstorys = new List<string>();
    public SkillData skillData;

    public void AddReadStory(string name)
    {
        if(!isReadStory(name))
            readstorys.Add(name);
    }
    public bool isReadStory(string name)
    {
        return readstorys.Contains(name);
    }
}

[System.Serializable]
public class StatData
{
    public float _maxhp;
    public float _hp;
    public float _maxExp;
    public float _exp;
    public int _level;
    public int _attackpower;
    public float _movespeed;
    public float _basicspeed;
    public float _jumppower;
    public float _basicjumppower;
    public string dragonMode;

    public void SetData(Status stat)
    {
        _maxhp = stat.MaxHp;
        _hp = stat.HP;
        _maxExp = stat.MaxExp;
        _exp = stat.Exp;
        _level = stat.Level;


        _attackpower = stat.BasicAttackPower;
        _movespeed = stat.MoveSpeed;
        _basicspeed = stat.BasicSpeed;
        _jumppower = stat.JumpPower;
        _basicjumppower = stat.BasicJumpPower;

        dragonMode = stat.GetComponent<Player>().ChangeMode;
    }
}

[System.Serializable]
public class SkillData
{
    public int skillPoint;
    public string[] skillNames;
    public int[] skillPoints;
    public string[] keySetting;
}

[System.Serializable]
public class SettingData
{
    public float bgmVolume;
    public float effectVolume;
    public int resolutionOption;
}
