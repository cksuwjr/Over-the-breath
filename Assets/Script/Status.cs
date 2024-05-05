using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status : MonoBehaviour
{
    [SerializeField]
    protected float _maxhp;
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected float _exp;
    [SerializeField]
    protected float _maxExp;
    [SerializeField]
    protected int _level;

    [SerializeField]
    protected int _attackpower;
    [SerializeField]
    protected int _basicattackpower;
    [SerializeField]
    protected float _movespeed;
    [SerializeField]
    protected float _basicspeed;
    [SerializeField]
    protected float _jumppower;
    [SerializeField]
    protected float _basicjumppower;

    public float MaxHp { get { return _maxhp; } set { _maxhp = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public float MaxExp { get { return _maxExp; } set { _maxExp = value; } }
    public float Exp { get { return _exp; } set { _exp = value; } }
    public int Level { get { return _level; } set { _level = value; } }

    public int AttackPower { get { return _attackpower; } set { _attackpower = value; } }
    public int BasicAttackPower { get { return _basicattackpower; } set { _basicattackpower = value; } }

    public float MoveSpeed { get { return _movespeed; } set { _movespeed = value; } }
    public float BasicSpeed { get { return _basicspeed; } set { _basicspeed = value; } }
    public float JumpPower { get { return _jumppower; } set { _jumppower = value; } }
    public float BasicJumpPower { get { return _basicjumppower; } set { _basicjumppower = value; } }

    private void Awake()
    {
        if (_maxhp == 0)
            _maxhp = 50f;
        if(_hp == 0)
            _hp = 50f;
        if (_maxExp == 0)
            _maxExp = 15f;
        if (_exp == 0)
            _exp = 0f;
        if (_level == 0)
            _level = 1;
        if (_attackpower == 0)
            _attackpower = 12;
        if (_basicattackpower == 0)
            _basicattackpower = 12;
        if(_basicspeed == 0)
            _basicspeed = 3.5f;
        if (_movespeed == 0)
            _movespeed = 3.5f;
        if (_jumppower == 0)
            _jumppower = 5f;
        if (_basicjumppower == 0)
            _basicjumppower = 5f;

    }
}
