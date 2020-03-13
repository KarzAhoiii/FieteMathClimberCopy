using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IphoneXCanvas : MonoBehaviour {


	public float value;
	// Use this for initialization
	void Awake () {
		float ratio = (float)Screen.width / Screen.height;
		if (ratio < 0.5f){
			CanvasScaler c = GetComponent<CanvasScaler> ();
			if (c != null) {
				c.matchWidthOrHeight = value;
			}

		}
	}

}
