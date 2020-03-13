using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour {
	public GameObject theYellowThing;
	public GameObject theStar;
	public GameObject theShadow;
	public GameObject textObj;
	public GameObject burstPoof;
	public GameObject landingPoof;
	public int reward;
    public GameObject coin;
	//GameObject _star;
	//RectTransform _bankTransform;

	CameraController camControl;
	GameController gameControl;
	public BankController bank;
	XPController exp;
	DropManager drop;
	TaskUI ui;
	public float appearTime = 0.32f;

	float timer;
	bool isAnimating;
	bool inPlace;
	Overlay overlay;
	Vector3 screenCenter;

	public AudioClip initSound;

    TMPro.TextMeshPro[] texts;
	public Text uiText;
	SmallHelpButton shb;

	static GameObject reference;
	// Use this for initialization
	void Start () {
		ui = FindObjectOfType<TaskUI>();
		exp = FindObjectOfType<XPController>();
		drop = FindObjectOfType<DropManager>();
		shb = FindObjectOfType<SmallHelpButton> ();

		camControl = FindObjectOfType<CameraController>();
		gameControl = FindObjectOfType<GameController>();
		screenCenter =  camControl.OverlayToWorld(new Vector3(Screen.width/2, Screen.height/2, 10));
        
        texts = theStar.GetComponentsInChildren<TMPro.TextMeshPro>();

		reference = theYellowThing;

		theShadow.SetActive(false);
		theStar.SetActive(false);
		theYellowThing.SetActive(false);
		textObj.SetActive(false);

		string levelString = ContentData.getLabelText ("title_LevelUp");
		uiText.text = levelString.ToUpper();
	}
	
	// Update is called once per frame
	void Update () {
		if (ClimberStateManager.state == ClimberState.LEVELUP && inPlace && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter))){
			Burst();
		}
		if (isAnimating){
			timer -= Time.deltaTime;
			if (timer <0 ){
				isAnimating = false;
				OnCoinEnd();
			}
		}
	}

	public void Init(Vector3 pos){
//		if (shb != null) {
//			shb.Deactivate (); 
//			FindObjectOfType<HelpController> ().root.SetActive (false);
//		}

		if (initSound != null) {
			LeanAudio.play (initSound);
		}
		Overlay.instance.Purge();
		ClimberStateManager.SwitchState(ClimberState.LEVELUP);

		theStar.SetActive(true);
		theStar.transform.position = pos;
		theStar.transform.localScale = Vector3.one * 0.08f;
		theShadow.SetActive(true);
		theShadow.transform.localScale = Vector3.one * 0.08f;
		theShadow.transform.localPosition = new Vector3 (pos.x, screenCenter.y - 1.9f, pos.z);
		theYellowThing.SetActive(true);
		theYellowThing.transform.position = pos;
		theYellowThing.transform.localScale = Vector3.one * 0.3f;

		LeanTween.scale(theYellowThing, Vector3.one * 10, 0.3f);
		//Star animation
		LeanTween.moveLocalY(theStar, screenCenter.y + 2f, appearTime/2).setEase(LeanTweenType.easeOutSine).setOnComplete(()=>{
			LeanTween.moveLocalY(theStar, screenCenter.y, appearTime/2).setEase(LeanTweenType.easeInSine);
		});
		LeanTween.moveLocalX (theStar, screenCenter.x, appearTime).setEase(LeanTweenType.easeInCubic);
		LeanTween.moveLocalX (theShadow, screenCenter.x, appearTime).setEase(LeanTweenType.easeInCubic);
		LeanTween.scale (theStar, Vector3.one * 0.5f, appearTime);
		LeanTween.scale (theShadow, Vector3.one * 0.5f, appearTime).setEase(LeanTweenType.easeInCubic);
		LeanTween.delayedCall(appearTime+0.1f, ()=>{
			inPlace = true;
			textObj.SetActive(true);
			LeanTween.alpha(theShadow, 0.3f, 1f).setLoopPingPong().setEase(LeanTweenType.easeInOutSine);
			LeanTween.moveLocalY(theStar, screenCenter.y+0.7f, 1f).setEase(LeanTweenType.pingPong).setLoopPingPong().setEase(LeanTweenType.easeInOutSine);
			GameObject particle = Instantiate (landingPoof, theStar.transform.position + Vector3.up * -1.6f, burstPoof.transform.rotation);
			particle.transform.localScale = Vector3.one * 1.5f;
			//particle.transform.position = transform.position;
			Destroy (particle, 1f);
		});

		bank.MoveToCenter();
		ui.xpRoot.SetActive(false);

		SetLevel();


		FindObjectOfType<BombController>().explosion.gameObject.SetActive(false);

        
	}
    
    public void Burst(){
    
        textObj.SetActive(false);
        theStar.SetActive(false);
        theShadow.SetActive(false);
        
        Vector3 center = camControl.WorldToOverlay (screenCenter);
        reward = Mathf.Clamp (exp.GetLevel () * 10, 10, 100);
        
        GameObject oc = GameObject.Find ("OverlayCamera");
        Camera overlayCam;
        if (oc != null) {
            overlayCam = oc.GetComponent<Camera>();
        } else {
            overlayCam = Camera.main;
        }
        
        
        for (int i = 0; i < reward; i++) {
            Vector3 pek = (Vector3)Random.insideUnitCircle.normalized * Random.Range(40f, 190f);
            float t = 0.6f;
            
            GameObject coinObj = Instantiate(coin) as GameObject;
            coinObj.transform.SetParent(bank.transform.parent.transform);
            RectTransform rectTrans = coinObj.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector3(0, -Screen.height);
            coinObj.transform.localScale = new Vector3(0, 0, 0);
            
            
            Vector3 pos1 = center + Vector3.up * 7 + pek;
            float random = Random.Range(-0.1f, 0.1f);        
            
            Vector3 dest = overlayCam.ScreenToWorldPoint(new Vector3(pos1.x, pos1.y, 10));
            
            Vector2 tragetPos = bank.coinTargetPos;
            tragetPos.x = -180;
            
            LeanTween.scale (coinObj, Vector3.one * 0.8f, t / 3).setEase(LeanTweenType.easeOutCubic);
            
            LeanTween.move (coinObj, dest, (t / 2) + random).setEase(LeanTweenType.easeInCubic).setOnComplete(() =>{
                 LeanTween.value(coinObj, rectTrans.anchoredPosition, tragetPos, t).setEase(LeanTweenType.easeInCubic).setOnUpdate((Vector2 pos) => {
                
                    rectTrans.anchoredPosition = pos;
                
                });
                LeanTween.scale (coinObj, Vector3.one * 0.2f, t).setEase(LeanTweenType.easeInCubic).setOnComplete(()=>{
                    Destroy(coinObj);
                });
            
            });

        }
        
        int repeats = Mathf.Clamp (exp.GetLevel (), 1, 10);
        LeanTween.delayedCall (0.8f, () => {
            LeanTween.delayedCall (0.6f / repeats, ()=>{
                bank.StopAnimations(); 
                bank.Flash(); 
                bank.Ding();
                bank.Bloat();
            } ).setRepeat (repeats);
        });
        //SpawnPoof (0);
        GameObject particle = Instantiate (burstPoof, theStar.transform.position, burstPoof.transform.rotation);
        //particle.transform.position = transform.position;
        Destroy (particle, 1f);
        timer = 1f;
        isAnimating = true;
        inPlace = false;
        if (bank.coinExplodeSound != null) {
            LeanAudio.play (bank.coinExplodeSound);
        }
    }

/*
   public void Burst(){
        textObj.SetActive(false);
        theStar.SetActive(false);
        theShadow.SetActive(false);
        Vector3 center = camControl.WorldToOverlay (screenCenter);
        reward = Mathf.Clamp (exp.GetLevel () * 10, 10, 100);
        for (int i = 0; i < reward; i++) {
            Vector3 pek = (Vector3)Random.insideUnitCircle.normalized * Random.Range(40f, 190f);
            //Debug.Log (pek);
            GameObject o = Overlay.instance.CreateOverlayObj (drop.coin, center + pek/10);
            o.name += i;
            float t = 0.6f;
            
            Overlay.instance.MoveTo (o, center + Vector3.up*10 + pek, t/2, LeanTweenType.easeOutCubic, false, false, ()=>{
                Overlay.instance.MoveTo (o, bank.bankPos, t, LeanTweenType.easeInCubic, false, false, NullAction);
                LeanTween.scale (o, Vector3.one * 0.2f, t).setEase(LeanTweenType.easeInCubic).setOnComplete(()=>{
                    o.SetActive(false);
                });
            });

        }
        int repeats = Mathf.Clamp (exp.GetLevel (), 1, 10);
        LeanTween.delayedCall (0.8f, () => {
            LeanTween.delayedCall (0.6f / repeats, ()=>{
                bank.StopAnimations(); 
                bank.Flash(); 
                bank.Ding();
                bank.Bloat();
            } ).setRepeat (repeats);
        });
        //SpawnPoof (0);
        GameObject particle = Instantiate (burstPoof, theStar.transform.position, burstPoof.transform.rotation);
        //particle.transform.position = transform.position;
        Destroy (particle, 1f);
        timer = 1f;
        isAnimating = true;
        inPlace = false;
        if (bank.coinExplodeSound != null) {
            LeanAudio.play (bank.coinExplodeSound);
        }
    }

    */

	void NullAction(){

	}



	void OnCoinEnd(){
		bank.CheckIn(exp.GetLevel() * 100 * Mathf.FloorToInt(1 + exp.GetLevel() / 5));
		Overlay.instance.Purge ();
		
        //ui.darken.gameObject.SetActive (false);
		//ui.pauseRoot.SetActive(true);
		//ui.backButton.SetActive(false);
		//ui.optButton.SetActive (false);
		
        ui.LvlUpPause();
	}

	public void Disable(){
		theStar.SetActive(false);
		LeanTween.cancel (theStar);
		theShadow.SetActive(false);
		LeanTween.cancel (theShadow);
		SpriteRenderer ren = theShadow.GetComponent<SpriteRenderer> ();
		Color c = ren.color;
		c.a = 0.8f;
		ren.color = c;
		theYellowThing.SetActive(false);
		bank.MoveToSide();
		ui.xpRoot.SetActive(true);
		gameControl.ResetCam();
//		SmallHelpButton shb = FindObjectOfType<SmallHelpButton> ();
//		if (shb != null) {
//			
//			shb.Activate();
//		}
		FindObjectOfType<BombController>().explosion.gameObject.SetActive(true);
	}

	void SetLevel(){
		for (int i = 0; i < texts.Length; i++) {
			texts[i].text = (exp.GetLevel ())+"";
		}
	}

	public static bool isActive {
		get {return reference.activeSelf;}
	}
}
