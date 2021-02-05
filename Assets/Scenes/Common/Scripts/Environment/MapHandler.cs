using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapHandler
{
    private readonly IReadOnlyCollection<string> _maps;
    private readonly int _numberOfRounds;

    private int _currentRound;
    private List<string> _remainingMaps;


    public MapHandler(MapSet mapSet, int numberOfRounds)
    {
        _maps = mapSet.Maps;
        _numberOfRounds = numberOfRounds;

        ResetMaps();
    }

    public bool IsComplete => _currentRound == _numberOfRounds;

    public string NextMap
    {
        get
        {
            if (IsComplete) return null;

            _currentRound++;

            if (_remainingMaps.Count == 0) ResetMaps();

            string map = _remainingMaps[UnityEngine.Random.Range(0, _remainingMaps.Count)];

            _remainingMaps.Remove(map);

            return map;
        }
    }


    private void ResetMaps() => _remainingMaps = _maps.ToList();
}
