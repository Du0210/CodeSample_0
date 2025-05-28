using System.Collections.Generic;
using UnityEngine;

public class LTPanel : UI_Base
{
    [SerializeField] private List<GameObject> LobbyButtons;
    [SerializeField] private List<GameObject> DungeonButtons;
    [SerializeField] private List<GameObject> StageButtons;
    [SerializeField] private List<GameObject> DeckButtons;

    private Dictionary<HDU.Define.CoreDefine.ELobbySceneType, List<GameObject>> _dictBtnLists;

    protected override void AwakeInit()
    {
        base.AwakeInit();
        _dictBtnLists = new Dictionary<HDU.Define.CoreDefine.ELobbySceneType, List<GameObject>>();
        _dictBtnLists.Add(HDU.Define.CoreDefine.ELobbySceneType.MainLobby, LobbyButtons);
        _dictBtnLists.Add(HDU.Define.CoreDefine.ELobbySceneType.Dungeon, DungeonButtons);
        _dictBtnLists.Add(HDU.Define.CoreDefine.ELobbySceneType.Stage, StageButtons);
        _dictBtnLists.Add(HDU.Define.CoreDefine.ELobbySceneType.Deck, DeckButtons);
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
        foreach (var lists in _dictBtnLists)
        {
            if(lists.Key != type)
            {
                foreach (var item in lists.Value)
                    item.SetActive(false);
            }
        }

        foreach (var item in _dictBtnLists[type])
            item.SetActive(true);
    }

    public void OnClickBack_Dungeon()
    {
        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.MainLobby);
    }

    public void OnClickBack_Stage()
    {
        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.Dungeon);
    }

    public void OnClickBack_Deck()
    {
        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.Stage);
    }
}
