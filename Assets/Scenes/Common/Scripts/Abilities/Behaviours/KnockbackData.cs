using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Knockback")]
public class KnockbackData : BaseBehaviourData
{
    [Header("Knockback Settings")]
    [SerializeField] float force = 3f;
}
