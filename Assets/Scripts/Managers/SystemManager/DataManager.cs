namespace HDU.Managers
{
    using HDU.Define;
    using UnityEngine;
    using UnityEngine.U2D;
    //using static UnityEditor.U2D.ScriptablePacker;

    public class DataManager : IManager
    {
        public PlayerUnitTableData @PlayerUnitTableData { get; private set; }
        public EnemyUnitTableData @EnemyUnitTableData { get; private set; }
        public StageTableData @StageTableData { get; private set; }

        public SpriteAtlas EnemySpriteAtlas { get; private set; }

        public void Clear()
        {

        }

        public void Init()
        {
            PlayerUnitTableData = Managers.Resource.Load<PlayerUnitTableData>("Table/PlayerUnitTableData");
            EnemyUnitTableData = Managers.Resource.Load<EnemyUnitTableData>("Table/EnemyUnitTableData");
            StageTableData = Managers.Resource.Load<StageTableData>("Table/StageTableData");
            EnemySpriteAtlas = Managers.Resource.Load<SpriteAtlas>("Atlas/EnemySpriteAtlas");
        }

        public PlayerUnitTableEntry GetPlayerUnitTableData(HDU.Define.CoreDefine.EPlayerUnitType type)
        {
            return PlayerUnitTableData.GetPlayerEntry(type);
        }

        public EnemyUnitTableEntry GetEnemyUnitTableData(HDU.Define.CoreDefine.EEnemyUnitType type)
        {
            return EnemyUnitTableData.GetEnemyEntry(type);
        }

        public StageTableEntry GetStageTableData(int index)
        {
            return StageTableData.GetStageEntry(index);
        }

        public Sprite GetEnemySprite(CoreDefine.EEnemyUnitType type)
        {
            int index = (int)type;
            Sprite sp = EnemySpriteAtlas.GetSprite($"Nature_{index + 1}");
            if (sp != null)
                return sp;
            else
            {
                Debug.LogError("Cant Find Sprite!! \nname : " + type.ToString());
                return null;
                //return EnemySpriteAtlas.GetSprite(nameof(KeySprite.None));
            }
        }
    }
}