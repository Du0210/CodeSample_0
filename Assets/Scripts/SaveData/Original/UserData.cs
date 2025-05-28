using UnityEngine;

[System.Serializable]
public class UserData : ISaveData
{
    public SettingData Setting;
    public StageData Stage;
    public UnitData Unit;
    public GoodsData Goods;
    public string Version;

    public void Initialize()
    {
        Setting = new SettingData();
        Stage = new StageData();
        Unit = new UnitData();
        Goods = new GoodsData();
        Version = "0.0.1";
    }
}
