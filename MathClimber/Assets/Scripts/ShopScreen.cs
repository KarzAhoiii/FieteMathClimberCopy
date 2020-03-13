using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreen : MonoBehaviour {

	public GameObject panel;
	public GameObject contentRoot;
	public GameObject shopItem;

	public Sprite[] coinIcons;
	List<ShopItem> items;

	CharacterStorage storage;
	GameController main;
	public BankController bank;
	public Animator shopAnim;
	public Animator curtainAnim;
	bool isOpening;
	bool _isClosing;
	float openingTimer;

	// Use this for initialization
	void Start () {
    
		panel.SetActive (true);
		//shopAnim = panel.GetComponent<Animator>();
		storage = FindObjectOfType<CharacterStorage> ();
		main = FindObjectOfType<GameController> ();
		items = new List<ShopItem> ();
		bool[] purchases = Persistence.GetPurchased ();
		bool isUnlocked = true;
		if (FMC_GameDataController.instance != null) {
			isUnlocked = FMC_GameDataController.instance.subscriptionIsActive ();
		}
		for (int i = 0; i < storage.characters.Length; i++) {
			
			storage.GetCharacter (i).isLocked = !isUnlocked;

			if (purchases.Length > 0) {
				storage.GetCharacter(i).isPurchased = purchases [i];
			}
			else if (i > 0){
				storage.GetCharacter(i).isPurchased = false;
			}
			SpawnItem (i);
		}
		if (purchases.Length == 0) {
			Save ();
		}
		shopAnim.SetBool("IsOpen", true);
		curtainAnim.SetBool ("IsOpen", true);
		panel.SetActive (false);
		curtainAnim.gameObject.SetActive (false);
	}

//	void OnEnable(){
//		bank.Refresh();
//	}
	// Update is called once per frame
	void Update () {
		if (isOpening) {
			openingTimer -= Time.deltaTime;
			if (openingTimer < 0) {
				isOpening = false;
				openingTimer = 1;
			}
		}
	}
	public void Save(){
		Persistence.StorePurchases (storage);
	}
	public void Open () {
		//DeselectAll ();
		if (!shopAnim.GetBool ("IsOpen") && !isOpening) {
			isOpening = true;
			ClimberStateManager.SwitchState (ClimberState.SHOP);
			SelectCurrent (false);
			bank.Refresh ();
			panel.SetActive (true);
			curtainAnim.gameObject.SetActive (true);
			shopAnim.SetBool ("IsOpen", true);
			curtainAnim.SetBool ("IsOpen", true);
			//LeanTween.cancel (bank.gameObject);
			bank.StopAnimations ();
			bank.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 158);
			bank.DisableButton ();
		}
	}

	public void Close() {
		if (!isOpening) {
			_isClosing = true;
			shopAnim.SetBool ("IsOpen", false);
			curtainAnim.SetBool ("IsOpen", false);
			LeanTween.delayedCall (gameObject, 1f, () => {
				_isClosing = false;
				panel.SetActive (false);
				curtainAnim.gameObject.SetActive (false);
			});	
			main.TogglePause ();

			for (int i = 0; i < items.Count; i++) {
				if (items [i].selected && Persistence.GetPurchased (i)) {
					FindObjectOfType<CharacterScript> ().SpawnChar (i);
					Persistence.currentChar = i;
				}
			}
			bank.DisableButton ();
		}
	}

	public void SelectCurrent(bool anim = true) { 
		//DeselectAll();
		//for (int i = 0; i < items.Count; i++) {
		foreach (var item in items) {
			if (item.id == Persistence.currentChar){
				item.Select (anim);
				break;
			}
		}
	}
    
    
	public void DeselectAll(){
		for (int i = 0; i < items.Count; i++) {
			items [i].Deselect ();
		}
	}

	void SpawnItem (int id) {
		CharacterProfile newChar = storage.GetCharacter (id);
		if (newChar != null) {
			GameObject go = Instantiate (shopItem, contentRoot.transform);

			ShopItem item = go.GetComponent<ShopItem> ();
			item.SetProfile (newChar);
			UnityEngine.UI.Image coinImg = item.coinRoot.GetComponentInChildren<UnityEngine.UI.Image> ();
			if (coinImg != null && newChar.rewardBonus <= coinIcons.Length) {
				coinImg.sprite = coinIcons [newChar.rewardBonus];
			}
			else {
				Debug.LogError("Failed to set coin icons");
			}
			items.Add (item);
			go.transform.SetSiblingIndex (items.Count - 1);
		}
		else {
			Debug.LogWarning ("Could not spawn char, skipping");
		}
	}
	public bool isClosing{
		get{ return _isClosing;}
	}
}
