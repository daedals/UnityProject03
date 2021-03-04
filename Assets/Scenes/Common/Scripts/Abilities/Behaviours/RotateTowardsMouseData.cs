using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/RotateTowardsMouse")]
public class RotateTowardsMouseData : BaseBehaviourData
{
    public RotateTowardsMouseData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.CHANNELING | BaseBehaviour.ExecutionMask.CASTING;
    }
}
