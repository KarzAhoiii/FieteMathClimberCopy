using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolorManager : MonoBehaviour {

	List<RecolorListener> listeners;


	CharacterStorage charStore;
	int curScheme;

	private static RecolorManager instance;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else if (instance != this){
			Destroy(gameObject);
			Debug.LogWarning("An instance of RecolorManager already exists");
		}

		charStore = FindObjectOfType<CharacterStorage>();

	}
	



	public void Recolor (int id){

		CharacterProfile chara = charStore.GetCharacter (id);
		Recolor (chara.primary, chara.secondary, chara.textcol, chara.buttonColor);
		curScheme = id;
	}
	public void Recolor (Color shade, Color highlight, Color txt, Color btn){
		for (int i = 0; i < listeners.Count; i++) {
			listeners [i].Recolor (shade, highlight, txt, btn);
		}
	}

	public void Register( RecolorListener rl){
		Init ();
		listeners.Add (rl);
	}
	public void Deregister (  RecolorListener rl){
		listeners.Remove (rl);
	} 

	void Init(){
		if (listeners == null) {
			listeners = new List<RecolorListener> ();
		}
	}

	public Color mainColor{
		get { 
			return charStore.GetCharacter (curScheme).primary;
		}
	}
	public Color hilightColor{
		get { 
			return charStore.GetCharacter (curScheme).secondary;
		}
	}

	public static RecolorManager Get () {
		
		return instance;
	}
}
[System.Serializable]
public struct ColorPair{
	public Color primary;
	public Color secondary;
	public Color textcol;
}