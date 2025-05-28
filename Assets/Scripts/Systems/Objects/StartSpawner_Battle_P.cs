using HDU.Managers;
using UnityEngine;

public class StartSpawner_Battle_P : StartSpawner
{
    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnSetFirstUnitSetting, SetFirst);
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnSelectCard, 
            (HDU.Define.CoreDefine.EPlayerUnitType type) => OnSelectCard(type));
    }
    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnSetFirstUnitSetting, SetFirst);
    }
    public override void SetSpawner()
    {
        _spawnType = HDU.Define.CoreDefine.ESpawnType.Player;
    }

    private void SetFirst()
    {
        var stack = Managers.Card.CardDataStack;
        while (stack.Count > 0)
        {
            var data = Managers.Card.CardDataStack.Pop();
            var unitDict = Managers.Spawn.PlayerUnitDict;
            switch (data.CardType)
            {
                case HDU.Define.CoreDefine.ECardType.Cost:
                    Managers.Goods.AddOrRemoveCost(data.Cost, true);
                    break;
                case HDU.Define.CoreDefine.ECardType.Unit:
                    if(unitDict.ContainsKey(data.UnitType))
                    {
                        unitDict[data.UnitType].UpgradeGrade();
                    }
                    else
                    {
                        Unit_Player unit = Managers.Spawn.SpawnPlayerUnit(data.UnitType) as Unit_Player;
                        (HDU.Define.CoreDefine.EUnitPosition posType, int posIndex) posData = Managers.Unit.GetUnitPosition(data.UnitType);
                        Transform spawner = GetSpawnerTR(posData.posType, posData.posIndex);
                        unit.SetUnit(data.UnitType, posData.posType, posData.posIndex, spawner, UnitParent, spawner.transform.position);
                    }
                    break;
            }
        }
    }

    private void OnSelectCard(HDU.Define.CoreDefine.EPlayerUnitType type)
    {
        var unitDict = Managers.Spawn.PlayerUnitDict;
        //업글
        if (Managers.Spawn.PlayerUnitDict.ContainsKey(type))
        {
            unitDict[type].UpgradeGrade();
        }
        // 생성
        else
        {
            Unit_Player unit = Managers.Spawn.SpawnPlayerUnit(type) as Unit_Player;
            (HDU.Define.CoreDefine.EUnitPosition posType, int posIndex) posData = Managers.Unit.GetUnitPosition(type);
            Transform spawner = GetSpawnerTR(posData.posType, posData.posIndex);
            unit.SetUnit(type, posData.posType, posData.posIndex, spawner, UnitParent, spawner.transform.position);
        }
    }
}
