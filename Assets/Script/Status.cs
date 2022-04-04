using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Status : MonoBehaviour
{
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected int _attackpower;
    [SerializeField]
    protected float _maxhp;
    [SerializeField]
    protected float _movespeed;
    [SerializeField]
    protected float _jumppower;

    public float MaxHp { get { return _maxhp; } set { _maxhp = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public int AttackPower { get { return _attackpower; } set { _attackpower = value; } }
    public float MoveSpeed { get { return _movespeed; } set { _movespeed = value; } }
    public float JumpPower { get { return _jumppower; } set { _jumppower = value; } }

    private void Start()
    {
        if(_hp == 0)
            _hp = 500f;
        if (_attackpower == 0)
            _attackpower = 50;
        if (_maxhp == 0)
            _maxhp = 500f;
        if(_movespeed == 0)
            _movespeed = 5f;
        if(_jumppower == 0)
            _jumppower = 5f;

    }
    public void StatInit(float hp, int power, float Maxhp, float Movespeed, float Jumppower)
    {
        _maxhp = Maxhp;
        _hp = hp;
        _attackpower = power;
        _movespeed = Movespeed;
        _jumppower = Jumppower;
    }
    public void StatInit(Status stat)
    {
        _maxhp = stat.MaxHp;
        _hp = stat.HP;
        _attackpower = stat.AttackPower;
        _movespeed = stat.MoveSpeed;
        _jumppower = stat.JumpPower;
    }
}
