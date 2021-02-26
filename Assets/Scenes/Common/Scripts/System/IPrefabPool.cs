using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public interface IPrefabPool
{
    int PoolSize { get; }
    // SyncList<GameObject> Pool { get; }
    List<GameObject> Pool { get; }
    GameObject Prefab { get; }

    void InitializePool();
    GameObject GetFromPool();
    GameObject SpawnPrefab(Vector3 position, Quaternion rotation);
    void UnspawnPrefab(GameObject instance);
}
