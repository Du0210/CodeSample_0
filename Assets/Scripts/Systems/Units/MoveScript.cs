using Cysharp.Threading.Tasks;
using DG.Tweening;
using HDU.Define;
using System.Threading;
using System;
using UnityEngine;
using HDU.Managers;

public class MoveScrpit
{
    private AttackScript _attackScript;

    public MoveScrpit(AttackScript atk)
    {
        Initialize(atk);
    }

    public void Initialize(AttackScript _atk)
    {
        _attackScript = _atk;
    }

    public async UniTask MoveToTarget(IUnit self, Transform target, float duration)
    {
        if (target.position == self.Transform.position)
            return;

        Vector3 dir = (target.position - self.Transform.position).normalized;
        float distance = Vector3.Distance(self.Transform.position, target.position);

        Vector3 destination = self.Transform.position + dir * distance;

        self.Transform.DOMove(destination, duration).SetEase(Ease.OutQuad).SetSpeedBased(false).SetAutoKill(false);

        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        self.SetState(CoreDefine.EUnitState.Idle);
    }

    public async UniTask MoveToTargetWithinRange(IUnit self, Transform target, float range, float duration, CancellationToken token)
    {
        if (target == null) return;

        // �ʱ� ��Ÿ� üũ
        if (_attackScript.IsTargetInRange(self, target))
        {
            self.SetState(CoreDefine.EUnitState.Attack);
            return;
        }

        // ���� �� �ӵ� ���
        Vector3 direction = (target.position - self.Transform.position).normalized;
        float distance = Vector3.Distance(self.Transform.position, target.position);
        float moveDistance = Mathf.Max(distance - self.AtkRange, 0f);
        float moveSpeed = moveDistance / duration;

        // �� ������ �̵�
        while (!_attackScript.IsTargetInRange(self, target))
        {
            if (token.IsCancellationRequested || target == null)
                return;

            // �� ������ �̵��� ���
            Vector3 moveDelta = direction * moveSpeed * Managers.Time.GetFixedDeltaTime();
            self.Transform.position += moveDelta;

            // ���� ������ ���
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        // ��Ÿ� ���� �� ���� ���� ����
        self.SetState(CoreDefine.EUnitState.Attack);
    }
}
