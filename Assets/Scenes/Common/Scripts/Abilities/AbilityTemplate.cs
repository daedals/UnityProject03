using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityTemplate : ScriptableObject
{
    [SerializeField] public string Name = "New Ability";
    [SerializeField] public string Description = "";
    
	[SerializeField] public float channelDuration;
	[SerializeField] public float castDuration;
	[SerializeField] public float cooldownDuration;
	[SerializeField] public float interruptionDuration;


    //This is for easier serialisation of behaviours


    [Header("Behaviours")]
    [SerializeField] public List<BaseBehaviourData> behaviours = new List<BaseBehaviourData>();

    public Ability Parse()
    {
        List<BaseBehaviour> behaviourInstances = new List<BaseBehaviour>();

        foreach(BaseBehaviourData data in behaviours)
        {
            Type T = data.GetBehaviourType();

            var obj = (BaseBehaviour)System.Activator.CreateInstance(T, new object[] { data });
            behaviourInstances.Add(obj);
        }

        Ability ability = new Ability(behaviourInstances, this);
        return ability;
        
            // Ability ability = new Ability(rawAbility.abilityData);

            // foreach (var abilityEvent in rawAbility.events)
            // {
            //     foreach (var action in abilityEvent.Actions)
            //     {
            //         // get the type of action class
            //         var actionType = System.Type.GetType(action.Type.ToString());
            //         var actionObj = (BaseAction)System.Activator.CreateInstance(actionType, action);

            //         // register the corresponding event callback in ability class
            //         ability.EventRegister[abilityEvent.Type.ToString()].Add(actionObj);
            //     }
            // }

            // return ability;
    }
}
