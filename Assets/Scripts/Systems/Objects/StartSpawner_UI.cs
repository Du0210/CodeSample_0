using HDU.Managers;
using UnityEngine;
using System.Collections.Generic;

public class StartSpawner_UI : StartSpawner
{
    private void Awake()
    {
        SetSpawner();
    }
    private void Start()
    {
        SetUnits();
    }

    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnSelectUnit, SetUnits);
    }
    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnSelectUnit, SetUnits);
    }

    public override void SetSpawner()
    {
        _spawnType = HDU.Define.CoreDefine.ESpawnType.UI;

        foreach (var spawnPoint in SpawnRenderers)
            spawnPoint.gameObject.SetActive(true);
    }

    private void SetUnits()
    {
        List<HDU.Define.CoreDefine.EPlayerUnitType> retainedKeyList = Managers.Unit.RetainedUnitList;
        List<HDU.Define.CoreDefine.EPlayerUnitType> selectKeyList = Managers.Unit.SelectedUnits;
        var unitDict = Managers.Spawn.PlayerUnitDict;

        for (int i = 0; i < retainedKeyList.Count; i++)
        {
            HDU.Define.CoreDefine.EPlayerUnitType key = retainedKeyList[i];
            PlayerUnitTableEntry tableData = Managers.Data.GetPlayerUnitTableData(key);
            // 생성
            if (!unitDict.ContainsKey(key) && selectKeyList.Contains(key))
            {
                Unit_Player unit = Managers.Spawn.SpawnPlayerUnit(key) as Unit_Player;

                (HDU.Define.CoreDefine.EUnitPosition posType, int posIndex) posData = Managers.Unit.GetUnitPosition(key);
                Transform spawner = GetSpawnerTR(posData.posType, posData.posIndex);
                
                unit.SetUnit(key, posData.posType, posData.posIndex, spawner, UnitParent, spawner.transform.position);
            }
            // 삭제
            else if (unitDict.ContainsKey(key) && !selectKeyList.Contains(key))
            {
                Managers.Spawn.DestroyPlayerUnit(key);
            }
        }
    }
}
