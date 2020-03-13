using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : MonoBehaviour {


	public TextMeshProUGUI task;
	public TextMeshProUGUI solution;
	public TextMeshProUGUI phantom;


	public Image success;
	public Image fail;
	public Image progress;



	//Pause screen
	public Image darken;
	public GameObject pauseRoot;
	public GameObject backButton;
	public GameObject optButton;
	public RectTransform shopButton;

	public GameObject xpRoot;

	public AnimationCurve curve;
	Vector3 savedpos; //Phantom image saved position
	float timer;
	float animTime = 1.5f;
	bool isAnimating;
	int direction;

	CameraController camControl;
	XPPanel expPanel;

	void Awake(){
		
		expPanel = FindObjectOfType<XPPanel>();
	}
	// Use this for initialization
	void Start () {
		camControl = FindObjectOfType<CameraController> ();
		if (phantom != null) {
			savedpos = phantom.transform.localPosition;
		}

		if (success != null) {
			success.enabled = false;
		}
		if (fail != null) {
			fail.enabled = false;
		}
		if (darken != null) {
			darken.enabled = false;
		}
		if (backButton != null) {
			backButton.SetActive (false);
		}
		if (pauseRoot != null){
			pauseRoot.SetActive (false);
		}
		if (optButton != null){
			optButton.SetActive (false);
		}


        if (success)
		    success.transform.GetChild (0).gameObject.SetActive (false);

		if (ClimberStateManager.isInitialized && !ClimberStateManager.inTestMode && !ClimberStateManager.inCampaign) {
            if (xpRoot != null) {
			    xpRoot.SetActive (false);
            }
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimating && !ClimberStateManager.isPaused) {
			timer += Time.deltaTime;


			UpdatePahntom (timer);

			if (timer >= animTime) {
				isAnimating = false;
				success.transform.GetChild (0).gameObject.SetActive (false);
			}
		}

		if (pauseRoot != null && pauseRoot.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))){
			FindObjectOfType<GameController>().TogglePause ();
		
			
		}
	}

	void UpdatePahntom(float t){
		float tt = curve.Evaluate( timer / (animTime*0.3f));

		if (direction < 0){
			
			phantom.transform.localPosition = Vector3.Lerp (savedpos, savedpos + Vector3.right * 454 * direction, tt);
			phantom.rectTransform.localScale = Vector3.Lerp (Vector3.one*2, Vector3.one * 0.65f, tt);
		}
		if (t> (animTime*0.8f)){
			float col_t = (t - animTime * 0.8f)/ (animTime * 0.2f);
			if (direction < 0){
				phantom.color = Color.Lerp (Color.black, Color.clear, col_t);
			}
			success.color = Color.Lerp (Color.white, Color.clear, col_t);
			fail.color = Color.Lerp (Color.white, Color.clear, col_t);
		}
	}

	public void SetSolution(int sol){
		solution.text = "" + sol;
	}
	public void SetSolution(string str){
		solution.text = str;
	}

	public void SetTask(string s){
		task.text = s;
	}

	public void Success(){
		
		success.enabled = true;
		success.transform.GetChild (0).gameObject.SetActive (true);
		fail.enabled = false;

		MovePhantom (1);
	}

	public void Fail(){
		success.enabled = false;
		fail.enabled = true;
		phantom.text = solution.text;
		MovePhantom (-1);
	}

	void MovePhantom(int dir){
		if (dir < 0) {
			phantom.color = Color.black;
		}
		else {
			phantom.color = Color.clear;
		}
		success.color = Color.white;
		fail.color = Color.white;

		isAnimating = true;
		timer = 0;
		direction = dir;
	}

	void ResetPhantom(){
		
	}


	public void SetXp(int xp, int limit){

		expPanel.SetXp(xp, limit);
	}
	public void SetLevel(int l){

		expPanel.SetLevel(l);
	}

	public void SetTimer(float time, float max){
		if (progress != null) {
			progress.fillAmount = 1 - time / max;
		}
	}

	public void Pause(){
		darken.enabled = true;
		xpRoot.transform.parent.gameObject.SetActive (false);
		pauseRoot.SetActive (true);		
		backButton.SetActive (true);
		if (ClimberStateManager.inTraining) {
			optButton.SetActive (true);
			float newX = optButton.GetComponent<RectTransform> ().anchoredPosition.x;
			AdjustShopButtonPos(-(int)newX);

		}
		else {
			optButton.SetActive (false);
			AdjustShopButtonPos (0);
		}
	}

	public void Unpause(){
		xpRoot.transform.parent.gameObject.SetActive (true);
		darken.enabled = false;
		pauseRoot.SetActive (false);
		optButton.SetActive (false);
	}

	public void LvlUpPause(){
		darken.enabled = false;
		pauseRoot.SetActive (true);
		optButton.SetActive (false);
		backButton.SetActive (false);
		AdjustShopButtonPos (0);
	}


	void AdjustShopButtonPos(int xPos){
		Vector2 vec = shopButton.anchoredPosition;
		vec.x = xPos;
		shopButton.anchoredPosition = vec;
	}
	public void ExplodeTask(){
		string[] parts = task.text.Split (' ');
		task.text = string.Empty;
		for (int i = 0; i < parts.Length; i++) {
			Vector3 meh = camControl.WorldToUI (task.transform.position-Vector3.right*0.5f + Vector3.right*i);
			Vector3 pos = Vector3.zero;
			RectTransformUtility.ScreenPointToWorldPointInRectangle (task.rectTransform, meh, camControl.uiCamera, out pos);
			GameObject go = NewText (parts [i], pos, task);
			Rigidbody body = go.AddComponent<Rigidbody> ();
			body.AddExplosionForce (50, task.transform.position, 5);
			Vector3 torque = new Vector3 (0, 0, -(pos.x - task.transform.position.x) * 100);
			body.AddTorque (torque);
			Destroy (go, 1.5f);
		}
	}

	GameObject NewText(string txt, Vector3 pos, TMPro.TMP_Text tmp){
		GameObject textcontainer = new GameObject ("textcontainer");
		textcontainer.layer = LayerMask.NameToLayer ("UI");
		textcontainer.transform.position = pos;


		textcontainer.transform.parent = task.transform.parent;
		textcontainer.transform.localScale = Vector3.one * 1f;

		textcontainer.AddComponent<MeshRenderer>();

		TMPro.TextMeshProUGUI newtxt = textcontainer.AddComponent<TMPro.TextMeshProUGUI> ();
		newtxt.font = tmp.font;
		newtxt.fontSize = tmp.fontSize;
		newtxt.alignment = tmp.alignment;
		newtxt.color = Color.black;
		newtxt.enableWordWrapping = false;
		newtxt.text = txt;

		return textcontainer;
	}
}
