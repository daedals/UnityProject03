using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject
{
    public float ChannelTime = 1f;
    public float CastTime = 1f;

    public List<AbilityBehaviour> Behaviours = new List<AbilityBehaviour>();
}
