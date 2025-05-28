using UnityEngine;

public class StagePanel : UI_Base
{
    [SerializeField] CanvasGroup @CanvasGroup;

    protected override void Init()
    {
        base.Init();
        gameObject.GetComponent<RectTransform>().position = Vector3.zero;
    }

    private void OnEnable()
    {
        HDU.Managers.Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    private void OnDisable()
    {
        HDU.Managers.Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    public void OnOpenLobbyTypeCallback(HDU.Define.CoreDefine.ELobbySceneType type)
    {
        if (type == HDU.Define.CoreDefine.ELobbySceneType.Stage)
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }
        else
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }

    public void OnClickStage()
    {
        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.Deck);
    }
}
