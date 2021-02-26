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
    private static Dictionary<string, AbilityTemplate> Database = new Dictionary<string, AbilityTemplate>();
    public static AbilityDatabase Instance { get; private set; }

    private void OnEnable()
    {
        if (!NetworkServer.active) return;

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAbilityData();
    }

    void LoadAbilityData()
    {
        var abilities = Resources.LoadAll<AbilityTemplate>("Abilities") as AbilityTemplate[];
        Debug.Log("Loading " + abilities.Length + " AbilityTemplate" + (abilities.Length > 1 ? "s" : ""));

        foreach (AbilityTemplate template in abilities)
        {
            Database[template.Name] = template; //ImplementAbility(template);
            Debug.Log("Loaded Ability: " + template.Name);
        }
    }

    public GameObject GetAbility(string abilityName, GameObject player)
    {
        if (Instance == null) throw new System.Exception("No instance of AbilityDatabase found, function call failed.");

        if (Database.ContainsKey(abilityName))
        {
            GameObject abilityClone = ImplementAbility(Database[abilityName]);
            abilityClone.transform.parent = player.transform;

            return abilityClone;
        }

        throw new System.Exception("Ability: " + abilityName + " doesn't exist");
    }
    
    public GameObject ImplementAbility(AbilityTemplate template)
    {
        GameObject abilityGameObject = template.CreateAbilityObject();
        NetworkServer.Spawn(abilityGameObject, connectionToClient);

        List<BaseBehaviour> behaviourInstances = new List<BaseBehaviour>();

        foreach(BaseBehaviourData data in template.behaviours)
        {
            GameObject behaviourGameObject = data.CreateBehaviourObject();
            behaviourGameObject.transform.SetParent(abilityGameObject.transform);
            
            NetworkServer.Spawn(behaviourGameObject, connectionToClient);
        }

        return abilityGameObject;
    }
}
