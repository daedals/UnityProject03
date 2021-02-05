using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Data Container", menuName = "Data/Entity Data Container")]
public class EntityDataContainer : ScriptableObject
{
    [Header("Rotation Attributes")]
    [SerializeField] public float rotationSpeed;

    [Header("Movement Attributes")]
    [SerializeField]public float movementSpeed;
    [SerializeField]public float acceleration;

    [Header("Health Attributes")]
    [SerializeField]public float MaximumHealth;
    [SerializeField]public float CurrentHealth;
}
