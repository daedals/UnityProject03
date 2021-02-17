using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(AbilityTemplate))]
public class AbilityEditor : Editor
{

	private static IEnumerable<Type> inheritedBehaviours;

	void OnEnable()
	{
		inheritedBehaviours = GetInheritedBehaviours();
	}


	[MenuItem("Game/Ability/Create New")]
	public static void CreateNewAbility()
	{
		AssetUtility.CreateAsset<AbilityTemplate>("Abilities/");
	}

	private IEnumerable<Type> GetInheritedBehaviours()
	{
		/* https://stackoverflow.com/questions/5411694/get-all-inherited-classes-of-an-abstract-class/6944605 */

		List<Type> behaviours = Assembly.GetAssembly(typeof(BaseBehaviour)).GetTypes()
			.Where(T => T.IsClass && !T.IsAbstract && T.IsSubclassOf(typeof(BaseBehaviour))).ToList();

		return behaviours;
	}

	private void ShowBehaviourList(AbilityTemplate template)
	{
		int newCount = EditorGUILayout.DelayedIntField("size", template.behaviours.Count);
		
		while (newCount < template.behaviours.Count)
			template.behaviours.RemoveAt( template.behaviours.Count - 1 );
		while (newCount > template.behaviours.Count)
			template.behaviours.Add(null);

		var behaviourNames = new List<string>();
		foreach (Type T in inheritedBehaviours) behaviourNames.Add(T.ToString());

		for (int i = 0; i < template.behaviours.Count; i++)
		{
			Type T = template.behaviours[i];

			T = inheritedBehaviours.ToList()[ 
				EditorGUILayout.Popup(2, behaviourNames.ToArray())
			];

			if (T != null)
			{
				ShowBehaviourItem(T);
			}
		}
	}

	private void ShowBehaviourItem(Type T)
	{



		// assign this to a behaivour
		// BaseBehaviour.ExecutionMask executionMask = BaseBehaviour.ExecutionMask.CASTING;
		// executionMask = (BaseBehaviour.ExecutionMask)EditorGUILayout.EnumFlagsField("Execution Mask", executionMask);
	}

	public override void OnInspectorGUI()
	{
		// base.OnInspectorGUI();
		AbilityTemplate template = (AbilityTemplate)target;


		EditorGUILayout.LabelField(template.Name, EditorStyles.boldLabel);

		foreach (Type T in inheritedBehaviours)
		{
			EditorGUILayout.LabelField(T.ToString());
		}

		ShowBehaviourList(template);

		EditorUtility.SetDirty(template);
	}

    
}
