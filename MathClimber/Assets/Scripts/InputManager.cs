using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour {


	public TaskUI ui;

	FMC_Task curTask;
	public float timeLimit;

	public AudioClip click;
	public AudioClip success;
	public AudioClip fail;

	float timer;
	float inputTimer;
	int result;
	int digits;

	int timeouts;

	float waitTime = 1.1f;
	float animTimer;
	bool isAnimating;
	bool statistics;

	float failsafeTimer;

	//FMC_TaskCreation taskCreator;
	FMC_GameDataController gameData;
	GameController main;
    private DateTime timeWhenTaskWasCreated;

   
	// Use this for initialization
	void Start () {
		

		ui = FindObjectOfType<TaskUI>();

		gameData = FindObjectOfType<FMC_GameDataController> ();
		main = FindObjectOfType<GameController> ();


		FMC_TaskCreation.newTaskCreated += NewTask;

		if(gameData != null){
			gameData.createFirstTask();
		}
		else{
			Debug.LogError("Achtung! There's no GameData object");
		}

		ui.SetSolution ("");

	}

	void OnDestroy(){
		FMC_TaskCreation.newTaskCreated -= NewTask;
	}
	// Update is called once per frame
	void Update () {
		if (!isAnimating && timeLimit > 0 && main.count2 != 0 && ClimberStateManager.state == ClimberState.IDLE){//!ClimberStateManager.isPaused && !(timer >= timeLimit*0.8f && ClimberStateManager.state != ClimberState.IDLE)){
			
			timer += Time.deltaTime;
			ui.SetTimer (timer, timeLimit);
			if (timer >= timeLimit){
				//SendResult();
				Debug.LogWarning("Timed Out!");
				timeouts ++;
				AutoFail();
			}
		}

		if (!isAnimating && result != 0) {
			failsafeTimer -= Time.deltaTime;
			if (failsafeTimer < 0) {
				
				TrySubmit ();
			}
		}

		if (isAnimating){
			animTimer -= Time.deltaTime;
			if (animTimer < 0) {
				SendResult ();
			}
		}

		Keyboard ();
	}

	public void Enter (int num) {
        
       
        if (main.character.tapEnable && ClimberStateManager.state != ClimberState.FLYING) {
            main.lockTap();
    		if (!isAnimating){
    			LeanAudio.play(click, 1f);
    			
    			if (num < 0) {// erase last number / Backspace
    				result = (result - result % 10) / 10;
    			}
    			else {
    				result = result * 10 + num;
    			}


    			ui.SetSolution (result);
    			TrySubmit ();
                
    			failsafeTimer = 0.1f;
    			timeouts = 0;
    		}
        }
	}
    
	/// <summary>
	/// Checks the task result and returns true if it's ready for submission
	/// </summary>
	/// <returns><c>true</c>, if it's ready for submission, not if it's the right answer, <c>false</c> otherwise.</returns>
	bool CheckResult(){
		string myAnswer = result.ToString ();
		string rightAnswer = curTask.correctAnswer.ToString ();
		bool mismatch0 = rightAnswer [0] != myAnswer [0];
		bool mismatch1 = digits > 1 && myAnswer.Length > 1 && rightAnswer [1] != myAnswer [1];
		bool mismatch2 = digits > 2 && myAnswer.Length > 2 && rightAnswer [2] != myAnswer [2];
		if (result > 1000
		    || curTask.correctAnswer == result
		    || myAnswer.Length == digits
		    || mismatch0
		    || mismatch1
		    || mismatch2) 
		{
			return true;
		}
		return false;
	}

	void TrySubmit(){
		//Debug.Log ("Try submit");
		failsafeTimer = 1f;
		if (curTask != null) {
			//When do we submit result?
			if (CheckResult()) 
			{
				ShowResult ();
			}
		}
		else {
			Debug.LogError ("No task generated, trying to save face ");
			gameData.createFirstTask ();
		}
	}
		

	public void NewTask(FMC_Task task){

		curTask = task;
		ui.SetTask (curTask.task);
        
		timeLimit = task.timeSpecification;
		result = 0;
		ui.SetSolution ("");
		isAnimating = false;
		timer = 0;
        timeWhenTaskWasCreated = DateTime.Now;
        ui.SetTimer (timer, timeLimit);
		digits = curTask.correctAnswer.ToString ().Length;

		if (timeouts >= 2) {
			timeouts = 0;
			main.TogglePause ();
		}
	}
	public void AutoSuccess(){
		statistics = false;
		ui.Success ();
		main.Jump ();

		if (gameData != null){
			curTask.setUserAnswer (curTask.correctAnswer, true);
		}
		WaitForAnimation (1);
	}
	public void AutoFail(){
		statistics = false;
		timer = 0;
		//ui.Fail (); //Removed at Wolfgang's requrst
		ui.SetSolution(curTask.correctAnswer.ToString());
		main.Fall ();
		ui.SetSolution((int)curTask.correctAnswer);

		if (gameData != null){
			curTask.setUserAnswer (-1, false);
		}
		WaitForAnimation (1);
	}

	void ShowResult(){
		statistics = true;
		curTask.setUserAnswer (result, false);
        

		if (curTask.answeredCorrectly) {
			ui.Success ();
			main.Jump ();
		}
		else {
			ui.Fail ();
			main.Fall ();
			ui.SetSolution((int)curTask.correctAnswer);
		}
        curTask.setTime((float)(DateTime.Now - timeWhenTaskWasCreated).TotalSeconds);
        WaitForAnimation (waitTime);
	}
    
	void WaitForAnimation(float t){
		isAnimating = true;
		animTimer = t;
	}

	void SendResult(){

		isAnimating = false;

		if (gameData != null) {
			if (statistics) {
				gameData.answerTask (curTask);
				statistics = false;
			}
			else {
				gameData.answerTask (curTask);
			}
		}
		else {
			Debug.LogWarning("No Gamedata object");
		}


	}

	public void Click(){
		LeanAudio.play(click, 0.20f);
	}

	public string task{
		get { 
			if (curTask != null) {
				return curTask.task;
			}
			else {
				return null;
			}
		}
	}

	public int answer{
		get {
			if (curTask != null) {
				return (int)curTask.correctAnswer; 
			}
			else {
				return -1;
			}
		}
	}

	void Keyboard(){
  
	
		if (Input.GetKeyDown(KeyCode.Keypad0)){  
			Enter (0);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)){  
			Enter (1);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)){  
			Enter (2);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)){  
			Enter (3);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)){  
			Enter (4);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)){  
			Enter (5);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)){  
			Enter (6);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7)){  
			Enter (7);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8)){  
			Enter (8);
		}
		else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9)){  
			Enter (9);
		}
		else if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)){  
			Enter (-1);
		}

		else if (Input.GetKeyDown(KeyCode.O)){
			main.TogglePause ();
		}
	}

    private void OnApplicationPause(bool isPaused)
    {
        if (!isPaused)
            timeWhenTaskWasCreated = DateTime.Now;
    }
}
