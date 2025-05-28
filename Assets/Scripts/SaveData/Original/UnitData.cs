using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public List<UnitEntry> UnitList;

    public UnitData()
    {
        Initialize();
    }

    public void Initialize()
    {
        UnitList = new List<UnitEntry>();
        UnitList.Capacity = (int)HDU.Define.CoreDefine.EPlayerUnitType.MaxCount;

        for (int i = 0; i < (int)HDU.Define.CoreDefine.EPlayerUnitType.MaxCount; i++)
        {
            UnitEntry unitEntry = new UnitEntry();
            unitEntry.UnitType = (HDU.Define.CoreDefine.EPlayerUnitType)i;
            unitEntry.UnitIndex = i;
            unitEntry.UnitLevel = 1;
            unitEntry.UnitExp = 0;
            unitEntry.UnitStar = 1;
            unitEntry.IsRetained = false;
            UnitList.Add(unitEntry);
        }

        AllRetainedDebug();
    }

    public void AllRetainedDebug()
    {
        for (int i = 0; i < UnitList.Count; i++)
            UnitList[i].IsRetained = true;
    }
}

[System.Serializable]
public class UnitEntry
{
    public HDU.Define.CoreDefine.EPlayerUnitType UnitType;
    public int UnitIndex;
    public int UnitLevel;
    public int UnitExp;
    public int UnitStar;
    public bool IsRetained;
}