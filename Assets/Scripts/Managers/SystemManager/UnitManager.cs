namespace HDU.Managers
{
    using System.Collections.Generic;
    using UnityEngine;
    using Spine.Unity;
    using EPlayerUnitType = HDU.Define.CoreDefine.EPlayerUnitType;
    using EUnitPosition = HDU.Define.CoreDefine.EUnitPosition;

    public class UnitManager : IManager
    {
        #region Property
        public UnitData @UnitData { get => Managers.Save.UserData.Unit; private set => Managers.Save.UserData.Unit = value; }
        public Dictionary<EUnitPosition, List<EPlayerUnitType>> SelectUnitPosDict { get; private set; }
        /// <summary> 유닛 보유 리스트 </summary>
        public List<EPlayerUnitType> RetainedUnitList { get; private set; }
        /// <summary> 유닛 보유 Dict </summary>        
        public Dictionary<EPlayerUnitType, UnitEntry> RetainedUnitDict { get; private set; }
        public List<EPlayerUnitType> SelectedUnits { get; private set; }
        #endregion

        /// <summary> 유닛을 추가 보유할때 갱신용 </summary>
        private bool _isDirty = true;

        
        public void Clear()
        {
            
        }

        public void Init()
        {
            RetainedUnitList = new List<EPlayerUnitType>();
            RetainedUnitDict = new Dictionary<EPlayerUnitType, UnitEntry>();

            SelectedUnits = new List<EPlayerUnitType>();
            SelectedUnits.Capacity = (int)HDU.Define.CoreDefine.MAXSELECTUNITCOUNT;

            SelectUnitPosDict = new Dictionary<EUnitPosition, List<EPlayerUnitType>> 
            {
                { EUnitPosition.Front, new List<EPlayerUnitType> { EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount } },
                { EUnitPosition.Middle,   new List<EPlayerUnitType> { EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount } },
                { EUnitPosition.Back,  new List<EPlayerUnitType> { EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount, EPlayerUnitType.MaxCount } },
            };
        }

        public void SetUnitStatData(IUnit unit, EPlayerUnitType type)
        {
            var tableData = Managers.Data.GetPlayerUnitTableData(type);
            unit.HP = tableData.InitHP;
            unit.Damage = tableData.InitDMG;
            unit.AtkRange = (float)tableData.AttackRange;
            unit.AtkSpeed = (float)tableData.AttackSpeed;
            unit.AtkType = tableData.AttackType;
            SetUnitCostStat(unit, type);
        }

        public Dictionary<EPlayerUnitType, UnitEntry> GetRetainedUnit()
        {
            if(_isDirty)
            {
                RetainedUnitDict.Clear();
                RetainedUnitList.Clear();
                foreach (var item in UnitData.UnitList)
                {
                    RetainedUnitDict.Add(item.UnitType, item);
                    RetainedUnitList.Add(item.UnitType);
                }

                _isDirty = false;
            }

            return RetainedUnitDict;
        }

        public void SetPlayerSpine(SkeletonGraphic spine, EPlayerUnitType type)
        {
            HDU.Define.CoreDefine.ESpineSkinType skinKey = Managers.Data.GetPlayerUnitTableData(type).SpineSkinType;
            HDU.Define.CoreDefine.ESpineWpSlotType wpKey = Managers.Data.GetPlayerUnitTableData(type).SpineWpSlotType;
            
            spine.Skeleton.SetSkin(skinKey.ToString());
            spine.Skeleton.SetSlotsToSetupPose();

            spine.Skeleton.SetAttachment("Weapon", wpKey.ToString());
        }

        public void SetPlayerSpine(SkeletonAnimation spine, HDU.Define.CoreDefine.EPlayerUnitType type)
        {
            HDU.Define.CoreDefine.ESpineSkinType skinKey = Managers.Data.GetPlayerUnitTableData(type).SpineSkinType;
            HDU.Define.CoreDefine.ESpineWpSlotType wpKey = Managers.Data.GetPlayerUnitTableData(type).SpineWpSlotType;

            spine.Skeleton.SetSkin(skinKey.ToString());
            spine.Skeleton.SetSlotsToSetupPose();

            spine.Skeleton.SetAttachment("Weapon", wpKey.ToString());
        }

        public void SetUnitCostStat(IUnit unit, EPlayerUnitType type)
        {
            var tableData = Managers.Data.GetPlayerUnitTableData(type);
            unit.MaxHP = tableData.InitHP + tableData.GrowthHP * unit.Cost;
            unit.HP = unit.MaxHP;
            unit.Damage = tableData.InitDMG + tableData.GrowthDMG * unit.Cost;
        }

        #region Unit Management
        public void SelectUnit(EPlayerUnitType type)
        {
            // remove
            if (SelectedUnits.Count > 0 && SelectedUnits.Contains(type))
            {
                SelectedUnits.Remove(type);
                RemoveUnitPosition(type);
            }
            // add
            else
            {
                // 6개 이상 선택됐을때
                if (SelectedUnits.Count >= Define.CoreDefine.MAXSELECTUNITCOUNT)
                    return;
                // 선택된 유닛의 포지션타입이 3개이상일때

                if (SetUnitPosition(type))
                    return;

                SelectedUnits.Add(type);

            }
            Managers.Event.InvokeEvent(Define.CoreDefine.EEventType.OnSelectUnit);
        }

        public bool IsContainsUnit(EPlayerUnitType type)
        {
            return SelectedUnits.Contains(type);
        }

        public (EUnitPosition posType, int index) GetUnitPosition(EPlayerUnitType type)
        {
            var data = Managers.Data.GetPlayerUnitTableData(type);
            int pos = -1;

            for (int i = 0; i < SelectUnitPosDict[data.UnitPosition].Count; i++)
            {
                if (SelectUnitPosDict[data.UnitPosition][i] == type)
                {
                    pos = i;
                    break;
                }
            }

            if (pos == -1)
                Debug.LogError("Can't Find Unit Position");

            return (data.UnitPosition, pos);
        }

        private bool SetUnitPosition(EPlayerUnitType type)
        {
            var data = Managers.Data.GetPlayerUnitTableData(type);

            bool isFull = true;
            int index = 0;
            for (int i = 0; i < SelectUnitPosDict[data.UnitPosition].Count; i++)
            {
                if (SelectUnitPosDict[data.UnitPosition][i] == EPlayerUnitType.MaxCount)
                {
                    index = i;
                    isFull = false;
                    SelectUnitPosDict[data.UnitPosition][index] = type;
                    break;
                }
            }

            return isFull;
        }

        private void RemoveUnitPosition(EPlayerUnitType type)
        {
            var posType = Managers.Data.GetPlayerUnitTableData(type).UnitPosition;
            bool isFind = false;
            for (int i = 0; i < SelectUnitPosDict[posType].Count; i++)
            {
                if (SelectUnitPosDict[posType][i] == type)
                {
                    SelectUnitPosDict[posType][i] = EPlayerUnitType.MaxCount;
                    isFind = true;
                    break;
                }
            }

            if (!isFind)
                Debug.LogError("Remove Failed");
        }
        #endregion
    }
}