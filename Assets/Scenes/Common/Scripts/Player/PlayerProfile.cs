using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    // public enum AbilityIdentifier
    // {
    //     BASIC_ABILITY_1,
    //     BASIC_ABILITY_2,
    //     BASIC_ABILITY_3,
    //     MOVEMENT_ABILITY
    // }
    
    // [SerializeField]
    // public Dictionary<AbilityIdentifier, string> Abilities = new Dictionary<AbilityIdentifier, string> 
    // {
    //     { AbilityIdentifier.BASIC_ABILITY_1, string.Empty },
    //     { AbilityIdentifier.BASIC_ABILITY_2, string.Empty },
    //     { AbilityIdentifier.BASIC_ABILITY_3, string.Empty },
    //     { AbilityIdentifier.MOVEMENT_ABILITY, string.Empty }
    // };

    [SerializeField] public string Ability1;
    [SerializeField] public string Ability2;
    [SerializeField] public string Ability3;
}
