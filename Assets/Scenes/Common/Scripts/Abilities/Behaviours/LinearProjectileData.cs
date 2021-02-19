using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Linear Projectile")]
public class LinearProjectileData : BaseBehaviourData
{
    [SerializeField] public GameObject projectilePrefab = null;

    [Header("Projectile Settings")]
    [SerializeField] public float movementSpeed = 3f;
    [SerializeField] public float lifeTime = 3f;

    public LinearProjectileData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.CASTING;
    }
}
