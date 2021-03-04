using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementSpeedModifier : NetworkBehaviour, IMovementSpeedModifier
{
    
    [Header("References")]
    [SerializeField] private MovementSpeedHandler _movementSpeedHandler = null;

    private readonly List<float> _modifiers = new List<float>(); 

	public override void OnStartAuthority()
	{
        enabled = true;
        Value = 1;
	}

    [ClientCallback]
    private void OnEnable() => _movementSpeedHandler.AddModifier(this);
    [ClientCallback]
    private void OnDisable() => _movementSpeedHandler.RemoveModifier(this);

    public float Value { get; private set; }

    [Client]
    private void Update() => CalculateValue();

    [Client]
    private void CalculateValue()
    {
        float value = 1;

        foreach (float modifier in _modifiers)
        {
            value *= modifier;
        }

        Value = value;
    }

    public void AddModifier(float modifier, float t = 0)
    {
        if (t < 0) return;

        _modifiers.Add(modifier);

        if (t > 0)
        {
            StartCoroutine(RemoveAfterSeconds(modifier, t));
        }
    }
    
    public void RemoveModifier(float modifier)
    {
        _modifiers.Remove(modifier);
    }

    IEnumerator RemoveAfterSeconds(float modifier, float t)
    {
        yield return new WaitForSeconds(t);
        _modifiers.Remove(modifier);
    }
    
}
