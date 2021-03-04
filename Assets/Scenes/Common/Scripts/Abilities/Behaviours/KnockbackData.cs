using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Knockback")]
public class KnockbackData : BaseBehaviourData
{
    [Header("Knockback Settings")]
    [SerializeField] public float force = 3f;
    
    public KnockbackData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.NONE;
    }
}
