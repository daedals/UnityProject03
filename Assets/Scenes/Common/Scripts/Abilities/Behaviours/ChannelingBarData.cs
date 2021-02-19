using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Channeling Bar")]
public class ChannelingBarData : BaseBehaviourData
{
    public ChannelingBarData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.CHANNELING;
    }
}
