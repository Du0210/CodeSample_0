using UnityEngine;

public interface IUnit
{
    public HDU.Define.CoreDefine.EUnitType UnitType { get; set; }
    public int Lv { get; set; }
    public int Cost { get; set; }
    public double MaxHP { get; set; }
    public double HP { get; set; }
    public double Damage { get; set; }
    public float AtkRange { get; set; }
    public float AtkSpeed { get; set; }
    public float MoveSpeed { get; set; }
    public HDU.Define.CoreDefine.EUnitAttackType AtkType { get; set; }
    public GameObject @GameObject { get; }
    public Transform @Transform {  get; }
    public bool IsAlive { get; }
    public bool IsValid();
    public void TakeDamage(double dmg);
    public void UpgradeGrade();
    public HDU.Define.CoreDefine.EUnitState State { get; }
    public void SetState(HDU.Define.CoreDefine.EUnitState state);
    public void PlayAnimation<T>(T state, bool isLoop) where T : System.Enum;
}
