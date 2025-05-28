using HDU.Define;
using HDU.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Base : MonoBehaviour, IUnit
{
    [SerializeField] protected GFX GFX;
    [SerializeField] protected HPBar Hpbar;

    #region Property
    public int Lv { get => _lv; set => _lv = value; }
    public double MaxHP { get => _maxHP; set => _maxHP = value; }
    public double HP { get => _hp; set => _hp = value; }
    public double Damage { get => _damage; set => _damage = value; }
    public float AtkRange { get => _atkRange; set => _atkRange = value; }
    public float AtkSpeed { get => _atkSpeed; set => _atkSpeed = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public CoreDefine.EUnitAttackType AtkType { get => _atkType; set => _atkType = value; }
    public int Cost { get => _cost; set => _cost = value; }
    public GameObject GameObject { get => base.gameObject; }
    public Transform Transform { get => base.transform; }
    public bool IsAlive { get => _hp > 0; }
    public bool IsValid() { return this != null && gameObject != null; }
    public CoreDefine.EUnitType UnitType { get => _unitType; set => _unitType = value; }
    public CoreDefine.EUnitState State => _state;

    #endregion

    #region FSM
    protected HDU.Define.CoreDefine.EBaseFSM _baseFSM = HDU.Define.CoreDefine.EBaseFSM.StateStart;
    protected HDU.Define.CoreDefine.EUnitState _state = HDU.Define.CoreDefine.EUnitState.Idle;

    private Dictionary<CoreDefine.EUnitState, Action> _startActionDict;
    private Dictionary<CoreDefine.EUnitState, Action<float>> _updateActionDict;
    private Dictionary<CoreDefine.EUnitState, Action> _endActionDict;

    protected float _stateElapsedTime = 0;
    #endregion

    protected CoreDefine.EUnitType _unitType;
    protected HDU.Define.CoreDefine.EUnitPosition _posType;
    protected int _lv = 1;
    protected int _cost = 0;
    protected double _maxHP = 0;
    protected double _hp = 0;
    protected double _damage = 0;
    protected float _atkRange = 0;
    protected float _atkSpeed = 0;
    protected float _moveSpeed = 0;
    protected HDU.Define.CoreDefine.EUnitAttackType _atkType = HDU.Define.CoreDefine.EUnitAttackType.None;
    protected IUnit _atkTarget;

    protected virtual void Awake()
    {
        InitFSM();
    }
    protected virtual void FixedUpdate()
    {
        FSMUpdate();
    }

    public virtual void SetState(HDU.Define.CoreDefine.EUnitState newState)
    {
        if (_state == CoreDefine.EUnitState.Dead)
            return;

        if (_state == newState)
            return;

        _state = newState;
        _baseFSM = HDU.Define.CoreDefine.EBaseFSM.StateEnd;
    }

    #region FSM Function
    private void InitFSM()
    {
        _startActionDict = new Dictionary<CoreDefine.EUnitState, Action>()
        {
            { CoreDefine.EUnitState.Idle, OnStartIdle },
            { CoreDefine.EUnitState.Move, OnStartMove },
            { CoreDefine.EUnitState.Attack, OnStartAttack },
            { CoreDefine.EUnitState.Skill, OnStartSkill },
            { CoreDefine.EUnitState.Dead, OnStartDead },
        };

        _updateActionDict = new Dictionary<CoreDefine.EUnitState, Action<float>>()
        {
            { CoreDefine.EUnitState.Idle, OnUpdateIdle },
            { CoreDefine.EUnitState.Move, OnUpdateMove },
            { CoreDefine.EUnitState.Attack, OnUpdateAttack },
            { CoreDefine.EUnitState.Skill, OnUpdateSkill },
            { CoreDefine.EUnitState.Dead, OnUpdateDead },
        };

        _endActionDict = new Dictionary<CoreDefine.EUnitState, Action>()
        {
            { CoreDefine.EUnitState.Idle, OnEndIdle },
            { CoreDefine.EUnitState.Move, OnEndMove },
            { CoreDefine.EUnitState.Attack, OnEndAttack },
            { CoreDefine.EUnitState.Skill, OnEndSkill },
            { CoreDefine.EUnitState.Dead, OnEndDead },
        };
    }

    private void FSMUpdate()
    {
        _stateElapsedTime = Time.fixedDeltaTime;
        switch (_baseFSM)
        {
            case HDU.Define.CoreDefine.EBaseFSM.StateStart:
                _startActionDict[_state]?.Invoke();
                _stateElapsedTime = 0f;
                _baseFSM = HDU.Define.CoreDefine.EBaseFSM.StateUpdate;
                break;
            case HDU.Define.CoreDefine.EBaseFSM.StateUpdate:
                _updateActionDict[_state]?.Invoke(_stateElapsedTime);
                break;
            case HDU.Define.CoreDefine.EBaseFSM.StateEnd:
                _endActionDict[_state]?.Invoke();
                _baseFSM = HDU.Define.CoreDefine.EBaseFSM.StateStart;
                break;
        }
    }

    protected virtual void OnStartIdle() { }
    protected virtual void OnUpdateIdle(float duration) { }
    protected virtual void OnEndIdle() { }

    protected virtual void OnStartMove() { }
    protected virtual void OnUpdateMove(float duration) { }
    protected virtual void OnEndMove() { }

    protected virtual void OnStartAttack() { }
    protected virtual void OnUpdateAttack(float duration) { }
    protected virtual void OnEndAttack() { }

    protected virtual void OnStartSkill() { }
    protected virtual void OnUpdateSkill(float duration) { }
    protected virtual void OnEndSkill() { }

    protected virtual void OnStartDead() { }
    protected virtual void OnUpdateDead(float duration) { }
    protected virtual void OnEndDead() { }
    #endregion

    public virtual void UpgradeGrade() { }
    public virtual void TakeDamage(double dmg)
    {
        if (!IsAlive)
            return;

        _hp -= dmg;
        Hpbar.DrawHpbar((float)(_hp / _maxHP));

        if (!IsAlive)
        {
            _hp = 0;
            SetState(CoreDefine.EUnitState.Dead);
        }
    }
    public virtual void PlayAnimation<T>(T state, bool loop) where T : Enum { }
    protected virtual void OnSetStateCallback() { }
}
