using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler {

	public GameObject priceTag;
	public GameObject confirmButton;
	public GameObject playButton;
	public GameObject lockIcon;
	public GameObject portrait;
	public GameObject coinRoot;
	public Spine.Unity.SkeletonGraphic skeleton;

	public Image background;
	public Image outline;

	public Sprite coin;

	TMPro.TextMeshProUGUI[] texts;

	CharacterProfile profile;

	bool isActive;
	bool isPurchased;

	ShopScreen shop;

	AudioClip click;

	LTDescr scaleTween;
	bool isAnimating;
	//bool isClosing;

	// Use this for initialization
	void Awake () {
		texts = GetComponentsInChildren<TMPro.TextMeshProUGUI> ();
		shop = FindObjectOfType<ShopScreen> ();

		UpdateStatus();
	}
	void Start () {
		click = FindObjectOfType<InputManager> ().click;
	}

	public void Buy(){
		
        if (shop.bank.balance > profile.price) {
			shop.bank.CheckIn (-profile.price);
			SpawnCoins(false);
			confirmButton.SetActive(false);
			priceTag.SetActive (true);

			isAnimating = true;
		}
	}

	public void Play(){
		if (!isActive) {
			Select ();
		}
		else if (!shop.isClosing){
			if (click != null) {
				LeanAudio.play (click, 0.2f);
			}
			//Select ();
			shop.Close ();
			FindObjectOfType<CharacterScript> ().SpawnChar(profile.id);
			//if (newChar.isPurchased){
			Persistence.currentChar = id;
			LeanAudio.play (profile.specialVoice);
				//		}
			//shop.isClosing = true;
		}

	}

	public void Select(bool anim = true){
    
		if (click != null) {
			LeanAudio.play (click, 0.2f);
		}
		shop.DeselectAll ();
		isActive = true;
		if (scaleTween != null) {
			Debug.Log ("Tweening zoom 1" + LeanTween.isTweening (scaleTween.id) + " " + scaleTween.id);
		}
		if (!profile.isPurchased && profile.price > shop.bank.balance) {
			shop.bank.Shake ();

		}
		else {
            if (anim) {
			    skeleton.AnimationState.SetAnimation (0, "tap", false);
			    skeleton.AnimationState.AddAnimation (0, "idle", true, 0);
            }

		}
        if (anim) {
    		scaleTween = LeanTween.scale (gameObject, Vector3.one * 0.8f, 0.1f).setEase(LeanTweenType.easeOutCubic).setOnComplete (() => {
    			scaleTween = LeanTween.scale (gameObject, Vector3.one, 0.15f).setEase(LeanTweenType.easeInOutCubic);
    		});
        }

		UpdateStatus ();

	}

	public void Deselect(){
		isActive = false;
		UpdateStatus ();
	}

	public void OnPointerClick(PointerEventData pointer) {
    
		if (!isAnimating){
			if (!isActive && profile != null) {
				Select ();
				if (profile.isPurchased || profile.price <= shop.bank.balance) {
					if (profile.tapVoice != null) {
						LeanAudio.play (profile.tapVoice);
					}
				}
			}
			else {
				shop.SelectCurrent ();
			}
		}
	}

	void UpdateStatus(){
		if (profile != null){
			isPurchased = profile.isPurchased;

			if (isPurchased) {
				priceTag.SetActive (false);
				confirmButton.SetActive (false);
				playButton.SetActive (true);
				lockIcon.SetActive(false);
			}
			else if(profile.isLocked){
				priceTag.SetActive (false);
				confirmButton.SetActive (false);
				playButton.SetActive (false);
				lockIcon.SetActive(true);
				//SetPrice("N/A");
			}
			else if (isActive && profile.price < shop.bank.balance) {
				priceTag.SetActive (false);
				confirmButton.SetActive (true);
				playButton.SetActive (false);
				lockIcon.SetActive(false);
			}

			else {
				priceTag.SetActive (true);
				confirmButton.SetActive (false);
				playButton.SetActive (false);
				lockIcon.SetActive(false);
			}
		}
		else {
			priceTag.SetActive (true);
			confirmButton.SetActive (false);
			playButton.SetActive (false);
			SetPrice("N/A");
			isActive = false;
		}

		if (isActive) {
			outline.color = Color.white;
		}
		else {
			outline.color = Color.clear;
		}

	}

	public void SetPrice (string s) {
		for (int i = 0; i < texts.Length; i++) {
			texts [i].text = s + " <sprite=0>";
			if (i>0 && texts [i].transform.childCount > 0){
				texts [i].transform.GetChild (0).gameObject.SetActive (false);
			}
		}
	}

	public void SetProfile (CharacterProfile p) {
		portrait.transform.GetChild (0).gameObject.SetActive (false);
		profile = p;
		background.color = Color.Lerp(profile.primary, profile.secondary, 0.5f);

		if (profile.portrait != null) {
			Instantiate (profile.portrait, portrait.transform);
			skeleton = portrait.GetComponentInChildren<Spine.Unity.SkeletonGraphic> ();
			skeleton.timeScale = Random.Range (0.9f, 1.1f);
			//LeanTween.delayedCall (Random.Range(0, 0.5f), ()=>{skeleton.AnimationState.SetAnimation (0, "idle", true);});
		}

		SetPrice(profile.price.ToString("N0",new System.Globalization.CultureInfo("is-IS")));
		UpdateStatus ();
	}

//	void SpawnCoins(){
//		GameObject root = new GameObject("coinRoot");
//		root.transform.position = shop.bank.bankPosRaw;
//		root.transform.SetParent (transform.root);
//		int count = Mathf.FloorToInt(profile.price / 500);
//		for (int i = 0; i < count; i++) {
//			GameObject go = Overlay.instance.CreateUIObj(coin, shop.bank.bankPosRaw);
//			go.transform.position = shop.bank.bankPosRaw + (Vector3) Random.insideUnitCircle * 20;
//			go.transform.SetParent(root.transform);
//		}
//
//
//		Vector3 pos = priceTag.transform.position;
//		Debug.Log("Moving to "+ pos);
//
//		float t = .5f;
//		LeanTween.move(root, pos, t).setEase(LeanTweenType.easeInCubic);
//		Destroy(root, t + 0.01f);
//	}

	void SpawnCoins(bool re = true) {
    
		if (shop.bank.coinExplodeSound != null) {
			LeanAudio.play (shop.bank.coinExplodeSound);
		}
        
		GameObject root = new GameObject("coinRoot");


		root.transform.position = shop.bank.bankPosRaw;
		root.transform.SetParent (transform.root);
        
		int count = Mathf.FloorToInt(Mathf.Sqrt(profile.price)/3);
		float maxRadius = 190f;
		float midPoint = 150f;
		if ((float)Screen.width / Screen.height < 0.5f) {
			maxRadius *= 2;
			midPoint *= 2;
		}
		float t = .6f;
		for (int i = 0; i < count; i++) {
			Vector3 pek = InsideSemicircle().normalized * Random.Range(40f, maxRadius);
			//Debug.Log (pek);
			GameObject o = Overlay.instance.CreateUIObj (coin, shop.bank.bankPosRaw);
			o.name += i;

			o.transform.SetParent(root.transform);
            
           
     

			Vector3 pos = priceTag.transform.position;
           
            float scaleFactor = 0.55f;
            
            if (SystemInfo.deviceModel.Contains("iPad")) { 
                scaleFactor = 1.1f;
            }
            
			LeanTween.scale (o, Vector3.one * scaleFactor, t / 3).setEase(LeanTweenType.easeOutCubic);
			LeanTween.move(o, shop.bank.bankPosRaw + Vector3.down * midPoint + pek, t/2 + Random.Range(-0.1f, 0.1f)).setEase(LeanTweenType.easeOutCubic).setOnComplete(
				()=>{
					LeanTween.move(o, pos, t).setEase(LeanTweenType.easeInCubic);
					LeanTween.scale (o, Vector3.one * 0.2f, t).setEase(LeanTweenType.easeInCubic).setOnComplete(()=>{
						o.SetActive(false);
					});
				}
			);
           

		}
		//Adjust for iphoneX
		if ((float)Screen.width / Screen.height < 0.5f) {
			root.transform.localScale = Vector3.one * 3.33f;
		}
		Destroy(root, t * 1.5f + 0.1f);
		LeanTween.delayedCall (t * 1.5f + 0.1f, () => {
			isAnimating = false;
			profile.isPurchased = true;
			shop.Save ();
			UpdateStatus ();
			LeanAudio.play (shop.bank.checkinSound);
			scaleTween = LeanTween.scale (gameObject, Vector3.one * 1.1f, 0.08f).setEase(LeanTweenType.easeOutCubic).setOnComplete (() => {
				scaleTween = LeanTween.scale (gameObject, Vector3.one, 0.15f).setEase(LeanTweenType.easeInOutCubic);
			});
		});
	}

	Vector3 InsideSemicircle(){
		Vector3 result = (Vector3)Random.insideUnitCircle;
		if (result.y > 0){
			result = InsideSemicircle ();
		}
		return result;
	}

	void NullAction(){

	}

	public bool selected{
		get{ return isActive;}
	}
	public int id{
		get{ return profile.id; }
	}
}
