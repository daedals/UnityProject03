using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Linear Projectile")]
public class LinearProjectileData : BaseBehaviourData
{
    [Header("Projectile Settings")]
    [SerializeField] public float movementSpeed = 3f;
    [SerializeField] public float lifeTime = 3f;
    

    [Header("Prefab Pool Settings")]
    [SerializeField] public GameObject projectilePrefab = null;
    [SerializeField] public int poolSize = 3;

    public LinearProjectileData()
    {
        ExecutionMask = BaseBehaviour.ExecutionMask.CASTING;
    }
}
