using UnityEngine;
//using EnhancedUI.EnhancedScroller;
using HDU.Managers;

public class DeckPanel : UI_Base//, IEnhancedScrollerDelegate
{
    [SerializeField] CanvasGroup @CanvasGroup;
    [SerializeField] UnitSlotHolder SlotPrefab;
    //[SerializeField] EnhancedScroller Scrollrect;

    protected override void Init()
    {
        base.Init();
        gameObject.GetComponent<RectTransform>().position = Vector3.zero;
        //Scrollrect.Delegate = this;
        //Scrollrect.ReloadData();
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
        if (type == HDU.Define.CoreDefine.ELobbySceneType.Deck)
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

    public void OnClickStart()
    {
        // 전투시작
        if (Managers.Unit.SelectedUnits.Count < 6)
            return;

        Managers.Stage.EnterStage();
        //HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.Stage);
    }

    //public int GetNumberOfCells(EnhancedScroller scroller)
    //{
    //    return (Managers.Unit.GetRetainedUnit().Count / 3 <= 0) ? 1 : (int)(System.Math.Ceiling((float)Managers.Unit.GetRetainedUnit().Count / 3f));
    //}

    //public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    //{
    //    return 250f;
    //}

    //public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    //{
    //    UnitSlotHolder slot = scroller.GetCellView(SlotPrefab) as UnitSlotHolder;

    //    slot.SetUI(cellIndex);

    //    return slot;
    //}
}
