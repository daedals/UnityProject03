using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/AreaOfEffect")]
public class AreaOfEffectData : BaseBehaviourData
{
    [Header("AoE Settings")]
    [SerializeField] public float range = 1f;
    [SerializeField] public string affectedTag = "Player";
    
    public AreaOfEffectData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.NONE;
    }
}
