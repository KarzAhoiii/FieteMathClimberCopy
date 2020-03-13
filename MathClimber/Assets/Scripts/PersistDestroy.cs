using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistDestroy : MonoBehaviour {
	public float time;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		Destroy (gameObject, time);
	}
	

}
