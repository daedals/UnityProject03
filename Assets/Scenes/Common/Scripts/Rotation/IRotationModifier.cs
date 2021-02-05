using UnityEngine;

public interface IRotationModifier
{
    Quaternion RMValue { get; }
    int RMPriority { get; }
}

