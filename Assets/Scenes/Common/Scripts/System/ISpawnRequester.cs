using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnRequester
{
    ulong Id { get; set; }

    // List<System.Tuple<int, GameObject>> SpawnedInstances { get; set; }
}
