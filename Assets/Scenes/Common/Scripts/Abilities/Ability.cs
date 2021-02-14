using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability : ICloneable
{
    public AbilityStateMachine stateMachine;
    public readonly List<BaseBehaviour> behaviours;
    public readonly AbilityTemplate template;
    public GameObject owner;


    public Ability(List<BaseBehaviour> behaviours, AbilityTemplate template)
    {
        this.behaviours = behaviours;
        this.template = template;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
        stateMachine = new AbilityStateMachine(behaviours, template, owner);
    }

    public object Clone()
    {
        List<BaseBehaviour> behavioursClone = new List<BaseBehaviour>();

        foreach (BaseBehaviour behaviour in behaviours)
        {
            behavioursClone.Add(behaviour.Clone() as BaseBehaviour);
        }

        Ability clone = new Ability(behavioursClone, template);

        return clone;
    }
}
