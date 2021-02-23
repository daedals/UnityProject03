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
        var abilities = Resources.LoadAll<AbilityTemplate>("Abilities") as AbilityTemplate[];
        Debug.Log("Loading " + abilities.Length + " AbilityTemplate" + (abilities.Length > 1 ? "s" : ""));

        foreach (AbilityTemplate template in abilities)
        {
            Database[template.Name] = Parse(template);
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
    
    public Ability Parse(AbilityTemplate template)
    {
        List<BaseBehaviour> behaviourInstances = new List<BaseBehaviour>();

        foreach(BaseBehaviourData data in template.behaviours)
        {
            System.Type T = data.GetBehaviourType();

            var obj = (BaseBehaviour)System.Activator.CreateInstance(T, new object[] { data });
            behaviourInstances.Add(obj);
        }

        Ability ability = new Ability(behaviourInstances, template);
        return ability;
    }
}
