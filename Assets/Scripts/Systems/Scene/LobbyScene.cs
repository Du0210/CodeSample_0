using Cysharp.Threading.Tasks;
using HDU.Managers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;


public class LobbyScene : BaseScene
{
    [Serializable]
    private class SceneObjEntry
    {
        public HDU.Define.CoreDefine.ELobbySceneType key;
        public GameObject go;
    }

    [Header("Lobby Camera")]
    [SerializeField] private float _zoomMinSize = 4.5f;
    [SerializeField] private float _zoomMaxSize = 5.5f;
    [SerializeField] private float _zoomSpeed = 1f;

    [Space(20)]
    [SerializeField] private List<SceneObjEntry> ListSceneObj = new List<SceneObjEntry>();
    [SerializeField] private IStartSpawner StartSpawner;

    private Dictionary<HDU.Define.CoreDefine.ELobbySceneType, GameObject> _dictSceneObj;
    private CancellationTokenSource _zoomCts;
    private UniTask _zoomTask;
    private bool _isSceneLoaded = false;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        HDU.Managers.Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    private void OnDisable()
    {
        HDU.Managers.Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, (HDU.Define.CoreDefine.ELobbySceneType type) => OnOpenLobbyTypeCallback(type));
    }

    public override void Clear()
    {
        CancelCamZoomLoop();
    }

    public override void Init()
    {
        if (_isSceneLoaded)
            return; 

        base.Init();

        _isSceneLoaded = true;
        SceneType = HDU.Define.CoreDefine.ESceneType.LobbyScene;

        _dictSceneObj = new Dictionary<HDU.Define.CoreDefine.ELobbySceneType, GameObject>();
        foreach (var item in ListSceneObj)
        {
            if(!_dictSceneObj.TryGetValue(item.key, out GameObject go))
                _dictSceneObj.Add(item.key, item.go);
        }

        HDU.Managers.Managers.Click.SetCamera(GameObject.Find("Object_Camera").GetComponent<Camera>());

        HDU.Managers.Managers.Event.InvokeEvent(HDU.Define.CoreDefine.EEventType.OnOpenLobbyType, HDU.Define.CoreDefine.ELobbySceneType.MainLobby);

        foreach (var item in ListSceneObj)
            item.go.transform.position = Vector3.zero;

        Managers.Stage.SetUnitsBattleState(HDU.Define.CoreDefine.EBattleState.None);
        Managers.Spawn.SetSpawner_Lobby(StartSpawner);
        Managers.Time.SetTimeRatio(1f);
    }

    public void StartCamZoomLoop()
    {
        CancelCamZoomLoop();

        _zoomCts = new CancellationTokenSource();
        _zoomTask = CamZoomLoop(_zoomCts.Token);
    }

    public void CancelCamZoomLoop()
    {
        if(_zoomCts != null && !_zoomCts.IsCancellationRequested)
        {
            _zoomCts.Cancel();
            _zoomCts.Dispose();
            _zoomCts = null;
        }
    }

    private async UniTask CamZoomLoop(CancellationToken token)
    {
        Camera cam = Camera.main;
        float t = 0f;

        while (!token.IsCancellationRequested)
        {
            t += Time.deltaTime * _zoomSpeed;
            float factor = Mathf.SmoothStep(0, 1f, Mathf.PingPong(t, 1f)); //Mathf.PingPong(t, 1f);
            cam.orthographicSize = Mathf.Lerp(_zoomMinSize, _zoomMaxSize, factor);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    public void OnOpenLobbyTypeCallback(HDU.Define.CoreDefine.ELobbySceneType type)
    {
        switch (type)
        {
            case HDU.Define.CoreDefine.ELobbySceneType.MainLobby:
                StartCamZoomLoop();
                break;
            default:
                CancelCamZoomLoop();
                break;
        }
        foreach (var item in _dictSceneObj)
        {
            if (item.Value != null && item.Key != type)
                item.Value.SetActive(false);
        }
        if (_dictSceneObj.TryGetValue(type, out GameObject go) && _dictSceneObj[type] != null)
        _dictSceneObj[type].SetActive(true);
    }
}
