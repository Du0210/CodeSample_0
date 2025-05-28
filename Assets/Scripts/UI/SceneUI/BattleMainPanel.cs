using HDU.Managers;
using TMPro;
using UnityEngine;

public class BattleMainPanel : UI_Base
{
    [SerializeField] TextMeshProUGUI TxtTime;
    [SerializeField] TextMeshProUGUI TxtCost;
    [SerializeField] TextMeshProUGUI TxtWave;

    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnDrawBattleTimer, (int i) => OnDrawTime(i));
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnDrawCost, OnDrawCost);
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnDrawWave, OnDrawWave);

    }
    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnDrawBattleTimer, (int i) => OnDrawTime(i));
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnDrawCost, OnDrawCost);
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnDrawWave, OnDrawWave);
    }

    public void OnClickStartBattle()
    {
        Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnHideCard);
        //Managers.Stage.SetBattleState(HDU.Define.CoreDefine.EBattleState.Move);
        Managers.Stage.SetBattleLoop(HDU.Define.CoreDefine.EBattleLoop.Battle);
    }

    private void OnDrawTime(int time)
    {
        int min = time / 60;
        int second = time % 60;
        TxtTime.text = $"{min:D2}:{second:D2}";
    }
    private void OnDrawCost()
    {
        TxtCost.text = $"{Managers.Goods.CardCost}"; 
    }
    private void OnDrawWave()
    {
        TxtWave.text = $"{Managers.Stage.CurWave}/{HDU.Define.CoreDefine.MAXWAVE}";
    }
    public void OnClickMenu()
    {
        Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenBattleMenuPanel, true);
    }
}
