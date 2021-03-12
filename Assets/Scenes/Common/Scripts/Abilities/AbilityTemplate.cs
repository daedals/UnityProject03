using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[System.Serializable]
[CreateAssetMenu(menuName = "Ability/New Ability")]
public class AbilityTemplate : ScriptableObject
{
    [SerializeField] public string Name = "New Ability";
    [SerializeField] public string Description = "";
    [SerializeField] public Sprite Icon = null;
    
	[SerializeField] public float channelDuration;
	[SerializeField] public float castDuration;
	[SerializeField] public float cooldownDuration;
	[SerializeField] public float interruptionDuration;


	[Header("State Settings")]
	[SerializeField] public List<ExecutionMaskDurationEntry> StateDurations;


    [Header("Behaviours")]
    [SerializeField] public List<BaseBehaviourData> behaviours = new List<BaseBehaviourData>();

	public bool Parse(uint templateNetId, uint ownerNetId)
	{
		if (NetworkIdentity.spawned.TryGetValue(ownerNetId, out NetworkIdentity owner) && NetworkIdentity.spawned.TryGetValue(templateNetId, out NetworkIdentity template))
		{
			template.transform.parent = owner.transform;
			template.name = Name;

			template.GetComponent<MecanimAbility>().Data = this;

			foreach (BaseBehaviourData behaviour in behaviours)
			{
				behaviour.Setup(template.gameObject);
			}

			return true;
		}

		return false;
	}

}
