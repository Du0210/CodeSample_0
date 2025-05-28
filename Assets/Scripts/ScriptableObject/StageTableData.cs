using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTableData", menuName = "Scriptable Objects/StageTableData")]
public class StageTableData : ScriptableObject
{
    public StageTableEntry[] stageTableEntries;

    public StageTableEntry GetStageEntry(int stageIndex)
    {
        foreach (var entry in stageTableEntries)
        {
            if (entry.StageIndex == stageIndex)
                return entry;
        }
        return null;
    }
}

[Serializable]
public class StageTableEntry
{
    public int StageIndex;
    public int StageLevel;
    public float TimeLimit;
    public List<HDU.Define.CoreDefine.EEnemyUnitType> Wave_One;
    public List<HDU.Define.CoreDefine.EEnemyUnitType> Wave_Two;
    public List<HDU.Define.CoreDefine.EEnemyUnitType> Wave_Three;
    public List<HDU.Define.CoreDefine.EEnemyUnitType> Wave_Four;
}
