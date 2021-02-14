using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*##########################################################################################################################

This Database is can only exist as a singleton and is instanciated by the NetworkManagerCustom when the server is started.
It loads all Assets of "AbilityTemplates" (datacontainer inheriting from ScriptableObject) in "Resource/Abilities".

Each asset is then instantiated as an "Ability" (Ability.cs) and cached in a dict where the key is the variable Name of the 
template. The Player prefab has a component "PlayerAbilityManager" that clones all relevant Abilities (dictated by Player
prefabs component "PlayerProfile") from the Database.

##########################################################################################################################*/

public class AbilityDatabase : NetworkBehaviour
{
    private static Dictionary<string, Ability> Database = new Dictionary<string, Ability>();
    public static AbilityDatabase Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAbilityData();
    }

    void LoadAbilityData()
    {
        // var abilities = Resources.LoadAll<DataDrivenAbility>("Abilities") as DataDrivenAbility[];
        // Debug.Log("Loading " + abilities.Length + " abilites:");
        // for (int i = 0; i < abilities.Length; i++)
        // {
        //     DataDrivenAbility rawAbility = abilities[i];
        //     Debug.Log(rawAbility.abilityData.Name);
        //     Ability ability = DataDrivenAbility.Parse(rawAbility);
        //     Database[ability.abilityData.Name] = ability;
        // }
        var abilities = Resources.LoadAll<AbilityTemplate>("Abilities") as AbilityTemplate[];
        Debug.Log("Loading " + abilities.Length + " AbilityTemplates");

        foreach (AbilityTemplate template in abilities)
        {
            Ability ability = template.Parse();
            Database[template.Name] = ability;
            
            Debug.Log("Loaded Ability: " + template.Name);
        }
    }

    public static Ability GetAbility(string abilityName)
    {
        if (Instance == null) throw new System.Exception("No instance of AbilityDatabase found, function call failed.");

        if (Database.ContainsKey(abilityName))
        {
            return Database[abilityName].Clone() as Ability;
        }

        throw new System.Exception("Ability: " + abilityName + " doesn't exist");
    }
}
