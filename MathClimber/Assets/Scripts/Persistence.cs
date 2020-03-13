using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Persistence : MonoBehaviour {


	public static int currentChar {
		get {
			if (PlayerPrefs.HasKey ("CurChar"+curPlayer)) {
				return PlayerPrefs.GetInt ("CurChar"+curPlayer);
			}
			else {
				return 0;
			}
		}
		set{ 
			PlayerPrefs.SetInt("CurChar"+curPlayer, value);
		}
	}
		
	public static int money {
		get { 
			if (PlayerPrefs.HasKey ("Money"+curPlayer)) {
				return PlayerPrefs.GetInt ("Money"+curPlayer);
			}
			else {
				return 0;
			}

		}
		set{ 
			int storedMoney = 0;
			if (PlayerPrefs.HasKey ("Money" + curPlayer)) {
				storedMoney = PlayerPrefs.GetInt ("Money" + curPlayer);
			}
			bool isPositive = value > storedMoney;
			PlayerPrefs.SetInt("Money"+curPlayer, value);
			if (isPositive){
				int lifetime = moneyLifetime;//PlayerPrefs.GetInt ("LifetimeMoney"+curPlayer);
				lifetime += value - storedMoney;
				//Debug.Log ("U got "+lifetime+ " lifetime monie");
				PlayerPrefs.SetInt ("LifetimeMoney"+curPlayer, lifetime);
			}
		}
	}

	public static int moneyLifetime {
		get{ 
			if (PlayerPrefs.HasKey ("LifetimeMoney"+curPlayer)) {
				return PlayerPrefs.GetInt ("LifetimeMoney"+curPlayer);
			}
			else {
				return 0;
			}
		}
	}

	public static int campaignXp {
		get { 
			return PlayerPrefs.GetInt ("CampaignXp"+curPlayer);
		}

		set{ 
			PlayerPrefs.SetInt("CampaignXp"+curPlayer, value);
		}
	}
		
	public static int freestyleXp {
		get { 
			//Debug.Log (Retrieving);
			return PlayerPrefs.GetInt ("FreestyleXp"+curPlayer);
		}

		set{ 
			PlayerPrefs.SetInt("FreestyleXp"+curPlayer, value);
		}
	}

	public static int bombs {
		get {
			if (PlayerPrefs.HasKey ("Bombs"+curPlayer)) {
				return PlayerPrefs.GetInt ("Bombs"+curPlayer);
			}
			else {
				return 0;
			}
		}
		set{ 
			PlayerPrefs.SetInt("Bombs"+curPlayer, value);
		}
	}

	public static void Clear(){
		PlayerPrefs.DeleteAll();
		if (FMC_GameDataController.instance != null) {
			FMC_GameDataController.instance.resetStoryModeData ();
		}
	}

	public static string curPlayer{
		get{
			if (FMC_GameDataController.instance != null) {
				return FMC_GameDataController.instance.getCurrentPlayerName ();
			}
			else {
				return string.Empty;
			}
		}
	}
	//public static void StorePurchases(CharacterProfile[] chars){
	public static void StorePurchases(CharacterStorage storage){
		string result = string.Empty;
		for (int i = 0; i < storage.characters.Length; i++) {
			if (storage.GetCharacter(i).isPurchased) {
				result += 1;
			}
			else {
				result += 0;
			}
				
		}
		PlayerPrefs.SetString ("Chars" + curPlayer, result);
	}

	public static bool[] GetPurchased(){
		if (PlayerPrefs.HasKey ("Chars" + curPlayer)) {

			string bin = PlayerPrefs.GetString ("Chars" + curPlayer);
			bool[] result = new bool[bin.Length];
			for (int i = 0; i < result.Length; i++) {
				result [i] = Convert.ToBoolean(int.Parse (new string(bin[i], 1)));

			}
			return result;
		}
		else{
			return new bool[0];
		}
	}

	public static int GetPurchaseNumber(){
		bool[] b = GetPurchased ();
		int result = 0;

		for (int i = 0; i < b.Length; i++) {
			
			if (b[i]) {
				result++;
			}
		}
	
		return result;
	}

	public static bool GetPurchased(int id){
		if (PlayerPrefs.HasKey ("Chars" + curPlayer)) {
	
			string bin = PlayerPrefs.GetString ("Chars" + curPlayer);
			return Convert.ToBoolean(int.Parse (new string(bin[id], 1)));
		}
		else{
			return false;
		}
	}
}
