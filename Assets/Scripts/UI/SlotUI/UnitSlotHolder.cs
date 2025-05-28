using System.Collections.Generic;
using UnityEngine;

public class UnitSlotHolder : UI_Slot
{
    [SerializeField] UnitSlot[] UnitSlots;

    private void OnEnable()
    {
        HDU.Managers.Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    private void OnDisable()
    {
        HDU.Managers.Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    public void SetUI(int index)
    {
        var myUnits = HDU.Managers.Managers.Unit.RetainedUnitList;

        for (int i = 0; i < UnitSlots.Length; i++)
        {
            int unitIndex = (index * 3) + i;
            if (unitIndex >= (int)myUnits.Count)
            {
                UnitSlots[i].gameObject.SetActive(false);
                continue;
            }

            UnitSlots[i].gameObject.SetActive(true);
            UnitSlots[i].SetUnit(myUnits[unitIndex]);
        }
    }

    public void OnOpenLobbyTypeCallback(HDU.Define.CoreDefine.ELobbySceneType type)
    {
        if (type == HDU.Define.CoreDefine.ELobbySceneType.Deck)
        {
            foreach (var item in UnitSlots)
                item.SetActiveSpine(true);
        }
        else
        {
            foreach (var item in UnitSlots)
                item.SetActiveSpine(false);
        }
    }
}
