using System;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanel : UI_Base
{
    [SerializeField] private List<UI_Button> LobbyButtons;


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
        switch (type)
        {
            case HDU.Define.CoreDefine.ELobbySceneType.MainLobby:
                foreach (var item in LobbyButtons)
                    item.gameObject.SetActive(true);
                break;
            default:
                foreach (var item in LobbyButtons)
                    item.gameObject.SetActive(false);
                break;
        }
    }

    public void OnClickAdv()
    {
        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.Dungeon);
    }
}
