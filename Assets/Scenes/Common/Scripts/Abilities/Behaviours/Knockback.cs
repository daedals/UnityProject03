using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Knockback : BaseBehaviour
{
    public override BaseBehaviourData Data { get => data; set => data = (KnockbackData)value; }
    [SerializeField] private KnockbackData data;

	public override void Initialize()
	{
		GetComponent<Ability>().AbilityTargetsIdentified += OnAbilityTargetsIdentified;
	}

	private void OnAbilityTargetsIdentified(GameObject obj, List<GameObject> targets)
	{
		// we need a transform here to now from where the knockback shold be applied from
		// in case of a linear projectile, the projectiles position
		// in case of a ability around the player, the players position

		foreach (GameObject target in targets)
		{
			Vector3 forceVector = data.force * Vector3.ProjectOnPlane(target.transform.position - obj.transform.position, Vector3.up).normalized + Vector3.up;
			
			CmdAddForceToTarget(target.GetComponent<NetworkIdentity>().netId, forceVector);
		}
	}

	[Command]
	private void CmdAddForceToTarget(uint netId, Vector3 forceVector)
	{
		NetworkConnection conn = NetworkIdentity.spawned[netId].connectionToClient;

		TargetAddForce(conn, netId, forceVector);
	}

	[TargetRpc]
	private void TargetAddForce(NetworkConnection conn, uint netId, Vector3 forceVector)
	{
		GameObject target = NetworkIdentity.spawned[netId].gameObject;

		ForceReceiver forceReceiver;

		if (!target.TryGetComponent<ForceReceiver>(out forceReceiver)) throw new System.Exception($"Knockback could not be applied because target {target.name} is missing a force receiver.");

		forceReceiver.AddForce(forceVector);
	}
}
