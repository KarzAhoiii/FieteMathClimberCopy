using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : IGameState {

	ClimberState _state = ClimberState.PAUSE;
	TaskUI _ui;
	//GameController _main;

	public PauseState(TaskUI ui){
		_ui = ui;
		//_main = main;
	}
	
	public void OnEnter () {
		LeanTween.pauseAll ();
		_ui.Pause ();
	}
	public void Update () {

	}

	public void OnExit () {
		LeanTween.resumeAll ();
		_ui.Unpause ();
	}

	public ClimberState state {
		get { return _state; }
	}
}
