using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OverrideTap : MonoBehaviour, IPointerDownHandler, IPauseHandler{

	Animator anim;
	public bool ignorePause; 
	Button button;
	// Use this for initialization
	void Start () {
		anim = GetComponent <Animator>();
		button = GetComponent <Button> ();
		ClimberStateManager.Subscribe (this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown(PointerEventData pointer){
		if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Disabled")) {
			anim.ResetTrigger ("Pressed");
			anim.ResetTrigger ("Highlighted");
			anim.Play ("Pressed");
		}

	}

	public void Pause(){
		if (!ignorePause){
			anim.enabled = false;
			button.interactable = false;
		}
	}
	public void Unpause(){
		if (!ignorePause) {
			anim.enabled = true;
			button.interactable = true;
		}
	}
}
