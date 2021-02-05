using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotationHandler : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] 
    private EntityDataContainer _playerData = null;
    
    private readonly List<IRotationModifier> _modifiers = new List<IRotationModifier>();

	public override void OnStartAuthority()
	{
        enabled = true;
	}

    [ClientCallback]
    private void Update() => Rotate();

    [Client]
    public void AddModifier(IRotationModifier modifier) => _modifiers.Add(modifier);

    [Client]
    public void RemoveModifier(IRotationModifier modifier) => _modifiers.Remove(modifier);

    [Client]
    private void Rotate()
    {
        Quaternion rotation = transform.rotation;
        int priority = -1;

        foreach (IRotationModifier modifier in _modifiers)
        {
            if (modifier.RMPriority > priority)
            {
                priority = modifier.RMPriority;
                rotation = modifier.RMValue;
            }
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _playerData.rotationSpeed * Time.deltaTime);
    }
}
