using HDU.Managers;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CardPanel : UI_Base
{
    [SerializeField] RectTransform CardHolderRect;
    [SerializeField] CardSlot[] CardSlots;

    protected override void AwakeInit()
    {
        base.AwakeInit();
        HideCard().Forget();
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenFirstCard, ShowFirstCard);
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenCard, ShowCard);
        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnHideCard, ForcedCloseCardUI);
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenFirstCard, ShowFirstCard);
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenCard, ShowCard);
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnHideCard, ForcedCloseCardUI);
    }

    public void ShowFirstCard()
    {
        CardHolderRect.gameObject.SetActive(true);

        for (int i = 0; i < CardSlots.Length; i++)
        {
            var data = Managers.Card.DrawCardDatas[i];
            switch (data.CardType)
            {
                case HDU.Define.CoreDefine.ECardType.Cost:
                    CardSlots[i].SetUI_Cost(data.Cost, true);
                    break;
                case HDU.Define.CoreDefine.ECardType.Unit:
                    CardSlots[i].SetUI_Unit(data.UnitType, true);
                    break;
            }
        }

        HideCard(1.5f).Forget();
    }

    public void ShowCard()
    {
        CardHolderRect.gameObject.SetActive(true);

        for (int i = 0; i < CardSlots.Length; i++)
        {
            var data = Managers.Card.DrawCardDatas[i];
            switch (data.CardType)
            {
                case HDU.Define.CoreDefine.ECardType.Cost:
                    CardSlots[i].SetUI_Cost(data.Cost, false);
                    break;
                case HDU.Define.CoreDefine.ECardType.Unit:
                    CardSlots[i].SetUI_Unit(data.UnitType, false);
                    break;
            }
        }
    }

    private async UniTask HideCard(float delay = 0f)
    {
        if (delay != 0)
            await UniTask.Delay(System.TimeSpan.FromSeconds(delay));
        CardHolderRect.gameObject.SetActive(false);
    }
    private void ForcedCloseCardUI()
    {
        if(CardHolderRect.gameObject.activeInHierarchy)
        CardHolderRect.gameObject.SetActive(false);
    }
}
