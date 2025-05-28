namespace HDU.Managers
{
    using HDU.Define;
    using UnityEngine;

    public class EnemyManager : IManager
    {
        public void Clear()
        {
            
        }

        public void Init()
        {
            
        }

        public void SetUnitStatData(IUnit unit, CoreDefine.EEnemyUnitType type)
        {
            var tableData = Managers.Data.GetEnemyUnitTableData(type);
            unit.HP = tableData.InitHP;
            unit.MaxHP = tableData.InitHP;
            unit.Damage = tableData.InitDMG;
            unit.AtkRange = (float)tableData.AttackRange;
            unit.AtkSpeed = (float)tableData.AttackSpeed;
            unit.AtkType = tableData.AttackType;
        }

        public void SetEnemyGFX(SpriteRenderer sp, CoreDefine.EEnemyUnitType type)
        {
            sp.sprite = Managers.Data.GetEnemySprite(type);   
        }

        public (CoreDefine.EUnitPosition posType, int index) GetUnitPosition(CoreDefine.EEnemyUnitType type)
        {
            var data = Managers.Data.GetEnemyUnitTableData(type);
            int pos = -1;

            pos = Random.Range(0, 3);

            return (data.UnitPosition, pos);
        }
    }
}