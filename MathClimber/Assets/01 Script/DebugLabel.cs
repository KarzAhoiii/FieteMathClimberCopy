using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLabel : MonoBehaviour {

	
	public Text label;
	public static DebugLabel instance;
	
	void Awake () {
		instance = this;
		
	}
	
	// Update is called once per frame
	public static void SetLabel  (string val) {
	
		instance.label.text = val;

	}
}
