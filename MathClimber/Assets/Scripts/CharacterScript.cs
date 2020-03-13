using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

//Place it on character prop
public class CharacterScript : MonoBehaviour, IPauseHandler {



	SpineAnimation anim; 
	Spine.Bone root;
	public GameObject rocket;

	[HideInInspector]
	public float jumpFlightTime;
	public AnimationCurve jumpCurve;

	[HideInInspector]
	public float fallFlightTime;
	public AnimationCurve fallCurve;


	Vector3 charOrigin;
	Vector3 stepUp;
	Vector3 stepDown;
	public float height = 4.61f;
	bool adjustHeight;
	int count;

	float timer;
	bool move;

	DropManager dropMgr;

	System.Action onEnd;

	bool hasPicked;
    public bool tapEnable = true;
    

	int tempJumps;
	bool isTemp;

	CharacterStorage charStorage;
	CharacterProfile curChar;

	AudioSource charVoice;


	// Use this for initialization
	void Start () {
		dropMgr = FindObjectOfType<DropManager> ();
		charStorage = FindObjectOfType<CharacterStorage> ();

		SpawnChar (Persistence.currentChar);
		//Init ();
		charOrigin = gameObject.transform.position;
        charOrigin.y += 1.25f;
		ClimberStateManager.Subscribe (this);

		stepUp = charOrigin + Vector3.forward * 1.6f/*725f*/ + Vector3.down * 0.765f;
        stepUp.y += 1.25f;
        
		stepDown = charOrigin + Vector3.back * 1.6f/*725f*/ + Vector3.up * 0.765f;
        stepDown.y -= 1.0f;
        
		rocket.SetActive(false);

	}
	

	public void Jump (Action callback) {
        
		ClimberStateManager.SwitchState (ClimberState.JUMPING);
		hasPicked = false;
		anim.Jump();
		height = Mathf.Abs (height);

		onEnd = callback;

		if (isTemp) {
			tempJumps++;
			if (tempJumps > 5) {
				isTemp = false;
				SpawnChar (Persistence.currentChar);
			}
		}
	}
    
    public void setTapEnable(bool enable) {
        tapEnable = enable;
    }
    
    
    public void forceIdle () {
        ClimberStateManager.SwitchState (ClimberState.IDLE);
        anim.resetIdle();
    }

	public void Fall(Action callback){
		ClimberStateManager.SwitchState (ClimberState.FALLING);
		hasPicked = false;
		anim.Fall ();
		height = - Mathf.Abs (height);

		onEnd = callback;

	}

	void HandleCustomEvent (Spine.TrackEntry trc, Spine.Event e) {
		
		if (e.Data.Name == "start" || e.Data.Name == "jump_start") {         
			move = true;
			if (height > 0) {
				timer = jumpFlightTime;
				if (curChar.jumpVoice != null && canTalk) {
					charVoice = LeanAudio.play (curChar.jumpVoice, 1f);
				}
			}
			else {
				timer = fallFlightTime;
				if (curChar.fallVoice != null && canTalk) {
					charVoice = LeanAudio.play (curChar.fallVoice, 1f);
				}
			}


			if (curChar.jumpSound != null) {
				charVoice = LeanAudio.play (curChar.jumpSound, 1f);
			}
		}
		if (e.Data.Name == "jump_end") {
			move = false;
			if (!hasPicked) {
				
				if (dropMgr.enabled) {
					//Debug.Log ("Character Shifts index");
					dropMgr.ShiftIndex (-(int)Mathf.Sign (height));
				}

				//Debug.Log ("Jump End " + Time.time);
				hasPicked = true;
				if (curChar.landSound != null) {
					charVoice = LeanAudio.play (charStorage.GetCharacter (Persistence.currentChar).landSound);
				}
			}
		}
		if (e.Data.Name == "particle" || e.Data.Name == "particles") {
			Debug.Log ("CALL FOR PARTICLES!");
			if (curChar.particle != null) {
				GameObject p = Instantiate (curChar.particle, transform.position + Vector3.up, curChar.particle.transform.rotation);
				p.transform.parent = FindObjectOfType<DropManager> ().topItemTransform;
				ParticleSystem sys = p.GetComponent<ParticleSystem> ();
				//sys.main.loop = false;
				sys.Play ();
				Destroy (p, sys.main.duration);

			}
		}
		if (e.Data.Name == "sound" || e.Data.Name == "sfx") {
			
		}
	}

	void OnJumpEnd(Spine.TrackEntry trc){
        
		if (trc.Animation.name == "tap"){
			trc.mixDuration = 0;
			anim.resetIdle ();
		} else if (trc.Animation.name != "idle"){
			trc.mixDuration = 0;
			adjustHeight = true;

			anim.resetIdle ();

			onEnd();
		} else if (trc.Animation.name == "idle") {
            if (ClimberStateManager._instance._curState.state != ClimberState.IDLE) {
                ClimberStateManager.SwitchState(ClimberState.IDLE);
            }
        }
	}
	void Update(){
	
        if (!ClimberStateManager.isPaused ){
			if (adjustHeight) {
                Vector3 pos = Vector3.up * height;
				transform.Translate (pos);
				adjustHeight = false;
			}

			if (move) {
				timer -= Time.deltaTime;
				float t = -1;
				if (height > 0) {
					t = 1 - timer / jumpFlightTime;
					Move (jumpCurve.Evaluate(t));
				}
				else {
					t = 1 - timer / fallFlightTime;
					Move (fallCurve.Evaluate(t));
				}
			}
		}
        
		if (Input.GetKeyDown (KeyCode.W)) {
			Switch ();
		}
        
        if (tapEnable) {
        
            if (Input.GetMouseButtonDown (0) && ClimberStateManager.state == ClimberState.IDLE) {

                Camera cam = GameObject.Find("StairCamera").GetComponent<Camera>();
                Vector3 touchPos = Utils.getTouchPos(Input.mousePosition, cam);
                
                
                RaycastHit2D [] hits = Physics2D.RaycastAll(touchPos, Vector3.forward, 100);

                foreach (RaycastHit2D hit in hits) {
                
                    CharacterScript tempCharacter = hit.collider.gameObject.GetComponent<CharacterScript>();
                    if (tempCharacter) {
                        anim.Tap ();
                        if (curChar.tapVoice != null && canTalk) {
                            charVoice = LeanAudio.play (curChar.tapVoice);
                        }
                    
                        FMC_Settings currentSetting = FMC_GameDataController.instance.getCurrentSettings();
                        if (currentSetting is FMC_Settings_StoryMode) {
                            SmallHelpButton smallButton = FindObjectOfType<SmallHelpButton> ();
                            if (smallButton) {
                                smallButton.Show(true);
                            }
                        }
                    }
                }
            }
        } else {
            Spine.TrackEntry trc = anim.skeleton.state.GetCurrent (0);
            string curAnim = trc.Animation.name;
            if (curAnim == "idle") {
                tapEnable = true;
               
            }
        }
	}


	public void Move(float t){
		if (height > 0) {
			transform.position = Vector3.Lerp (charOrigin, stepUp, t);
		}
		else {
			transform.position = Vector3.Lerp (charOrigin, stepDown, t);
		}
        setToFixedZPos();
	}

	public void UpdateReturn (float t) {
		
        if (!adjustHeight) {
			if (height > 0) {
                Vector3 pos = stepUp + transform.rotation * Vector3.up * height;
                pos.y += 0.25f;
				transform.position = Vector3.Lerp (pos, charOrigin, t);
			} else {
                Vector3 pos = stepDown + transform.rotation * Vector3.up * height;
                pos.y -= 0.55f;
				transform.position = Vector3.Lerp (pos, charOrigin, t);
			}
            setToFixedZPos();
		}
	}
    
    public void resetPosition (float t) {
        
        if (!adjustHeight) {
            if (height > 0) {
                Vector3 pos = stepUp + transform.rotation * Vector3.up * height;
                pos.y += 0.25f;
                transform.position = Vector3.Lerp (pos, charOrigin, t);
            } else {
                Vector3 pos = stepDown + transform.rotation * Vector3.up * height;
                pos.y -= 0.55f;
                transform.position = Vector3.Lerp (pos, charOrigin, t);
            }
            setToFixedZPos();
        }
    }
    
    private void setToFixedZPos () {
        Vector3 currentPos = transform.position;
        currentPos.z = -6.75f;
        transform.position = currentPos;
    }
	
	public void Switch(){
		count++;

		if (count >= charStorage.characters.Length) {
			count = 0;
		}
		SpawnChar (count);

		isTemp = true;
		tempJumps = 0;
	}
	public void SpawnChar(int id){
		Spine.TrackEntry trc = null;
		string curAnim = "idle";
		float normalTime = 0;
		bool initiated = anim != null;
		if (initiated){
			trc = anim.skeleton.state.GetCurrent (0);
			curAnim = trc.Animation.name;
			normalTime = trc.TrackTime / trc.Animation.duration;

			Destroy (anim.gameObject);
		}

		curChar = charStorage.GetCharacter(id);
		GameObject go = Instantiate (curChar.prefab, transform.position, curChar.prefab.transform.rotation);
		go.transform.parent = transform;
		Init (go);
		anim.skeleton.state.SetAnimation (0, curAnim, curAnim == "idle");

		if (initiated) {
			trc = anim.skeleton.state.GetCurrent (0);


			trc.TrackTime = normalTime * trc.Animation.duration;
			trc.mixDuration = 0;
		}
		root = anim.skeleton.skeleton.FindBone ("body");

		//Recolor!
		RecolorManager rc = RecolorManager.Get();
		if (rc != null) {
			rc.Recolor (id);
		}
	}

	public void ToggleChar (bool b) {
		anim.gameObject.SetActive(b);
		anim.resetIdle ();
	}

	void Init (GameObject inst){
		anim = inst.GetComponent<SpineAnimation> ();

		Spine.SkeletonData data = anim.skeleton.skeleton.data;

		Spine.Animation up = data.FindAnimation ("jump_up");

		Spine.Animation dn = data.FindAnimation ("jump_down");

		jumpFlightTime = GetJumpLength (up);
		fallFlightTime = GetJumpLength (dn);
        
        //Subscribe to events
		anim.skeleton.state.Event += HandleCustomEvent;
		anim.skeleton.state.Complete += OnJumpEnd;
	}

	void OnDestroy(){
		anim.skeleton.state.Event -= HandleCustomEvent;
		anim.skeleton.state.Complete -= OnJumpEnd;
	}
    
	float GetJumpLength(Spine.Animation anim){
		float begin = -1;
		float end = -1;
		for (int i = 0; i < anim.timelines.Count; i++) {
			//Check if it's an event timeline
			if (anim.timelines.Items [i] is Spine.EventTimeline) {
				Spine.EventTimeline ev = anim.timelines.Items [i] as Spine.EventTimeline; //casting Timeline to EventTimeline
				for (int j = 0; j < ev.Events.Length; j++) { //Read through events in the timeline
					float timing = ev.Events [j].Time;
					if (ev.Events [j].data.Name == "start" || ev.Events [j].data.Name == "jump_start") {
						begin = timing;
					}
					else if (ev.Events [j].data.Name == "end" || ev.Events [j].data.Name == "jump_end") {
						end = timing;
					}
				}
			}
		}
		return end - begin;
	}
    
	public bool canTalk {
		get {return charVoice == null;}
	}
    
	public void Pause(){
		anim.skeleton.timeScale = 0;
		LeanTween.pause(gameObject);
	}

	public void Unpause(){
		anim.skeleton.timeScale = anim.savedTimeScale;
		LeanTween.resume (gameObject);
	}

	public Spine.Bone rootBone{
		get {return root;}
	}

}
