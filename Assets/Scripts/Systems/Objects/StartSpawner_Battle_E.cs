using HDU.Managers;
using System.Collections.Generic;
using UnityEngine;

public class StartSpawner_Battle_E : StartSpawner
{
    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnSpawnEnemy, SpawnEnemy);
    }   

    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnSpawnEnemy, SpawnEnemy);
    }


    public override void SetSpawner()
    {
        _spawnType = HDU.Define.CoreDefine.ESpawnType.Enemy;
    }

    public void SpawnEnemy()
    {
        var tableData = Managers.Data.GetStageTableData(Managers.Stage.StageIndex);
        List<HDU.Define.CoreDefine.EEnemyUnitType> unitList = null;

        switch (Managers.Stage.CurWave)
        {
            case 1:
                unitList = tableData.Wave_One;
                break;
            case 2:
                unitList = tableData.Wave_Two;
                break;
            case 3:
                unitList = tableData.Wave_Three;
                break;
            case 4:
                unitList = tableData.Wave_Four;
                break;
        }

        foreach (var unitKey in unitList)
        {
            Unit_Enemy unit = Managers.Spawn.SpawnEnemyUnit(unitKey) as Unit_Enemy;
            (HDU.Define.CoreDefine.EUnitPosition posType, int posIndex) posData = Managers.Enemy.GetUnitPosition(unitKey);
            Transform spawner = GetSpawnerTR(posData.posType, posData.posIndex);
            unit.SetUnit(unitKey, posData.posType, posData.posIndex, UnitParent, spawner.transform.position);
        }
    }
}
