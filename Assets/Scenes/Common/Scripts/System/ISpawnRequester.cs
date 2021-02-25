using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnRequester
{
    ulong Id { get; set; }

    Queue<ulong> CollectionTickets { get; set; }
}
