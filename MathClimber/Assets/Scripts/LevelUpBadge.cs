using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBadge : MonoBehaviour {

	XPController exp;
	TMPro.TextMeshPro[] texts;
	// Use this for initialization
	void Start () {
		exp = FindObjectOfType<XPController> ();
		texts = GetComponentsInChildren<TMPro.TextMeshPro> ();
		for (int i = 0; i < texts.Length; i++) {
			texts[i].text = ""+(exp.GetLevel ()+1);	
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
