using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
	[MenuItem("Assets/Create/CharacterProfile")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<CharacterProfile> ();
	}
}