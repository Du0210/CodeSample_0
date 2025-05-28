using UnityEngine;
using Spine.Unity;
using HDU.Managers;
using UnityEngine.UI;

public class UnitSlot : UI_Button
{
    [SerializeField] private SkeletonGraphic UnitSpine;
    [SerializeField] private Image SelectedImg;

    private HDU.Define.CoreDefine.EPlayerUnitType _unitType;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnEnable()
    {
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnSelectUnit, SetSelected);
    }

    private void OnDisable()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnSelectUnit, SetSelected);
    }

    public void SetUnit(HDU.Define.CoreDefine.EPlayerUnitType type)
    {
        _unitType = type;

        if (_scrollRect == null)
            _scrollRect = HDU.Utils.UnityUtils.GetComponentInNearestParent<ScrollRect>(transform);

        Managers.Unit.SetPlayerSpine(UnitSpine, type);
        SetSelected();
    }

    private void SetSelected()
    {
        SelectedImg.gameObject.SetActive(Managers.Unit.IsContainsUnit(_unitType));
    }

    public void OnClickSlot()
    {
        Managers.Unit.SelectUnit(_unitType);
    }

    public void SetActiveSpine(bool isActive)
    {
        UnitSpine.timeScale = isActive ? 1 : 0;
    }
}
