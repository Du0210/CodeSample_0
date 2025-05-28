using HDU.Managers;
using TMPro;
using UnityEditor.Build;
using UnityEngine;

public class ResultPanel : UI_Base
{
    [SerializeField] GameObject Panel;
    [SerializeField] TextMeshProUGUI TxtMsg;

    private bool _isMenu = false;

    private void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenBattleMenuPanel, (bool b) => SetActiveMenu(b));
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenResultPanel, (bool b) => SetActiveResult(b));
    }
    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenBattleMenuPanel, (bool b) => SetActiveMenu(b));
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenResultPanel, (bool b) => SetActiveResult(b));
    }

    protected override void Init()
    {
        base.Init();
        Panel.SetActive(false);
    }

    public void SetActiveMenu(bool isActive)
    {
        Panel.SetActive(isActive);
        TxtMsg.text = "일시정지";
        _isMenu = isActive == true ? true : false;
    }

    public void SetActiveResult(bool isClear)
    {
        Panel.SetActive(isClear);
        if (isClear)
            TxtMsg.text = "승리!";
        else
            TxtMsg.text = "패배!";
        _isMenu = false;
    }

    public void OnClickExit()
    {
        Managers.Stage.ExitStage();
    }
    public void OnClickRetry()
    {
        Managers.Stage.Retry();
    }
    public void CloseMenu()
    {
        Panel.SetActive(false);
    }
}
