using HDU.Managers;
using UnityEngine;

public class BattleScene : BaseScene
{
    [SerializeField] Transform Spawner;
    public override void Init()
    {
        base.Init();

        Managers.Event.RegistEvent(HDU.Define.CoreDefine.EEventType.OnMoveSpawnerNext, OnMoveSpawnerNext);
        HDU.Managers.Managers.Click.SetCamera(GameObject.Find("Object_Camera").GetComponent<Camera>());
        Managers.Stage.StartBattleLoop().Forget();
        Spawner.transform.position = Vector3.zero;
    }
    public override void Clear()
    {
        Managers.Event.RemoveEvent(HDU.Define.CoreDefine.EEventType.OnMoveSpawnerNext, OnMoveSpawnerNext);
    }

    private void OnMoveSpawnerNext()
    {
        Vector3 pos = Spawner.transform.position;
        pos.x = pos.x + 18;
        Spawner.transform.position = pos;
    }
}
