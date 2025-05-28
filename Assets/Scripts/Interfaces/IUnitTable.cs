using HDU.Define;
using UnityEngine;

public interface IUnitTable
{
    public CoreDefine.EUnitAttackType AttackType { get; set; }
    public CoreDefine.EUnitPosition UnitPosition { get; set; }

    public int lv { get; set; }
    public double InitDMG { get; set; }
    public double InitHP { get; set; }
    public double GrowthDMG { get; set; }
    public double GrowthHP { get; set; }
    public double AttackRange { get; set; }
    public double AttackSpeed { get; set; }
}
