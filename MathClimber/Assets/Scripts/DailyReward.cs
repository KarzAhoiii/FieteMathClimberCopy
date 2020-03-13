using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyReward : MonoBehaviour {
	public GameObject chest;
	public BankController bank;
	BankController otherBank;
	public GameObject[] stuffToHide;

	public Sprite coin;
	public XPController exp; 

	TMPro.TextMeshProUGUI[] texts;
	UnityEngine.UI.Text uiText;

	GameObject root;
	public GameObject poof;
	bool isAnimating;
	float timer;
    
	void Awake () {
     
		root = chest.transform.parent.gameObject;
		root.SetActive (false);
		FMC_GameDataController.grantReturnReward += Open;
	}
	
	void Start () {
		//bank = FindObjectOfType <BankController> ();
		BankController[] banks = FindObjectsOfType<BankController>();
		for (int i = 0; i < banks.Length; i++) {
			if (banks [i] != bank) {
				otherBank = banks [i];
			} 
		}
		exp = FindObjectOfType<XPController>();
		uiText = root.GetComponentInChildren<UnityEngine.UI.Text> ();

		texts = root.GetComponentsInChildren<TMPro.TextMeshProUGUI> ();
		uiText.text = ContentData.getLabelText ("title_Daily");
		for (int i = 0; i < texts.Length; i++) {
			texts [i].text = ContentData.getLabelText ("title_Daily");
		}
        
	}
	void OnDestroy(){
		FMC_GameDataController.grantReturnReward -= Open;
	}
	

	void Update () {
		if (root.activeSelf && !isAnimating && Input.GetMouseButtonDown(0)){
			Burst ();
		}
		if (isAnimating){
			timer -= Time.deltaTime;
			if (timer < 0) {
				isAnimating = false;
				OnCoinEnd ();
			}
		}
		if (Input.GetKeyDown (KeyCode.Q) || Input.touchCount >= 5){
			Open ();

		}
	}

	public void Open(){
		root.SetActive (true);
		chest.SetActive(true);
		for (int i = 0; i < stuffToHide.Length; i++) {
			stuffToHide [i].SetActive (false);
		}
	}

	public void Burst(){
		
		chest.SetActive(false);
		Vector3 center = new Vector3(Screen.width/2, Screen.height/2, root.transform.position.z);
		Vector3 targetPos = Camera.main.WorldToScreenPoint (bank.bankPos);
		int reward = 10;
		for (int i = 0; i < reward; i++) {
			Vector3 pek = (Vector3)Random.insideUnitCircle.normalized * Random.Range(10f, 190f);
			//Debug.Log (pek);
			GameObject o = Overlay.instance.CreateUIObj (coin, center);
			o.transform.localPosition = center - Vector3.back * center.z;
			o.transform.localScale = Vector3.one * 0.4f;  
			o.name += i;
			float t = 0.6f;
			LeanTween.scale (o, Vector3.one * 0.85f, t/3).setEase(LeanTweenType.easeOutCubic);
			Overlay.instance.MoveTo (o, center + Vector3.up*10 + pek, t/2, LeanTweenType.easeOutCubic, false, false, ()=>{
				//Overlay.instance.MoveTo (o, center + Vector3.up * 300f, t, LeanTweenType.easeInCubic, false, false, NullAction);
				Overlay.instance.MoveTo (o, targetPos, t, LeanTweenType.easeInCubic, false, false, NullAction);
				LeanTween.scale (o, Vector3.one * 0.2f, t).setEase(LeanTweenType.easeInCubic).setOnComplete(()=>{
					o.SetActive(false);
				});
			});

		}
		//int repeats = Mathf.Clamp (exp.GetLevel (), 1, 10);
		int repeats = 3;
		LeanTween.delayedCall (0.8f, () => {
			LeanTween.delayedCall (0.6f / repeats, ()=>{
				bank.StopAnimations(); 
				bank.Flash(); 
				bank.Ding();
				bank.Bloat();
			} ).setRepeat (repeats);

		});
		GameObject particle = Instantiate (poof, chest.transform.position, poof.transform.rotation);
		particle.transform.localScale = Vector3.one * 0.7f;
		Destroy (particle, 1f);
		timer = 1f;
		isAnimating = true;
//		inPlace = false;
		if (bank.coinExplodeSound != null) {
			LeanAudio.play (bank.coinExplodeSound);
		}
	}

	void NullAction(){
		
	}

	void OnCoinEnd(){
		bank.CheckIn((1 + exp.GetLevel()) * 100 * Mathf.FloorToInt(1 + exp.GetLevel() / 5));
		Overlay.instance.Purge ();
		LeanTween.delayedCall (1, () => {
			otherBank.Refresh();
			root.SetActive (false);	
		});
	}

}
