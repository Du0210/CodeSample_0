using System;
using UnityEngine;
using DG.Tweening;
using HDU.Managers;
using HDU.Define;
using Cysharp.Threading.Tasks;

public class Projectile : MonoBehaviour, IProjectile
{
    private Action _onHitCallback;

    public Transform Transform => base.transform;

    public GameObject GameObject => base.gameObject;

    public CoreDefine.EProjectileType ProjectileType { get => _projType; set => _projType = value; }

    private CoreDefine.EProjectileType _projType;

    public void SetProjectile(Action onHitCallback, Vector3 target, Vector3 from)
    {
        transform.position = from;
        _onHitCallback = onHitCallback;
        Logic(target);
    }

    public void Logic(Vector3 target)
    {
        transform.DOMove(target, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                OnHit().Forget();
            });
    }

    private async UniTask OnHit(float delay = 0)
    {
        _onHitCallback?.Invoke();
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        Managers.Resource.Destroy(gameObject);
    }
}