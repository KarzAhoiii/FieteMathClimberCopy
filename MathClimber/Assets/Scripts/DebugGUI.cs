using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : MonoBehaviour {

	
	GameController jump;
	CharacterScript chara;
	StairController stair;
	DropManager drop;
	InputManager input;
	bool inDebug;
	bool touch;
	public GameObject cheatPanel;

	string task;
	string solution;
	// Use this for initialization
	void Start () {
		jump = FindObjectOfType<GameController> ();
		chara = FindObjectOfType<CharacterScript>();
		stair = FindObjectOfType<StairController>();
		drop = FindObjectOfType<DropManager> ();
		input = FindObjectOfType<InputManager> ();

		cheatPanel.SetActive (false);
	}
	
	// Update is called once per frame
	/*
	void Update () {

		
		if(Input.GetKeyDown(KeyCode.A) || (Input.touchCount == 5 && !touch)){
		print("*****");
			touch = true;
			if (!inDebug) {
				inDebug = true;
			}
			else if (!cheatPanel.activeSelf) {
				cheatPanel.SetActive (true);
			}
			else {
				inDebug = false;
				cheatPanel.SetActive (false);
			}
		}
		if (Input.touchCount < 5) {
			touch = false;
		}
		if (Input.GetKeyDown (KeyCode.Equals)) {

			drop.SpawnForced (PickableItem.Type.ROCKET, 7, 7);
		}

		if (input.task != null) {
			task = string.Empty + input.task;
		}
		else{
			task = "N/A";
		}
		if(input.answer != -1){
			solution = string.Empty + input.answer;

		}
		else{
			solution = "N/A";
		}
	}

	void OnGUI () {
		if (inDebug) {
			GUI.color = Color.blue;
			GUI.Label (new Rect (10, 10, 200, 30), "" + ClimberStateManager.state);
			GUI.Label (new Rect (10, 30, 200, 30), "Success count: " + jump.count);
			GUI.Label (new Rect (10, 50, 200, 30), "Jump count: " + jump.count2);
			GUI.Label (new Rect (10, 70, 200, 30), "Pos: " + chara.transform.position);
			GUI.Label (new Rect (10, 90, 200, 30), "Cur task: " + input.task);
			GUI.Label (new Rect (10, 110, 200, 30), "Cur solution: " + input.answer);
		}
//		else {
//			GUI.Label (new Rect (10, 10, 200, 30), "" + Input.touchCount);
//		}
	}
	*/

}
