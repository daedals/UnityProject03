using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Behaviour/Knockback")]
public class KnockbackData : BaseBehaviourData
{
    // [HideInInspector] public static Type behaviourType = LinearProjectile;

    [Header("Knockback Settings")]
    [SerializeField] float force = 3f;
}

public class Knockback : BaseBehaviour
{
	public Knockback(KnockbackData data) : base(data) {}

	public override void Tick()
	{
		throw new System.NotImplementedException();
	}

	public override void OnEnter()
	{
		throw new System.NotImplementedException();
	}

	public override void OnExit()
	{
		throw new System.NotImplementedException();
	}

    public override object Clone()
    {
        return null;
    }
}
