namespace HDU.Managers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectileManager : IManager
    {
        private List<IProjectile> _projectiles;
        public void Clear()
        {
            _projectiles.Clear();
        }

        public void Init()
        {
            _projectiles = new List<IProjectile>();
        }

        public IProjectile MakeProjectile(HDU.Define.CoreDefine.EProjectileType type, Action action, Vector3 target, Vector3 from)
        {
            IProjectile projectile = Managers.Resource.Instantiate($"Projectiles/{type}").GetComponent<IProjectile>();
            projectile.SetProjectile(action, target, from);
            return projectile;
        }
    }
}