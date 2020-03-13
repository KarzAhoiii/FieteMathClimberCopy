using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour {
	public Canvas uiRoot;
	List<GameObject> sprites;
	List<float> timers;

	Camera overlayCam;

	public static Overlay instance;
	void Awake(){
		instance = this;
		sprites = new List<GameObject> ();
		timers = new List<float> ();
		GameObject oc = GameObject.Find ("OverlayCamera");
		if (oc != null) {
			overlayCam = oc.GetComponent<Camera>();
		}
		else {
			overlayCam = Camera.main;
		}
	}

	void Start () {
		
	}
	
	void Update () {
		if (!ClimberStateManager.isInitialized || !ClimberStateManager.isPaused){
			for (int i = 0; i < timers.Count; i++) {
				timers [i] -= Time.deltaTime;
				if (timers[i] < 0) {
					DestroySprite (sprites [i] as object);
				}
			}
		}
	}

	public GameObject CreateUIObj (Sprite spr, Vector3 pos) {
		GameObject go = CreateOverlayObj(spr, pos);
		SpriteRenderer ren = go.GetComponent<SpriteRenderer>();
		Image img = go.AddComponent<Image>();

		img.sprite = spr;
		img.preserveAspect = true;
		img.rectTransform.anchorMin = new Vector2 (0, 0);
		img.rectTransform.anchorMax = new Vector2 (0, 0);
		img.rectTransform.anchoredPosition = pos;
		img.SetNativeSize();
		img.rectTransform.localScale = Vector3.one * 0.033f;
		Destroy(ren);

		go.transform.parent = uiRoot.transform;

		return go;
	}
	public GameObject CreateOverlayObj(Sprite spr, Vector3 pos) {
    
		GameObject go = new GameObject ("Pickup Instance");
		go.name += System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + ":" + System.DateTime.Now.Millisecond ;
		go.layer = LayerMask.NameToLayer ("Overlay");

		SpriteRenderer img = go.AddComponent<SpriteRenderer> ();
		img.sprite = spr;
		img.sortingOrder = -1;

		img.transform.position = overlayCam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 10));
		img.transform.localScale = Vector3.one * 0.5f;
		//		Debug.Log ("Spawning overlay obj at "+pos);
		sprites.Add(go);
		timers.Add (0.5f);
		return go;
	}

	public void MoveTo(GameObject go, Vector3 to, float t, System.Action callback){
		MoveTo (go, to, t, LeanTweenType.easeInCubic, false, true, callback);
	}

	public void MoveTo(GameObject go, Vector3 to, float t, LeanTweenType movCurve, bool arch, bool scale, System.Action callback) {
		//Debug.Log("Moving "+ go.name);
        
		float random = Random.Range(-0.1f, 0.1f);        
	    Vector3 dest = overlayCam.ScreenToWorldPoint(new Vector3(to.x, to.y, 10));
        //Vector3 dest = GameObject.Find("BankCamera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(to.x, to.y, 10));

		if (arch){
			float climax = go.transform.position.y + 2 * Mathf.Sign(to.y - go.transform.position.y);
			LeanTween.moveY (go, climax, (t + random) / 2).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>{
				LeanTween.moveY (go, dest.y, (t + random) / 2).setEase(LeanTweenType.easeInCubic);
			});
            LeanTween.moveX (go, dest.x, t+random).setEase(movCurve);
		} else {
            LeanTween.move (go, dest, t+random).setEase(movCurve);
		}
		if (scale){
			LeanTween.scale (go, Vector2.one*0.8f, t * 0.1f).setEase (LeanTweenType.easeOutCubic).setOnComplete (() => {
				LeanTween.scale (go, Vector2.one * 0.35f, t * 0.9f).setEase (LeanTweenType.easeInCubic);
			});
		}
		LeanTween.delayedCall(go, t+random, callback);
		timers[sprites.IndexOf(go)] = t+random+0.1f;
	}

	public void Purge(){
		int cnt = sprites.Count;

		for (int i = 0; i < cnt; i++) {
			DestroySprite (sprites [0] as object);
		}
	}

	public void DestroySprite(object obj){

		GameObject go = obj as GameObject;


		timers.RemoveAt(sprites.IndexOf(go));
		sprites.Remove(go);

		Destroy (go);

	}

}
