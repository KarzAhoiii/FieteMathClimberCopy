using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IphoneXUIAdjuster : MonoBehaviour {


	public Vector3 adjustedPos;
	public Vector3 adjustedScale = Vector3.one;
	public bool activate;
	private Vector3 curPos;
	RectTransform rt;
	float updateTimer;
	float ratio;
	// Use this for initialization
	void Awake() {
		ratio = (float)Screen.width / Screen.height;
		rt = GetComponent<RectTransform> ();
		if (ratio < 0.5f && activate){
			Reposition ();

		}
	}
	void Start () {
		

		if (ratio < 0.5f && activate){
			Reposition ();

		}
	}

//	void Update(){
//		if (updateTimer >= 0) {
//			updateTimer -= Time.deltaTime;
//			if (updateTimer < 0) {
//				updateTimer = 1;
//				if (rt != null) {
//					curPos = rt.anchoredPosition;
//				}
//				else {
//					curPos = transform.position;
//
//				}
//			}
//		}
//	}

	void Reposition(){
		if (rt != null) {
			if (rt.offsetMin == Vector2.zero && rt.offsetMax == Vector2.zero) {
				rt.offsetMax = adjustedPos;
			}
			else {
				rt.anchoredPosition = adjustedPos;
				rt.localScale = adjustedScale;
			}

		}
		else {
			transform.localPosition = adjustedPos;
			transform.localScale = adjustedScale;
		}
	}
}
