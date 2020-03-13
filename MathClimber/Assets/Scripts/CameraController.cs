using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A service provider for manipulating cameras

public class CameraController : MonoBehaviour {

	public Vector3 camStart;
	public Vector3 camEnd;

	private Camera stairCam;
	private Camera uiCam;
	private Camera overlayCam;



	private float increment; //how far camera moves each step
	private float _progress; //How far has camera moved already


	private int _shiftCount;
	//bool isShifting;
	//CameraController instance;
	void Awake(){
		Application.targetFrameRate = 60;
		stairCam = GameObject.Find ("StairCamera").GetComponent<Camera>();
		uiCam = GameObject.Find ("Main Camera").GetComponent<Camera>();
		overlayCam = GameObject.Find ("OverlayCamera").GetComponent<Camera>();

	}

	// Use this for initialization
	void Start () {
		//progress = 0.15f;
		//UpdateCam (0);
	}
	


	public void UpdateCam (float t){ 
		
		float f = _progress + t*increment;
		stairCam.transform.position = Vector3.Lerp (camStart, camEnd, f);
	}
	public void UpdateCamRaw (float t){ 
		stairCam.transform.position = Vector3.Lerp (camStart, camEnd, t);
	}

	public void IncProgress (int dir){
		_progress += increment*dir;
	}


	/// <summary>
	/// Shift the camera in specified direction.
	/// </summary>
	/// <param name="dir">positive or negative direction</param>
	/// <param name="t">Time to tween</param>
	/// <param name="callback">Callback to perform at end</param>
//	public void Shift(int dir, float t, System.Action callback) {
//		LTDescr shiftTween = null;
//		if (dir > 0) {
//			shiftTween = LeanTween.value (gameObject, UpdateCam, 0, -_progress, t).setEase (LeanTweenType.easeInOutCubic);
//		}
//		else {
//			shiftTween = LeanTween.value (gameObject, UpdateCam, 0, 0.8f, t).setEase (LeanTweenType.easeInOutCubic);
//		}
//
//		shiftTween.setOnComplete(()=>{
//			_progress = (dir>0) ? 0 : increment * 15; 
//			callback();
//		});
//		_shiftCount += dir;
//	}


	public void setIncrement(float inc){
		increment = inc;
	}

	public int shiftCount{
		get{ return _shiftCount;}
		set{ _shiftCount = value;}
	}
	public float progress{
		get{ return _progress;}
		set{ _progress = value;}
	}

	public Vector3 UIToOverlay(Vector3 uiCoords){
		return uiCoords + new Vector3(stairCam.pixelWidth, 0, 0);
	}
	public Vector3 OverlayToUI(Vector3 overlayCoords){
		return overlayCoords - new Vector3(stairCam.pixelWidth, 0, 0);
	}
	public Vector3 UIToWorld(Vector3 uiCoords){
		return uiCam.ScreenToWorldPoint (uiCoords);
	}
	public Vector3 WorldToUI(Vector3 uiCoord){
		return uiCam.WorldToScreenPoint(uiCoord);
	}
	public Vector3 OverlayToWorld(Vector3 screenCoord){
		return overlayCam.ScreenToWorldPoint(screenCoord);
	}

	public Vector3 WorldToOverlay(Vector3 worldCoord){
		return overlayCam.WorldToScreenPoint(worldCoord);
	}

	public Camera stairCamera {
		get { return stairCam;}
	}
	public Camera uiCamera{
		get{ return uiCam; }
	}
}

