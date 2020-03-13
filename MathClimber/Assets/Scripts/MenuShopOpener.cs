using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShopOpener : MonoBehaviour {
	FMC_MenuController menu;
	// Use this for initialization
	void Start () {
		menu = FindObjectOfType<FMC_MenuController> ();
	}
	
	// Update is called once per frame
	void Update () {
		

	
	}

	public void OpenShop(){
		menu.playStoryMode ();
		LeanTween.delayedCall (1, ()=>{
			FindObjectOfType<ShopScreen>().Open();
		});
	}
}
