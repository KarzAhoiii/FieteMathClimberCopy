using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpController : MonoBehaviour {

	public GameObject root;
	public RectTransform questionIcon;
	public RectTransform questionShadow;
	public RectTransform comicTail;
	public Text descrText;
	public Text confirmText;
	public Text rejectText;
    public Text easierText;
	bool isHarder;

	public AudioClip fwoosh;



	SmallHelpButton smallButton;
	RectTransform rootRect;
	public Transform player;
	CameraController camControl;
	float refHeight;
	Vector3 savedPos;
	int hideCount;
	public bool isShowing;
    bool activeOpen = false;
	float animationTime = 0.33f;

	// Use this for initialization
	void Start () {
   
		confirmText.text = ContentData.getLabelText ("title_Confirm");
		rejectText.text = ContentData.getLabelText ("title_Reject");
        easierText.text = ContentData.getLabelText ("title_Easier");

		rootRect = root.GetComponent<RectTransform>();
		camControl = FindObjectOfType<CameraController>();
		smallButton = FindObjectOfType<SmallHelpButton> ();
		refHeight = rootRect.rect.height;
		LeanTween.moveY(questionIcon, questionIcon.anchoredPosition.y + 100f, 2f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
		LeanTween.alpha(questionShadow, 0.1f, 2f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();

		savedPos = rootRect.anchoredPosition;//root.transform.position;
		Hide ();
		root.SetActive (false);
	}
	
	void Update () {
		if (isShowing) {
			if (root.activeSelf) {
				UpdateTailPos ();
				if (ClimberStateManager.state == ClimberState.LEVELUP || ClimberStateManager.state == ClimberState.FLYING) {
					root.SetActive (false);
				}
			}
			else {
				if (ClimberStateManager.state != ClimberState.LEVELUP && ClimberStateManager.state != ClimberState.FLYING) {
					root.SetActive (true);
				}
			}
			
		}

		if (Input.GetKeyDown (KeyCode.Q)) {
			if (isShowing) {
				Hide ();
			}
			else {
				Show(isHarder);
			}
		}
	}

	public void Show(bool harder) {
    
		if (!root.activeSelf) {
			if (fwoosh != null) {
				LeanAudio.play (fwoosh);
			}
            activeOpen = true;
			smallButton.Hide ();
			isShowing = true;
			isHarder = harder;
			root.SetActive (true);
            
		    descrText.text = ContentData.getLabelText ("title_MakeHarder");
			

			Vector3 startPos = GetAdjustedPlayerScreenPos ();
			root.transform.localScale = Vector3.one * 0.2f;
			comicTail.transform.localScale = Vector3.one * 5;


			LeanTween.value (root, MoveRoot, startPos, savedPos, animationTime*0.757f).setEase (LeanTweenType.easeOutExpo);
			LeanTween.scale (root, Vector3.one, animationTime).setEase (LeanTweenType.easeOutBack);
			LeanTween.scale (comicTail, Vector3.one, animationTime).setEase (LeanTweenType.easeOutCirc);
			UpdateTailPos ();

		}
	}
    
	void Hide(){
		if (root.activeSelf) {
  
			if (fwoosh != null && activeOpen) {
				LeanAudio.play (fwoosh);
			}
			Vector3 targetPos = GetAdjustedPlayerScreenPos () + Vector3.right * 32 + Vector3.down * 6;
			//LeanTween.move(rootRect, playerPos, 0.25f).setEase(LeanTweenType.easeInExpo);
			LeanTween.delayedCall (animationTime * 0.152f, () => {
				LeanTween.value (root, MoveRoot, savedPos, targetPos, animationTime * 0.454f).setEase (LeanTweenType.easeInExpo);	
			});

			LeanTween.scale (root, Vector3.one * 0.2f, animationTime * 0.606f).setEase (LeanTweenType.easeInBack);
			LeanTween.scale (comicTail, Vector3.one * 5, animationTime * 0.606f).setEase (LeanTweenType.easeInCirc).setOnComplete (() => {
				root.SetActive (false); /*smallButton.gameObject.SetActive (true);*/
			});
			//root.SetActive (false);
			//Debug.Break ();
			isShowing = false;
			hideCount++;
		}

	}
    
    public void autoHide(){
        isShowing = false;
        hideCount++;
    }

	Vector3 GetAdjustedPlayerScreenPos(){
		Vector3 playerPos = camControl.stairCamera.WorldToScreenPoint (player.position);
		playerPos.y = refHeight * (playerPos.y / Screen.height) + refHeight * 0.2f;
		//Vector3 targetPos = new Vector3(camControl.stairCamera.pixelWidth/2, playerPos.y);
		Vector3 adjustment = new Vector3(rootRect.anchorMin.x * Screen.width, rootRect.anchorMax.y * Screen.height, -playerPos.z);

		//Vector3 targetPos = new Vector3(Screen.width* 0.125f, playerPos.y);
		//return targetPos;
		return savedPos + playerPos - adjustment;
	}
	void MoveRoot(Vector3 vec){
		rootRect.anchoredPosition = vec;
	}

	void UpdateTailPos(){
		Vector2 pos = comicTail.anchoredPosition; 
		Vector3 playerPos = camControl.stairCamera.WorldToScreenPoint(player.position);
		float newY = refHeight * (playerPos.y / Screen.height) + refHeight * 0.2f;
		pos.y = Mathf.Clamp(newY, refHeight*0.2f, refHeight);
		comicTail.anchoredPosition = pos;
	}
    
	public void Confirm(bool harder) {
    
        if (FMC_GameDataController.instance != null) {
    		if (harder) {
    		    FMC_GameDataController.instance.makeStoryModeHarder ();
    		} else {
    			FMC_GameDataController.instance.makeStoryModeEasier ();
    		}
        }
        
        FMC_GameDataController.instance.createFirstTask ();  
        GameObject go = GameObject.Find ("diamonds");
        ParticleSystem part = go.GetComponent<ParticleSystem> ();
        part.Play ();
        
		Hide ();
	}
    

	public void Decline(){
		Hide ();
	}

	public bool fold{
		get{ return hideCount > 1; }
	}
}
