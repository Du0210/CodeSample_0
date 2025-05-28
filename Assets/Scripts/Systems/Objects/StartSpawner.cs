using UnityEngine;

public abstract class StartSpawner : MonoBehaviour, IStartSpawner
{
    [SerializeField] protected Transform UnitParent;
    [SerializeField] protected Transform[] SpawnPoints_Front;
    [SerializeField] protected Transform[] SpawnPoints_Middle;
    [SerializeField] protected Transform[] SpawnPoints_Back;
    [SerializeField] protected SpriteRenderer[] SpawnRenderers;

    Transform[] IStartSpawner.SpawnPoints_Front { get => SpawnPoints_Front; set => SpawnPoints_Front = value; }
    Transform[] IStartSpawner.SpawnPoints_Middle { get => SpawnPoints_Middle; set => SpawnPoints_Middle = value; }
    Transform[] IStartSpawner.SpawnPoints_Back { get => SpawnPoints_Back; set => SpawnPoints_Back = value; }

    protected HDU.Define.CoreDefine.ESpawnType _spawnType;

    public abstract void SetSpawner();
    //public abstract void SpawnUnit(SpawnType type, HDU.Define.CoreDefine.EUnitPosition spawnPos);
    public virtual Transform GetSpawnerTR(HDU.Define.CoreDefine.EUnitPosition type, int index)
    {
        Transform transform = null;
        switch (type)
        {
            case HDU.Define.CoreDefine.EUnitPosition.Front:
                transform = SpawnPoints_Front[index];
                break;
            case HDU.Define.CoreDefine.EUnitPosition.Middle:
                transform = SpawnPoints_Middle[index];
                break;
            case HDU.Define.CoreDefine.EUnitPosition.Back:
                transform = SpawnPoints_Back[index];
                break;
        }
        return transform;
    }
}