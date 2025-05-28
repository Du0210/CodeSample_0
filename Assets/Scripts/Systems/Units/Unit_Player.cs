using HDU.Managers;
using UnityEngine;
using HDU.Define;
using DG.Tweening;
using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using EAnimKey = HDU.Define.CoreDefine.EPlayerSpineAnimKey;
using System.Runtime.CompilerServices;

public class Unit_Player : Unit_Base
{
    public CoreDefine.EPlayerUnitType PlayerUnitType { get; private set; }

    public bool IsLobby { get => Managers.Scene.CurrentSceneType == CoreDefine.ESceneType.LobbyScene ?  true : false; }

    private AttackScript _attackScript;
    private MoveScrpit _moveScript;
    private SpineAnimator _spineAnimator;

    private Transform _followObject;
    private CancellationToken _moveToken;
    private CancellationToken _atkToken;
    private float _mana;


    private void OnEnable()
    {
        Managers.Event.RegistEvent(CoreDefine.EEventType.OnChangedBattleState, OnSetStateCallback);
    }

    private void OnDisable()
    {
        Managers.Event.RemoveEvent(CoreDefine.EEventType.OnChangedBattleState, OnSetStateCallback);
    }

    public void SetUnit(CoreDefine.EPlayerUnitType unitType, CoreDefine.EUnitPosition posType, int posIndex, Transform followObj, Transform parent, Vector2 worldpos )
    {
        UnitType = CoreDefine.EUnitType.Player;
        PlayerUnitType = unitType;
        transform.parent = parent;
        transform.position = worldpos;
        _posType = posType;
        _followObject = followObj;
        _cost = 1;

        if(!IsLobby)
        {
            Managers.Unit.SetUnitStatData(this, PlayerUnitType);

            Hpbar.SetData((float)(_hp / _maxHP));
            if (_attackScript == null)
                _attackScript = new AttackScript(_atkSpeed, _atkType);
            _atkToken = new CancellationToken();
        }

        _moveToken = new CancellationToken();

        OnSetStateCallback();

        Managers.Unit.SetPlayerSpine(GFX.GetSpineGFX(), unitType);

        if (_moveScript == null)
            _moveScript = new MoveScrpit(_attackScript);
        if(_spineAnimator == null)
        {
            _spineAnimator = new SpineAnimator();
            _spineAnimator.SetSkeletonAnimation(GFX.GetSpineGFX());
        }
    }

    #region FSM
    protected override void OnSetStateCallback()
    {
        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.None:
            case CoreDefine.EBattleState.Wait:
                SetState(CoreDefine.EUnitState.Idle);
                break;
            case CoreDefine.EBattleState.Battle:
                SetState(CoreDefine.EUnitState.Attack);
                break;
            default:
                SetState(CoreDefine.EUnitState.Idle);
                Debug.LogError("BattleState Wrong");
                break;
        }
    }

    protected override void OnStartIdle()
    {
        base.OnStartIdle();
        PlayAnimation(EAnimKey.IDLE, true);
    }
    protected override void OnUpdateIdle(float duration)
    {
        base.OnUpdateIdle(duration);

        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.None:
            case CoreDefine.EBattleState.Wait:
                if (_followObject.position != transform.position)
                    SetState(CoreDefine.EUnitState.Move);
                break;
            case CoreDefine.EBattleState.Battle:
                break;
        }
    }

    protected override void OnStartMove()
    {
        base.OnStartMove();

        switch (Managers.Stage.BattleState)
        {
            case CoreDefine.EBattleState.None:
            case CoreDefine.EBattleState.Wait:
                PlayAnimation(EAnimKey.FLY, true);
                _moveScript.MoveToTarget(this, _followObject, 1.5f).Forget();
                break;
            case CoreDefine.EBattleState.Battle:
                _atkTarget = Managers.Spawn.GetClosedEnemy(transform);

                if (_atkTarget == null || !_atkTarget.IsValid())
                {
                    Debug.Log($"{name}: 타겟 없음 → Idle 상태 전환");
                    SetState(CoreDefine.EUnitState.Idle);
                    return;
                }

                if (_atkTarget == null)
                    return;
                PlayAnimation(EAnimKey.FLY, true);
                _moveScript.MoveToTargetWithinRange(this, _atkTarget.Transform, _atkRange, 1.5f, _atkToken).Forget();
                break;
        }
    }

    protected override void OnStartAttack()
    {
        base.OnStartAttack();

        _atkTarget = Managers.Spawn.GetClosedEnemy(transform);
        if (_atkTarget == null)
            return;

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
        base.OnStartDead();

        Debug.Log($"사망 : {PlayerUnitType.ToString()}");
        // 연출후 삭제
        DeadDirect().Forget();
    }
    #endregion

    private async UniTask DeadDirect()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        Managers.Spawn.DestroyPlayerUnit(PlayerUnitType);
        Managers.Event.InvokeEvent(CoreDefine.EEventType.OnKillPlayer);
    }

    public override void UpgradeGrade()
    {
        if (_cost >= 5)
            return;
        _cost++;
        Managers.Unit.SetUnitCostStat(this, PlayerUnitType);
        Hpbar.DrawHpbar((float)(_hp / _maxHP));
    }

    public override void PlayAnimation<T>(T state, bool isLoop)
    {
        if (isLoop)
            _spineAnimator.Play<T>(state, isLoop);
        else
            _spineAnimator.PlayOnce<T>(state);
    }
}