using UnityEngine;

public interface IStartSpawner
{
    public Transform[] SpawnPoints_Front { get; set; }
    public Transform[] SpawnPoints_Middle { get; set; }
    public Transform[] SpawnPoints_Back { get; set; }
}
