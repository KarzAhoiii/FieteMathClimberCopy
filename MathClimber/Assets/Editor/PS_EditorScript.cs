using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PS_WholePlayerSelectionLayout))]
public class PS_EditorScript : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PS_WholePlayerSelectionLayout myScript = (PS_WholePlayerSelectionLayout)target;
		if(GUILayout.Button("Set layout"))
		{
			myScript.setLayout();
		}
	}
}