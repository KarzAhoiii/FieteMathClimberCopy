using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IphoneXAnimation : MonoBehaviour {


	public RuntimeAnimatorController animController;


	// Use this for initialization
	void Awake () {
		
		float ratio = (float)Screen.width / Screen.height;
		if (ratio < 0.5f){
			Animator a = GetComponent<Animator> ();
			if (a != null) {
				a.runtimeAnimatorController = animController;
			}

		}
	}

}
