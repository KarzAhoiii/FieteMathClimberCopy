using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
	
    public float jumpHeight;


	public int maxSteps;
	//public int camSwitchSteps;

    public GameObject pauseBG;

	int direction;
	public List<int> queue;

	int successCount;
	int jumpCount;

	System.Action callback;

	public CharacterScript character;

	DropManager dropMgr;

	public CameraController camCtrl;

	public StairController stairCtrl;

	XPController exp;

	TaskUI ui;

	float _slideTime = 0.266f;
	float timer;
	public AnimationCurve moveAnimation;
    
    SmallHelpButton smallButton;


	bool adjustFlight;

	bool isPaused;

	void Awake(){
		Application.targetFrameRate = 30;
		Dependencies dep = new Dependencies ();
		dep.main = this;
		dep.normalBank = null;
		dep.shopBank = null;
		dep.levelUpScreen = FindObjectOfType<LevelUpScreen> ();
		dep.ui = FindObjectOfType<TaskUI> ();

		ClimberStateManager state = new ClimberStateManager ();
		ClimberStateManager.Init (dep);
		ClimberStateManager.SwitchState (ClimberState.IDLE);
	}


	void Start () {
		ui = FindObjectOfType<TaskUI>();
		dropMgr = FindObjectOfType<DropManager> ();
		character = FindObjectOfType<CharacterScript> ();
		camCtrl = FindObjectOfType<CameraController> ();
		stairCtrl = FindObjectOfType<StairController> ();
		exp = FindObjectOfType<XPController> ();

		queue = new List<int> ();
		ResetCam();

        smallButton = FindObjectOfType<SmallHelpButton> ();

	}
	

	void Update () {

		if (ClimberStateManager.state == ClimberState.MOVING && queue.Count > 0) {
			timer -= Time.deltaTime;
			float t = 1 - (timer / _slideTime);
            
			stairCtrl.UpdateStairs (direction, moveAnimation.Evaluate (t));
			character.UpdateReturn (moveAnimation.Evaluate (t));

			camCtrl.UpdateCam (moveAnimation.Evaluate (t) * direction);


			if (timer < 0) {
				OnEndMove ();
			}
		}

		if (ClimberStateManager.isFlying){
			if (queue.Count > 0) {
                if (smallButton && smallButton.isShowing) {
                    smallButton.Hide();
                }
            
				timer -= Time.deltaTime;
				float t = 1 - (timer / _slideTime);

				stairCtrl.UpdateStairs (direction, t);



				if (timer < 0) {
					adjustFlight = false;
                    
                    RemoveQueue();
					//queue.RemoveAt (0);
                    
					jumpCount += direction;

					dropMgr.ShiftIndex (-1);
					stairCtrl.SnapBack ();

					if (queue.Count == 0) {
						EndFlight ();
					}
					else {
						timer = _slideTime;
						//dropMgr.SpawnRandom (dropMgr.spawnPos, 7);
						dropMgr.SpawnRandom (7, 7);
					}
				}
			}
			else{
				EndFlight ();
			}
		}
//		else if (ClimberStateManager.state == ClimberState.SLIDING){
//			timer -= Time.deltaTime;
//
//			float t = timer / 1;
//
//
//			if (direction < 0){
//				t = 1 - (timer / 1);
//			}
//			camCtrl.UpdateCamRaw (t);
//			if (timer < 0.15f){
//				if (direction > 0) {
//
//						//Loop ();
//					camCtrl.progress = 0.15f;
//					camCtrl.shiftCount += 1;
//					callback ();
//				}
//				else if (direction <0){
//					camCtrl.progress = 0.85f;
//					camCtrl.shiftCount -= 1;
//					callback ();
//				}
//			}
//
//				
//		}

//		if (ClimberStateManager.state == ClimberState.PAUSE && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter))){
//			TogglePause ();
//		}
	}
    
    
    
    public void lockTap () {
        character.forceIdle();
        character.setTapEnable(false);
    }
    
    
    private void AddQueue (int num) {
       queue.Add(num);
    }
    
    private void RemoveQueue () {
        queue.RemoveAt (0);
       // print("RemoveQueue: "+queue.Count);
    }
		
	public void Jump (){

		successCount++;
        
        queue.Clear();
        AddQueue(1);

		if(queue.Count == 1){
			direction = 1;
			character.Jump (OnJumpEnd);
		}
	}

	public void Fall () {
        
		if (successCount > 0){
			successCount--;
            
            queue.Clear();
            AddQueue(-1);

			if(queue.Count == 1){
				direction = -1;
				character.Fall (OnJumpEnd);
			}
		}
	}

	public void Fly(int n){
		if (ClimberStateManager.state != ClimberState.IDLE) {
			timer = 0;

			camCtrl.IncProgress (direction);
            
            RemoveQueue();
			//queue.RemoveAt (0);
            
			jumpCount += direction;
			adjustFlight = true;
			ClimberStateManager.SwitchState(ClimberState.JUMPING);
			LeanTween.value (gameObject, UpdateShit, 0, 1, 0.5f).setEase(moveAnimation).setOnComplete( Fly );
				

		}
		else {
			Fly ();
		}


		LeanTween.delayedCall(character.rocket, 0.1f, () => {
			character.rocket.SetActive(true);
		});
		character.ToggleChar(false);

		for (int i = 0; i < n; i++) {
			successCount ++;
            
            AddQueue(1);
		}
	}

	void Fly() {
    
		if (adjustFlight) {
			adjustFlight = false;
			stairCtrl.SnapBack ();
		}
        
		ClimberStateManager.SwitchState(ClimberState.FLYING);

		timer = _slideTime;
		direction = 1;


	}
	void EndFlight(){
		Debug.Log ("FlightEnd!");

		if (ClimberStateManager.state != ClimberState.LEVELUP) {
			ClimberStateManager.SwitchState (ClimberState.IDLE);
		}
	
		character.rocket.SetActive (false);
		character.ToggleChar (true);

	}

	void UpdateShit(float t){
		stairCtrl.UpdateStairs (1, t);
		character.UpdateReturn (t);
	}


	void OnJumpEnd(){

		ClimberStateManager.SwitchState (ClimberState.MOVING);
		timer = _slideTime;

	}

	void OnEndMove(){
		
        RemoveQueue();
		//queue.RemoveAt (0);


		stairCtrl.SnapBack ();

		camCtrl.IncProgress (direction);

		jumpCount += direction;

		//CheckCameraShift (direction, Loop);
		Loop();

	}


	void Loop(){
		if (queue.Count > 0) {
			if (queue [0] > 0) {
				direction = 1;
				character.Jump (OnJumpEnd);
			}
			else {

				direction = -1;
				character.Fall (OnJumpEnd);
			}
		}
		else {
			ClimberStateManager.SwitchState (ClimberState.IDLE);
		}

	}


//	void CheckCameraShift(int dir, System.Action call){
//
//		if (dir > 0 && camCtrl.progress > 0.75f) {
//			
//			ClimberStateManager.SwitchState(ClimberState.SLIDING);
//
//			timer = camCtrl.progress;
//			callback = call;
//			Debug.Log ("GOIN UP");
//		}
//
//		else if (dir < 0 && camCtrl.progress < 0.15f) {
//			//Debug.Log ("this happens");
//
//			Debug.Log("Slide shadowfax");
//			ClimberStateManager.SwitchState(ClimberState.SLIDING);
//
//			timer = 1-camCtrl.progress;
//			callback = call;
//		}
//		else {
//			
//			call();
//		}
//
//	}

	public void ResetCam () {
		maxSteps = exp.GetExperienceToLevel();
		camCtrl.setIncrement(1f / maxSteps);
		float curProgress = exp.levelProgress;
		camCtrl.UpdateCamRaw (curProgress);
		camCtrl.progress = curProgress;

		if (ClimberStateManager.state == ClimberState.LEVELUP) {
			camCtrl.IncProgress (-direction);

			jumpCount = -1;
			successCount = queue.Count - 1;
		}
		else {
			jumpCount = exp.curThreshold - exp.GetExperienceToLevel() - exp.GetExperience();
			successCount = jumpCount;
		}
	}


	public void TogglePause () {
    

		if (!ClimberStateManager.isPaused) {
			ClimberStateManager.SwitchState (ClimberState.PAUSE);
            
            if (smallButton) {
                smallButton.Hide();
            }
           
            if (pauseBG) {
                pauseBG.SetActive(true);
            }

		} else {
			ClimberStateManager.SwitchState ();
            
            if (pauseBG) {
                pauseBG.SetActive(false);
            }
		}
		BankController[] banks = FindObjectsOfType<BankController>();
		foreach (var item in banks) {
			item.Refresh();
		}
	}

	public void MainMenu (){
    
		ClimberStateManager.SwitchState(ClimberState.RELOADING);
		ui.Unpause ();
		if (FLS_LoadingScreen.instance) {
			FLS_LoadingScreen.instance.loadScene("Menu01");
		} else {
			Debug.LogWarning("No Loading Screen instance available.");
			SceneManager.LoadScene("Menu01");
		}
	}

	public void Settings(){

		if (FMC_GameDataController.instance != null) {
			MainMenu ();
			LeanTween.delayedCall (1f, () => {
				if (FMC_GameDataController.instance.getCurrentPlayMode () == FMC_Settings_Controller.activeSetting.freestyle) {
                
					FMC_GameDataController.instance.menuController.openFreestyleSettings (0);
				} else if (FMC_GameDataController.instance.getCurrentPlayMode () == FMC_Settings_Controller.activeSetting.oneTimesOne) {
                
					FMC_GameDataController.instance.menuController.openOneTimesOneSettings (0);
				}  else if (FMC_GameDataController.instance.getCurrentPlayMode () == FMC_Settings_Controller.activeSetting.oneTimesOneBig) {
                
                    FMC_GameDataController.instance.menuController.openOneTimesOneSettingsBig (0);
                }
			});
		}
	}

	//============= DEBUG VALUES ==========
	public int count{
		get{ return successCount;}
	}
	public int count2 {
		get{ return jumpCount;}
	}
}
