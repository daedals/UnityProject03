using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEditor.Callbacks;

public class AbilityEditor : Editor
{
	[MenuItem("Game/Ability/Create New")]
	public static void CreateNewAbility()
	{
		// AbilityEditor editor = GetWindow<AbilityEditor>();
		// editor.Show();
		AssetUtility.CreateAsset<AbilityTemplate>("Abilities/");
	}
    
}
