using System;
using UnityEngine;

public interface IProjectile
{
    public Transform @Transform { get; }
    public GameObject @GameObject { get; }
    public HDU.Define.CoreDefine.EProjectileType ProjectileType { get; set; }
    public void SetProjectile(Action onHitCallback, Vector3 target, Vector3 from);
    public void Logic(Vector3 target);
}
