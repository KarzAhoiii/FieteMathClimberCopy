using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour {

	int count;
	public Text txt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Increment(){
		count++;
		txt.text = "" + count;
	}
	public void Decrement(){
		count--;
		txt.text = "" + count;	
	}
	public void Reset (){
		count = 0;
		txt.text = "" + count;
	}

}
