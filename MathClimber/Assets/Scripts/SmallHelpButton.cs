using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHelpButton : MonoBehaviour {

	public Transform target;
	public Vector2 offset;
	public Camera cam;
	public GameObject root;
    
	CharacterScript character;
	CameraController camControl;
	RectTransform rt;
	float scaleRatio;
	HelpController help;
	bool isHarder;
	public bool isShowing;

    
	void Start () {
    
		help = FindObjectOfType<HelpController> ();
		rt = root.GetComponent<RectTransform> ();
		UnityEngine.UI.CanvasScaler cnv = rt.root.GetComponent<UnityEngine.UI.CanvasScaler> ();
		float canvasRatio = Mathf.Lerp (cnv.referenceResolution.x, cnv.referenceResolution.y, cnv.matchWidthOrHeight);
		float screenRatio = Mathf.Lerp (Screen.width, Screen.height, cnv.matchWidthOrHeight);
		scaleRatio = canvasRatio / screenRatio;

		character = target.GetComponent<CharacterScript>();
        
        if (root) {
		    root.SetActive (false);
        }
	}
	

	void Update () {
		if (isShowing) {
			if (root.activeSelf) {
				rt.anchoredPosition = GetTargetScreenPos () + (Vector3)offset;
				if (ClimberStateManager.state == ClimberState.LEVELUP || ClimberStateManager.state == ClimberState.RELOADING) {
					root.SetActive (false);
				}
			}
			else {
				if (ClimberStateManager.state != ClimberState.LEVELUP && ClimberStateManager.state != ClimberState.RELOADING) {
					root.SetActive (true);
				}
			}
		}

	}

	public void Show (bool harder) {
    
        if (!help.isShowing) {
    
            LeanTween.cancel(gameObject);
    		isHarder = harder;
    		isShowing = true;
    		root.SetActive (true);
            LeanTween.delayedCall(gameObject, 4, Hide);
        }
	}

	public void Hide(){
		isShowing = false;
		root.SetActive (false);
        help.autoHide();
	}


	Vector3 GetTargetScreenPos(){
		Spine.Bone bone = character.rootBone;
		Vector3 playerPos = cam.WorldToScreenPoint (target.TransformPoint(new Vector3(bone.worldX, bone.worldY, 0)));
		return playerPos * scaleRatio;
	}

	public void OnPress() {
		help.Show (isHarder);
		Hide();
	}
    
	public bool showing{
		get{ return isShowing;}
	}
}
