namespace HDU.Managers
{
    using System.Collections.Generic;
    using UnityEngine;

    public class SpawnManager : IManager
    {
        public Dictionary<HDU.Define.CoreDefine.EPlayerUnitType, IUnit> PlayerUnitDict { get; private set; }
        public List<IUnit> EnemyUnitList { get; private set; }
        public IStartSpawner LobbySpawner { get; private set; }
        public IStartSpawner BattleSpawner_P { get; private set; }
        public IStartSpawner BattleSpawner_E { get; private set; }
        public void Clear()
        {
            PlayerUnitDict.Clear();
            EnemyUnitList.Clear();
            LobbySpawner = null;
            BattleSpawner_P = null;
            BattleSpawner_E = null;
        }

        public void Init()
        {
            PlayerUnitDict = new Dictionary<Define.CoreDefine.EPlayerUnitType, IUnit>();
            EnemyUnitList = new List<IUnit>();
        }

        public IUnit SpawnPlayerUnit(HDU.Define.CoreDefine.EPlayerUnitType type)
        {
            IUnit unit = Managers.Resource.Instantiate("Units/Unit_Player").GetComponent<IUnit>();
            PlayerUnitDict.Add(type, unit);

            return unit;
        }
        public void DestroyPlayerUnit(Define.CoreDefine.EPlayerUnitType type)
        {
            Managers.Resource.Destroy(PlayerUnitDict[type].GameObject);
            PlayerUnitDict.Remove(type);
        }

        public IUnit SpawnEnemyUnit(Define.CoreDefine.EEnemyUnitType type)
        {
            IUnit unit = Managers.Resource.Instantiate("Units/Unit_Enemy").GetComponent<IUnit>();
            EnemyUnitList.Add(unit);

            return unit;
        }
        public void DestroyEnemyUnit(IUnit unit)
        {
            Managers.Resource.Destroy(unit.GameObject);
            EnemyUnitList.Remove(unit);
        }

        public IUnit GetClosedEnemy(Transform from)
        {
            IUnit closest = null;
            float minDist = float.MaxValue;

            foreach (var enemy in EnemyUnitList)
            {
                if (!enemy.IsAlive)
                    continue;
                float dist = Vector3.Distance(from.position, enemy.Transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }
            return closest;
        }

        public IUnit GetClosedPlayerUnit(Transform from)
        {
            IUnit closest = null;
            float minDist = float.MaxValue;

            foreach (var unit in PlayerUnitDict)
            {
                if (!unit.Value.IsAlive)
                    continue;

                float dist = Vector3.Distance(from.position, unit.Value.Transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = unit.Value;
                }
            }
            return closest;
        }

        public IUnit ValidateOrFindNewTarget(IUnit current, IUnit self)
        {
            if (current == null || current.GameObject == null || !current.IsAlive)
            {
                return Managers.Spawn.GetClosedEnemy(self.Transform);
            }

            return current;
        }

        public void SetSpawner_Lobby(IStartSpawner spawner) => LobbySpawner = spawner;
        public void SetSpawner_Player(IStartSpawner spawner) => BattleSpawner_P = spawner;
        public void SetSpawner_Enemy(IStartSpawner spawner) => BattleSpawner_E = spawner;
    }
}