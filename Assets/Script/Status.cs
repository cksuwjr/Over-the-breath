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

    public float HP { get { return _hp; } set { _hp = value; } }
    public int AttackPower { get { return _attackpower; } set { _attackpower = value; } }
    public float MaxHp { get { return _maxhp; } set { _maxhp = value; } }
    public float MoveSpeed { get { return _movespeed; } set { _movespeed = value; } }
    public float JumpPower { get { return _jumppower; } set { _jumppower = value; } }

    private void Start()
    {
        _hp = 500f;
        _attackpower = 50;
        _maxhp = 500f;
        _movespeed = 5f;
        _jumppower = 5f;

    }
}
