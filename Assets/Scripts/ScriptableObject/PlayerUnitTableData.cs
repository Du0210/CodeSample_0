using UnityEngine;
using HDU.Define;

[CreateAssetMenu(fileName = "PlayerUnitTableData", menuName = "Scriptable Objects/PlayerUnitTableData")]
public class PlayerUnitTableData : ScriptableObject
{
    public PlayerUnitTableEntry[] playerUnitTableEntries;

    public PlayerUnitTableEntry GetPlayerEntry(CoreDefine.EPlayerUnitType type)
    {
        return playerUnitTableEntries[(int)type];
    }
}

[System.Serializable]
public class PlayerUnitTableEntry : IUnitTable
{
    CoreDefine.EUnitAttackType IUnitTable.AttackType { get => AttackType; set => AttackType = value; }
    CoreDefine.EUnitPosition IUnitTable.UnitPosition { get => UnitPosition; set => UnitPosition = value; }
    int IUnitTable.lv { get => Lv; set => Lv = value; }
    double IUnitTable.InitDMG { get => InitDMG; set => InitDMG = value; }
    double IUnitTable.InitHP { get => InitHP; set => InitHP = value; }
    double IUnitTable.GrowthDMG { get => GrowthDMG; set => GrowthDMG = value; }
    double IUnitTable.GrowthHP { get => GrowthHP; set => GrowthHP = value; }
    double IUnitTable.AttackRange { get => AttackRange; set => AttackRange = value; }
    double IUnitTable.AttackSpeed { get => AttackSpeed; set => AttackSpeed = value; }

    public CoreDefine.EPlayerUnitType UnitType;
    public CoreDefine.EUnitAttackType AttackType;
    public CoreDefine.EUnitPosition UnitPosition;
    public CoreDefine.ESpineSkinType SpineSkinType;
    public CoreDefine.ESpineWpSlotType SpineWpSlotType;   

    public int Lv;
    public double InitDMG;
    public double InitHP;
    public double GrowthDMG;
    public double GrowthHP;
    public double AttackRange;
    public double AttackSpeed;
    public string Name;
}