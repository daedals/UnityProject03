using System.Collections;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map Set", menuName = "Rounds/Map Set")]
public class MapSet : ScriptableObject
{
    [Scene]
    [SerializeField] private List<string> _maps = new List<string>();

    public IReadOnlyCollection<string> Maps => _maps.AsReadOnly();
}
