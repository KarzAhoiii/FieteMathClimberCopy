using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankController : MonoBehaviour {
	//public Vector2 bankPos;
	int stored;
	int money;
//	List<GameObject> coins;
	public GameObject bankRoot;
	public GameObject coinIcon;
    public RectTransform coinTargetRect;
    public Vector2 coinTargetPos;
	public Image shine;
    public bool isMenuScene = false;
    

	//float countTime;
	//Text txt;
	TMPro.TextMeshProUGUI[] txt;


	public Vector3 coinPos;
	Vector3 savedPos;
	Vector3 centerPos;
	bool isMoving;
	bool isCentered;
	float time = 0.3f;
	float timer;

	bool isCounting;
	float countTime = 0.75f;
	float countTimer;

	CameraController camControl;


	Button button;

	LTDescr shakeTween;
	LTDescr shineTween;
	AudioSource src;
	public AudioClip checkinSound;
	public AudioClip countSound;
	public AudioClip coinExplodeSound;
	public AudioClip notEnoughSound;
    
    public Camera bankCamera;
    
    private Rect defaultViewPortRect;
    private Rect centerViewPortRect;

	float disableTimer;

	//LTDescr shakeAnim;
	// Use this for initialization
	void Start () {
		
		txt = GetComponentsInChildren<TMPro.TextMeshProUGUI> ();

		coinPos = coinIcon.transform.localPosition;
        
        setCoinTargetPosition();
        
        
        
		savedPos = bankRoot.transform.position;
		centerPos = new Vector3(0, savedPos.y, savedPos.z);

		Refresh();

		camControl = FindObjectOfType<CameraController> ();

		button = GetComponentInChildren<Button> ();

		src = gameObject.AddComponent<AudioSource>();
		src.playOnAwake = false;
		src.loop = true;
		src.volume = 0.8f;
		src.clip = countSound;
        
		if (!isMenuScene && ClimberStateManager.isInitialized && !ClimberStateManager.inTestMode && !ClimberStateManager.inCampaign) {
			RectTransform rt = bankRoot.GetComponent<RectTransform>();
			Vector2 pos = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2 (0, pos.y);
		}
        
        if (bankCamera) {
            defaultViewPortRect = bankCamera.rect;
            centerViewPortRect = defaultViewPortRect;
            centerViewPortRect.x = 0;
            centerViewPortRect.width = 1;
        }
        
	}
    
    public void setCoinTargetPosition () {
        if (coinTargetRect) {
            coinTargetPos = bankRoot.GetComponent<RectTransform>().anchoredPosition;
            coinTargetPos.x = 0;
        }
    }
    
    
	void OnEnable(){
		if (src != null) {
			Refresh ();
		}
	}

	public void Refresh () {
		money = Persistence.money;
		stored = money;
		ShowCoins (money);
	}

	void Update () {
		if (isMoving) {
			timer += Time.deltaTime;

			if (isCentered) {
				bankRoot.transform.position = Vector3.Lerp (savedPos, centerPos, timer / time);
                bankCamera.rect = centerViewPortRect;
			} else {
				bankRoot.transform.position = Vector3.Lerp (centerPos, savedPos, timer / time);
                bankCamera.rect = defaultViewPortRect;
			}
			if (timer > time) {
				isMoving = false;
			}
		}
		if (isCounting) {
			countTimer += Time.deltaTime;
			int count =  Mathf.RoundToInt(Mathf.Lerp(stored, money, countTimer/countTime));
			ShowCoins(count);

			if (countTimer > countTime) {
				isCounting = false;
				stored = money;
				src.Stop ();
				SetShineAlpha (0);
			}

		}
		if (disableTimer > 0) {
			disableTimer -= Time.deltaTime;
			if (disableTimer < 0) {
				EnableButton ();
			}
		}
	}
		

	public void CheckIn (int amount) {

		if (isCounting) {
			stored += Mathf.RoundToInt((money - stored) * countTimer / countTime);
		}
		money += amount;
		Persistence.money = money;
		countTimer = 0;
		isCounting = true;
	
//		SetShineAlpha(1);
//		shineTween = LeanTween.value (gameObject, SetShineAlpha, 1, 0, 0.5f);
		Flash();
		Bloat();
		if (amount > 0 && checkinSound != null) {
			LeanAudio.play (checkinSound, 0.8f);
		}

		src.Play();


	}
	public void Ding(){
		if (checkinSound != null) {
			LeanAudio.play (checkinSound, 0.8f);
		}
	}
	public void Flash(){
		SetShineAlpha(1);
		shineTween = LeanTween.value (gameObject, SetShineAlpha, 1, 0, 0.5f);

	}
	void SetShineAlpha (float a) {
		Color c = shine.color;
		c.a = a;
		shine.color = c;
	}

	public void StopAnimations(){
		if (shakeTween != null) {
			LeanTween.cancel (gameObject, shakeTween.id, false);
			transform.localScale = Vector3.one;
		}
		if (shineTween != null) {
			LeanTween.cancel (gameObject, shineTween.id, false);
			SetShineAlpha (0);
		}
	}


	public void Bloat(){
		if (shakeTween != null) {
			LeanTween.cancel (gameObject, shakeTween.id, false);
		}
		transform.localScale = Vector3.one * 1.2f;
		shakeTween = LeanTween.scale (gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeInCubic);
	}

	void ShowCoins (int c) {

		int len = txt [0].text.Length;
	
		if (c >= 1000000) {
			coinIcon.SetActive (false);
			for (int i = 0; i < txt.Length; i++) {
				txt [i].rectTransform.offsetMax = new Vector2 (0, txt [i].rectTransform.offsetMax.y);
			}
			//Debug.Log ("Shifting Text pos +");
		}
		else if (c >= 100000) {
			coinIcon.SetActive (true);
			coinIcon.transform.localPosition = coinPos + Vector3.right * 14;
			for (int i = 0; i < txt.Length; i++) {
				txt [i].rectTransform.offsetMax = new Vector2 (-55, txt [i].rectTransform.offsetMax.y);
			}
			//Debug.Log ("Shifting coin pos +");
		}
		else {
			coinIcon.SetActive (true);
			coinIcon.transform.localPosition = coinPos;
			for (int i = 0; i < txt.Length; i++) {
				txt [i].rectTransform.offsetMax = new Vector2 (-55, txt [i].rectTransform.offsetMax.y);
			}
			//Debug.Log ("Shifting coin pos -");
		}
			

		for (int i = 0; i < txt.Length; i++) {
			txt [i].text = c.ToString ("N0", new System.Globalization.CultureInfo ("is-IS"));
		}
	}

	public void Shake(){
		if (notEnoughSound != null) {
			LeanAudio.play (notEnoughSound);
		}
		
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        
        LeanTween.value(gameObject, 0, -30, 0.25f).setEase(LeanTweenType.easeShake).setOnUpdate((float val) => {
            rectTransform.anchoredPosition = new Vector2(val, rectTransform.anchoredPosition.y);
        }).setOnComplete (() => {
            LeanTween.value(gameObject, 0, 15, 0.2f).setEase(LeanTweenType.easeShake).setOnUpdate((float val) => {
                rectTransform.anchoredPosition = new Vector2(val, rectTransform.anchoredPosition.y);
            }).setOnComplete (() => {
                LeanTween.value(gameObject, 0, 0, 0.1f).setEase(LeanTweenType.easeOutSine).setOnUpdate((float val) => {
                    rectTransform.anchoredPosition = new Vector2(val, rectTransform.anchoredPosition.y);
                });
            });
        });	
	}

	public void MoveToCenter(){
		if (!isCentered){
			isMoving = true;
			isCentered = true;
			timer = 0;
			button.interactable = false;
		}
	}

	public void MoveToSide(){
		if (isCentered){
			isMoving = true;
			isCentered = false;
			timer = 0;
			button.interactable = true;
		}
	}

	public void DisableButton(){
		button.interactable = false;
		disableTimer = 1;
	}
	public void EnableButton (){
		button.interactable = true;
	}

	public Vector3 bankPos{
		get{ 
			//return camControl.WorldToOverlay(txt[0].transform.position);
			return bankPosRaw;
		}
	}
	public Vector3 bankPosRaw{
		get{ 
			return txt[0].transform.position;
		}
	}

	public int balance{
		get{ return money;}
	}

	public void Wipe(){
		
		Persistence.Clear();
		FindObjectOfType<CharacterStorage>().Wipe();
		FindObjectOfType<XPController> ().Load();
		FindObjectOfType<XPController> ().Refresh();
		FindObjectOfType<GameController> ().ResetCam ();
		Refresh();
	}
}
