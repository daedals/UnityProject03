using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : BaseBehaviour
{
    public override BaseBehaviourData Data { get => data; set => data = (KnockbackData)value; }
    [SerializeField] protected new KnockbackData data;

	public override void Initialize(Ability ability)
	{
		base.Initialize(ability);

		ability.AbilityHitTarget += OnAbilityTargetHit;
	}

	private void OnAbilityTargetHit(GameObject obj, GameObject other)
	{
		// we need a transform here to now from where the knockback shold be applied from
		// in case of a linear projectile, the projectiles position
		// in case of a ability around the player, the players position
		Debug.Log("Ability hit Target, Knockback should occur now (but doesn't because there is no logic for it)");
	}
}
