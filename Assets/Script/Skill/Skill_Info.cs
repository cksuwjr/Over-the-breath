using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public abstract class CastableSkill : Skill
{
    public abstract void Casting(GameObject attacker, Vector3 position, Vector3 direction);
    public bool isCasting = false;
    public bool castable = true;
    public float coolTimer;
    public KeyCode keycode;

    private void OnEnable()
    {
        isCasting = false;
        castable = true;
    }
}

[System.Serializable]
public abstract class Skill : MonoBehaviour
{
    public Skill_Info info;
    [SerializeField] private int skillLevel = 0;
    public int SkillLevel { get { return info.values.Length <= skillLevel ? info.values.Length : skillLevel; } set { skillLevel = value; } }
    public bool LevelUp() 
    {
        if (AcquisitionCondition() && skillLevel < info.values.Length)
        {
            skillLevel += 1;
            return true;
        }
        return false;
    }
    public virtual string GetDescription() { return info.skillDescription; }
    
    public virtual bool AcquisitionCondition() { return true; }
}



[CreateAssetMenu(fileName = "Skill  Data", menuName = "Skill Scriptable/Skill Data", order = int.MaxValue), System.Serializable]
public class Skill_Info : ScriptableObject
{
    public Sprite icon;
    public string skillName;
    [Multiline(3)]
    public string skillDescription;
    public Values[] values;
}

[Serializable]
public class Values
{
    public float basicValue;
    public float ratio;
    public int count;
    public float cooldownTime;
}

public interface ISpawnableSkill
{
    int prefab_Id { get; set; }
}

public interface IPassiveSkill
{
    void Lasting();
}


