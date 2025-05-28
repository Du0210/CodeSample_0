using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public List<int> StageIndexList;
    public List<List<int>> StageScoreList;

    public StageData()
    {
        Initialize();
    }

    public void Initialize()
    {
        StageIndexList = new List<int>();

        int stageMaxCount = (int)HDU.Define.CoreDefine.EStageType.MaxCount;

        StageIndexList.Capacity = stageMaxCount;
        for (int i = 0; i < stageMaxCount; i++)
            StageIndexList.Add(1);

        StageScoreList = new List<List<int>>();
        for (int i = 0; i < stageMaxCount; i++)
        {
            StageScoreList.Add(new List<int>());
            for (int j = 0; j < HDU.Define.CoreDefine.MAXSTAGECOUNT; j++)
                StageScoreList[i].Add(0);
        }
    }
}