using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//Self-registering
public class RecolorListener : MonoBehaviour {
	RecolorManager manager;

	new Light light;
	public bool isShade;
	public bool ignoreChildren;
    public bool isButton = false;
    
	Image[] images;
	Text[] uitxt;
	TextMeshProUGUI[] txt;
	TextMesh[] craptxt;

	MeshRenderer[] renderers;
	SpriteRenderer[] sprites;

	void Awake () {
		light = GetComponent<Light> ();
		if (!ignoreChildren) {
			images = GetComponentsInChildren<Image> ();
			txt = GetComponentsInChildren<TextMeshProUGUI> ();
			uitxt = GetComponentsInChildren<Text> ();
			craptxt = GetComponentsInChildren<TextMesh> ();
			sprites = GetComponentsInChildren<SpriteRenderer> ();
			renderers = GetComponentsInChildren<MeshRenderer> ();
		}
		else {
			images = GetComponents<Image> ();
			txt = GetComponents<TextMeshProUGUI> ();
			uitxt = GetComponents<Text> ();
			craptxt = GetComponents<TextMesh> ();
			sprites = GetComponents<SpriteRenderer> ();
			renderers = GetComponents<MeshRenderer> ();
		}
			

		manager = RecolorManager.Get ();
		
		if (manager == null) {
			manager = FindObjectOfType<RecolorManager> ();
		}
		SelfRegister ();
	}

	void Start () {
		manager.Deregister (this);
		SelfRegister ();
	}
	

	public void SelfRegister(){
		
		if (this == null) {
			Debug.Log ("Something went horribly wrong");
		}
		else
			manager.Register (this);

	}
	public void Recolor(Color shade, Color highlight){

		Recolor (shade, highlight, shade * highlight, shade);

	}
	public void Recolor (Color shade, Color highlight, Color text,  Color btn) {

		if (light != null && enabled) {
			light.color = (isShade) ? shade : highlight;
		}
		if (images.Length > 0 && enabled) {
			for (int i = 0; i < images.Length; i++) {
				if (images [i].name.StartsWith ("Inner") || isShade) {
					
					images [i].color = highlight;
				}
				else if (images [i].name.StartsWith ("Ignore")) {
					//Do nothing
				}
				else {
					images [i].color = shade;
				}
					
			}


		}
		if (txt.Length > 0 && enabled) {
			for (int i = 0; i < txt.Length; i++) {
				if (!txt [i].name.StartsWith ("Ignore")) {
					txt [i].color = text;
				}



			}
		}
		for (int i = 0; i < sprites.Length; i++) {
            if (!isButton) {
    			if (!isShade) {
    				sprites [i].color = highlight;
    			}
    			else {
    				sprites [i].color = shade;
    			}
            } else {
                sprites [i].color = btn;
            }
		}

		for (int i = 0; i < craptxt.Length; i++) {

			craptxt [i].color = text;

		}

		for (int i = 0; i < uitxt.Length; i++) {
			if (!uitxt [i].name.StartsWith ("Ignore")){
				uitxt [i].color = text;		
			}
		}
		for (int i = 0; i < renderers.Length; i++) {
			if (!isShade) {
				renderers [i].material.color = highlight;
			}
			else {
				renderers [i].material.color = shade;
			}
		}
	}
	void OnDestroy () {
		manager.Deregister(this);
	}
}
