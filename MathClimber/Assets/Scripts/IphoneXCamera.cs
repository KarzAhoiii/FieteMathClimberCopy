using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IphoneXCamera : MonoBehaviour {


	public float orthoSize;


	// Use this for initialization
	void Awake () {
		float ratio = (float)Screen.width / Screen.height;
		if (ratio < 0.5f){
			Camera c = GetComponent<Camera> ();
			if (c != null) {
				c.orthographicSize = orthoSize;
			}

		}
	}

}
