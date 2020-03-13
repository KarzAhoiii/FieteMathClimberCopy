using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStorage : MonoBehaviour {
	
	public CharacterProfile[] characters;

	private static CharacterStorage instance;
	// Use this for initialization
	void Start () {
    
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else if (instance != this){
			Destroy(gameObject);
			Debug.LogWarning("An instance of CharacterStorage already exists");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public CharacterProfile GetCharacter(int id){
		for (int i = 0; i < characters.Length; i++) {
			if (characters[i].id == id){
				return characters[i];
			}
		}

		Debug.LogError("Character profile with id "+id+" not found!");
		return null;
	}

	public static CharacterProfile curChar{
		get{return instance.GetCharacter (Persistence.currentChar); }
	}

	public void Wipe(){
		foreach (var item in characters) {
			item.isPurchased = false;
		}
		GetCharacter (0).isPurchased = true;
		Persistence.StorePurchases (this);
	}
}
