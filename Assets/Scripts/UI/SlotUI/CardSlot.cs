using HDU.Managers;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Define = HDU.Define.CoreDefine;

public class CardSlot : UI_Button
{
    [SerializeField] SkeletonGraphic _spine;
    [SerializeField] TextMeshProUGUI _txtName;
    [SerializeField] TextMeshProUGUI _txtCost;
    [SerializeField] Image _imgCard;
    
    private Define.EPlayerUnitType _unitType;
    private int _cost = 0;
    private bool _isFirst = false;

    public void SetUI_Unit(Define.EPlayerUnitType type, bool isFirst)
    {
        IsAnim = !isFirst;
        _isFirst = isFirst;
        _imgCard.gameObject.SetActive(false);
        _spine.gameObject.SetActive(true);

        var unitData = Managers.Data.GetPlayerUnitTableData(type);
        _unitType = type;

        Managers.Unit.SetPlayerSpine(_spine, type);
        _txtName.text = unitData.Name;

        if (!Managers.Spawn.PlayerUnitDict.ContainsKey(type))
        {
            _cost = 5;
            _txtCost.text = "5";
        }
        else
        {
            _cost = (Managers.Spawn.PlayerUnitDict[type].Cost + 1) * 5;
            _txtCost.text = _cost.ToString();
        }
    }

    public void SetUI_Cost(int amount, bool isFirst)
    {
        IsAnim = !isFirst;
        _isFirst = isFirst;
        _imgCard.gameObject.SetActive(true);
        _spine.gameObject.SetActive(false);
        _txtCost.text = amount.ToString();
        _txtName.text = string.Empty;
        _cost = 0;
    }

    public void OnClickCard()
    {
        if (_isFirst)
            return;
        if (Managers.Goods.CardCost >= _cost)
        {
            Managers.Goods.AddOrRemoveCost(_cost, false);
            Managers.Event.InvokeEvent(Define.EEventType.OnDrawCost);
            Managers.Event.InvokeEvent(Define.EEventType.OnSelectCard, _unitType);
            Managers.Card.GiveCard();
        }
    }
}
