using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPPanel : MonoBehaviour {

	public GameObject levelRoot;
	TextMeshProUGUI[] leveltext;

	public Image xpBar1;
	public Image xpBar2;

	XPController exp;

	void Awake(){
		leveltext = levelRoot.GetComponentsInChildren <TextMeshProUGUI>();
		exp = FindObjectOfType<XPController> ();
		exp.panel = this;
	}


	public void OnEnable(){
		//Debug.Log ("I'm enabled, bitch");
		exp = FindObjectOfType<XPController> ();
		exp.panel = this;
		Refresh ();
	}
	public void SetXp(int xp, int limit){


		SetXp ((float)xp / limit);
		//Debug.Log ("Roo t " + transform.root.name);
		//Debug.Log (xpBar1.gameObject.activeSelf);		
		//Debug.Log ("Setting XP to " + xpBar1.fillAmount);
	}

	public void SetXp(float percent){
		xpBar1.fillAmount = percent;
		xpBar2.fillAmount = percent;
	}
	public void SetLevel(int l){
		for (int i = 0; i < leveltext.Length; i++) {
			leveltext [i].text = string.Empty + l;
		}

	}
		
	public void Refresh () {
		

		int level = exp.GetLevel ();

		SetXp(exp.levelProgress);


		SetLevel (level);
	}
}
