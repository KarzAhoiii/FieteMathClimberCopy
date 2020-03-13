using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfile : ScriptableObject {
	public GameObject prefab;
	//public Sprite portrait;
	public GameObject portrait;
	public GameObject particle;
	public Color primary;
	public Color secondary;
    public Color buttonColor;
	public Color textcol;
	public int priceOverride;
	public bool isPurchased;
	public bool isLocked;
	public int id;
	public int rewardBonus;

	public AudioClip jumpSound;
	public AudioClip jumpVoice;
	public AudioClip fallVoice;
	public AudioClip landSound;
	public AudioClip specialVoice;
	public AudioClip tapVoice;

	public int price{
		get{ 
			if (priceOverride == 0) {
				int row = Mathf.FloorToInt (id / 3);
				int basePrice = 100;
				float multiplier = 1.5f;
				float prc = basePrice;
				for (int i = 1; i <= row; i++) {
					prc = prc * multiplier;
				}
				int result = Mathf.FloorToInt (Mathf.RoundToInt (prc / 10) * 10);
				if (result > 1000) {
					if (result % 500 < 250) {
						result = Mathf.FloorToInt (result - result % 500);
					}
					else {
						result = Mathf.FloorToInt (Mathf.RoundToInt (prc / 1000) * 1000);
					}
				}
				return Mathf.FloorToInt (result - result % 10);
			}
			else {
				return priceOverride;
			}
		}
	}
}
