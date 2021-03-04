using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityTemplate : ScriptableObject
{
    [SerializeField] public string Name = "New Ability";
    [SerializeField] public string Description = "";
    [SerializeField] public Sprite Icon = null;
    
	[SerializeField] public float channelDuration;
	[SerializeField] public float castDuration;
	[SerializeField] public float cooldownDuration;
	[SerializeField] public float interruptionDuration;


    //This is for easier serialisation of behaviours


    [Header("Behaviours")]
    [SerializeField] public List<BaseBehaviourData> behaviours = new List<BaseBehaviourData>();

	public GameObject CreateAbilityObject()
	{
		GameObject obj = new GameObject(Name);
		Ability ability = obj.AddComponent<Ability>();

		ability.template = this;

		foreach (BaseBehaviourData behaviour in behaviours)
		{
			behaviour.AddBehaviourScript(obj);
		}

		return obj;
	}

}
