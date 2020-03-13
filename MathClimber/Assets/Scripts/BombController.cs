using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour {

	public RectTransform positionRef;
	Button button;
	Vector2 pos;
	int _charges;
	public ParticleSystem explosion;
	InputManager input;
	public AudioClip sound;
	public AudioClip chargeSound;
	AudioSource src;

	public Image shadow;
	public Image buttonTop;

	public GameObject activeIcon;
	public GameObject inactiveIcon;

	public RectTransform toShake;

	public int capacity = 1000;
	TMPro.TextMeshProUGUI[] txts;

	bool onCd;
	float cooldown;
	//Vector3 shakePos;
	Vector2 anchorPos;
	RecolorManager rec;
	CameraController camControl;
	LTDescr shakeAnimX;
	LTDescr shakeAnimY;

	float scaleRatio = 1f;
	RecolorListener reclist;
	// Use this for initialization
	void Start () {
		rec = RecolorManager.Get();
		reclist = transform.parent.GetComponent<RecolorListener> ();
		txts = buttonTop.GetComponentsInChildren<TMPro.TextMeshProUGUI> ();
		src = gameObject.AddComponent<AudioSource> ();
		src.clip = sound;
		input = FindObjectOfType<InputManager> ();
		button = GetComponentInChildren<Button> ();

		camControl = FindObjectOfType<CameraController>();


		Vector3 targetOffset = new Vector3 (223, 234, 0);
		if ((float)Screen.width / Screen.height < 0.5f) {
			UnityEngine.UI.CanvasScaler cnv = positionRef.root.GetComponent<UnityEngine.UI.CanvasScaler> ();
			float canvasRatio = Mathf.Lerp (cnv.referenceResolution.x, cnv.referenceResolution.y, cnv.matchWidthOrHeight);
			float screenRatio = Mathf.Lerp (Screen.width, Screen.height, cnv.matchWidthOrHeight);
			scaleRatio = canvasRatio / screenRatio;
			//Debug.Log (Screen.width/Screen.height);
			targetOffset = new Vector3(150,110,0);
		}


		Vector3 b = buttonTop.rectTransform.TransformPoint(targetOffset);


		Vector2 screenPoint = camControl.WorldToUI(b); 
		pos = screenPoint * scaleRatio;


		Debug.LogWarning("Bomb pos is "+pos);

		anchorPos = toShake.anchoredPosition;

		_charges = Persistence.bombs;
		Toggle (_charges>0);

		UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
		if (onCd) {
			cooldown -= Time.deltaTime;
			if (cooldown < 0) {
				onCd = false;
				LeanTween.cancel (toShake.gameObject, shakeAnimY.id, false);
				LeanTween.cancel (toShake.gameObject, shakeAnimX.id, false);

				toShake.anchoredPosition = anchorPos;

			}
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			Charge ();
		}
	}

	public void Charge(){
		if (chargeSound != null) {
			LeanAudio.play (chargeSound, 0.5f);
		}
		if (_charges < capacity) {
			_charges++;
			Persistence.bombs = _charges;
		}

		Toggle (true);

		UpdateUI ();

	}

	public void Activate(){
		if (!ClimberStateManager.isPaused && !ClimberStateManager.isFlying && cooldown <= 0 ){
			_charges--;
			Persistence.bombs = _charges;
			explosion.Play (true);
			src.Play ();
			FindObjectOfType <TaskUI>().ExplodeTask();

			Toggle (_charges > 0);
			UpdateUI ();
			input.AutoSuccess ();

			Shake();
			cooldown = 1;
			onCd = true;
		}
	}

	void Toggle(bool b){
		button.interactable = b;

		reclist.enabled = b;
		activeIcon.SetActive(b);
		inactiveIcon.SetActive (!b);

		if (b) {
			shadow.color = rec.mainColor;
			buttonTop.color = rec.mainColor;
		}
		else {
			shadow.color = new Color (0.83f, 0.83f, 0.83f);
			buttonTop.color = new Color (0.83f, 0.83f, 0.83f);
		}
			
	}

	void UpdateUI(){
		for (int i = 0; i < txts.Length; i++) {
			if (_charges > 0) {
				txts [i].text = string.Empty + _charges;
			}
			else {
				txts [i].text = string.Empty;
			}
		}
	}

	public Vector3 position{
		get{ return pos; }
	}

	public int charges{
		get{ return _charges; }
	}

	void Shake(){
		
		shakeAnimX = LeanTween.moveX (toShake, anchorPos.x - 100f, 0.2f+Random.Range(0, 0.2f)).setEase (LeanTweenType.easeShake);
		shakeAnimY = LeanTween.moveY (toShake, anchorPos.y-200, 0.15f+Random.Range(0, 0.1f)).setEase(LeanTweenType.easeShake).setOnComplete (
			() => {
				shakeAnimY = LeanTween.moveY (toShake, anchorPos.y+100f, 0.1f).setEase(LeanTweenType.easeShake).setOnComplete (
					() => {
						shakeAnimY = LeanTween.move (toShake, anchorPos, 0.1f).setEase(LeanTweenType.easeOutSine);
					});
			});
	}
}
