using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Callbacks;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(AbilityTemplate))]
public class AbilityEditor : Editor
{

	private static IEnumerable<Type> inheritedBehaviours;
	private static List<string> behaviourChoices;

	void OnEnable()
	{
		inheritedBehaviours = GetInheritedBehaviours();
		
		behaviourChoices = inheritedBehaviours.Select(T => T.ToString()).ToList();
		behaviourChoices.Insert(0, "None");
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

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		return;

		serializedObject.Update();

		AbilityTemplate template = (AbilityTemplate)target;


		// EditorGUILayout.LabelField(template.Name, EditorStyles.boldLabel);

		HandleTextField("Name", "Name");

		HandleTextArea("Description", "Description");

		HandleFloatField("Channel Duration", "channelDuration");
		HandleFloatField("Cast Duration", "castDuration");
		HandleFloatField("Cooldown Duration", "cooldownDuration");
		HandleFloatField("Interruption Duration", "interruptionDuration");

		foreach (Type T in inheritedBehaviours)
		{
			EditorGUILayout.LabelField(T.ToString());
		}

		ShowBehaviourList(template);

		serializedObject.ApplyModifiedProperties();
	}

	private void ShowBehaviourList(AbilityTemplate template)
	{
		var sp = serializedObject.FindProperty("behaviours").Copy();

		if (!sp.isArray) throw new Exception("Behaviour extraction failed");

		sp.Next(true); // skip generic field
		sp.Next(true); // advance to array size field

		int length = 0;

		int newCount = EditorGUILayout.DelayedIntField("size", template.behaviours.Count);
		
		while (newCount < template.behaviours.Count)
			template.behaviours.RemoveAt( template.behaviours.Count - 1 );

		while (newCount > template.behaviours.Count)
			template.behaviours.Add(null);

		

		// for (int i = 0; i < template.behaviours.Count; i++)
		// {
		// 	Type T = template.behaviours[i];
		// 	int index = 0;

		// 	if (T != null)
		// 	{
		// 		index = behaviourChoices.IndexOf(T.ToString());
		// 	}

		// 	index = EditorGUILayout.Popup(index, behaviourChoices.ToArray());

		// 	if (index == 0)
		// 	{
		// 		template.behaviours[i] = null;
		// 		continue;
		// 	}

		// 	template.behaviours[i] = Type.GetType(behaviourChoices[index]);

		// 	ShowBehaviourItem(T);
		// }
	}

	private void ShowBehaviourItem(Type T)
	{



		// assign this to a behaivour
		// BaseBehaviour.ExecutionMask executionMask = BaseBehaviour.ExecutionMask.CASTING;
		// executionMask = (BaseBehaviour.ExecutionMask)EditorGUILayout.EnumFlagsField("Execution Mask", executionMask);
	}

	#region Field Handler

	private void HandleTextField(string label, string name)
	{
		string val = serializedObject.FindProperty(name).stringValue;

		EditorGUI.BeginChangeCheck();
		val = EditorGUILayout.TextField(label: label, val);

		if(EditorGUI.EndChangeCheck())
		{
			serializedObject.FindProperty(name).stringValue = val;
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void HandleTextArea(string label, string name)
	{
		string val = serializedObject.FindProperty(name).stringValue;

		GUIStyle style = new GUIStyle(EditorStyles.textArea);
		style.wordWrap = true;

		EditorGUI.BeginChangeCheck();

		EditorGUILayout.LabelField(label);
		val = EditorGUILayout.TextArea(val, style, GUILayout.Height(style.lineHeight * 5));

		if(EditorGUI.EndChangeCheck())
		{
			serializedObject.FindProperty(name).stringValue = val;
		}

		serializedObject.ApplyModifiedProperties();
	}

	private void HandleFloatField(string label, string name)
	{
		float val = serializedObject.FindProperty(name).floatValue;

		EditorGUI.BeginChangeCheck();
		val = EditorGUILayout.FloatField(label: label, val);

		if(EditorGUI.EndChangeCheck())
		{
			serializedObject.FindProperty(name).floatValue = val;
		}

		serializedObject.ApplyModifiedProperties();
	}

	#endregion
}
