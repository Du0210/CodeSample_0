using System.Threading;
using UnityEngine;
using HDU.Define;
using HDU.Managers;
using Cysharp.Threading.Tasks;
using System;

public class Unit_Enemy : Unit_Base
{
    public CoreDefine.EEnemyUnitType EnemyUnitType { get; private set; }

    private AttackScript _attackScript;
    private MoveScrpit _moveScript;

    //private Transform _followObject;
    private CancellationToken _moveToken;
    private CancellationToken _atkToken;


    private void OnEnable()
    {
        Managers.Event.RegistEvent(CoreDefine.EEventType.OnChangedBattleState, OnSetStateCallback);
    }

    private void OnDisable()
    {
        Managers.Event.RemoveEvent(CoreDefine.EEventType.OnChangedBattleState, OnSetStateCallback);
    }

    public void SetUnit(CoreDefine.EEnemyUnitType unitType, CoreDefine.EUnitPosition posType, int posIndex, Transform parent, Vector2 worldpos)
    {
        UnitType = CoreDefine.EUnitType.Enemy;
        EnemyUnitType = unitType;
        transform.parent = parent;
        transform.position = worldpos;
        _posType = posType;
        //_followObject = followObj;

        Managers.Enemy.SetUnitStatData(this, EnemyUnitType);
        Hpbar.SetData((float)(_hp / _maxHP));

        _moveToken = new CancellationToken();
        _atkToken = new CancellationToken();

        OnSetStateCallback();

        Managers.Enemy.SetEnemyGFX(GFX.GetSRGFX(), unitType);

        if (_attackScript == null)
            _attackScript = new AttackScript(_atkSpeed, _atkType);
        if (_moveScript == null)
            _moveScript = new MoveScrpit(_attackScript);
    }

    protected override void OnSetStateCallback()
    {
        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.Battle:
                SetState(CoreDefine.EUnitState.Attack);
                break;
            default:
                SetState(CoreDefine.EUnitState.Idle);
                break;
        }
    }

    protected override void OnUpdateIdle(float duration)
    {
        base.OnUpdateIdle(duration);

        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.Battle:
                break;
        }

    }

    protected override void OnStartMove()
    {
        base.OnStartMove();

        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.Battle:
                _atkTarget = Managers.Spawn.GetClosedPlayerUnit(transform);

                if (_atkTarget == null || !_atkTarget.IsValid())
                {
                    Debug.Log($"{name}: 타겟 없음 → Idle 상태 전환");
                    SetState(CoreDefine.EUnitState.Idle);
                    return;
                }

                if (_atkTarget == null)
                    return;
                _moveScript.MoveToTargetWithinRange(this, _atkTarget.Transform, _atkRange, 1.5f, _atkToken).Forget();
                break;
        }
    }

    protected override void OnStartAttack()
    {
        base.OnStartAttack();
        if (_atkTarget == null)
            _atkTarget = Managers.Spawn.GetClosedPlayerUnit(transform);
        _attackScript.TryAttack(this, _atkTarget, _atkToken).Forget();
    }

    protected override void OnUpdateAttack(float duration)
    {
        base.OnUpdateAttack(duration);
        if (_atkTarget == null || !_atkTarget.IsAlive || !_attackScript.IsTargetInRange(this, _atkTarget.Transform))
        {
            SetState(CoreDefine.EUnitState.Move);
            return;
        }
        _attackScript.TryAttack(this, _atkTarget, _atkToken).Forget();
    }

    protected override void OnStartDead()
    {
        Debug.Log($"사망 : {EnemyUnitType.ToString()}");

        // 연출후 삭제
        DeadDirect().Forget();
    }

    private async UniTask DeadDirect()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        Managers.Spawn.DestroyEnemyUnit(this);
        Managers.Event.InvokeEvent(CoreDefine.EEventType.OnKillEnemy);
    }
}