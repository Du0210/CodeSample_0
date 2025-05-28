using Cysharp.Threading.Tasks;
using HDU.Managers;
using System;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;

public class AttackScript
{
    private float _lastAttackTime;
    private float _attackDelay;
    private HDU.Define.CoreDefine.EUnitAttackType _atkType;
    public bool CanAttack => Time.time - _lastAttackTime >= _attackDelay;

    public AttackScript(float atkDelay, HDU.Define.CoreDefine.EUnitAttackType atkType)
    {
        Initialize(atkDelay, atkType);
    }

    public void Initialize(float atkDelay, HDU.Define.CoreDefine.EUnitAttackType atkType)
    {
        _attackDelay = atkDelay;
        _atkType = atkType;
    }

    public bool IsTargetInRange(IUnit self, Transform target)
    {
        if (target == null || self == null) return false;

        Vector3 dir = (target.position - self.Transform.position).normalized;
        float distance = Vector3.Distance(self.Transform.position, target.position);

        return distance <= self.AtkRange ? true : false;
    }

    public async UniTask TryAttack(IUnit self, IUnit target, CancellationToken token)
    {
        if (!CanAttack || !target.IsAlive || !IsTargetInRange(self, target.Transform)) return;
        _lastAttackTime = Time.time;    

        if (_atkType == HDU.Define.CoreDefine.EUnitAttackType.Melee)
            await Attack_Melee(self, target, token);
        else
            await Attack_Range(self, target, token);
    }

    private async UniTask Attack_Melee(IUnit attacker, IUnit target, CancellationToken token)
    {
        if (token.IsCancellationRequested) return;

        // 간단하게 직접 데미지
        target.TakeDamage(attacker.Damage);
        attacker.PlayAnimation(HDU.Define.CoreDefine.EPlayerSpineAnimKey.ATTACK6, false);
        await UniTask.Delay(100, cancellationToken: token); // 타격 연출 시간
    }

    private async UniTask Attack_Range(IUnit attacker, IUnit target, CancellationToken token)
    {
        if (token.IsCancellationRequested) return;

        attacker.PlayAnimation(HDU.Define.CoreDefine.EPlayerSpineAnimKey.ATTACK2, false);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token); // 타격 연출 시간

        if (target == null || attacker == null)
            return;

        var projectile = Managers.Projectile.MakeProjectile(HDU.Define.CoreDefine.EProjectileType.Bullet_0,
            () => target.TakeDamage(attacker.Damage), target.Transform.position, attacker.Transform.position);
    }
}
