using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpState : IGameState {

	ClimberState _state = ClimberState.LEVELUP;
	LevelUpScreen _screen;
	TaskUI _ui;

	public LevelUpState(TaskUI ui, LevelUpScreen screen){
		_screen = screen;
		_ui = ui;
	}

	public void OnEnter () {

	}
	public void Update () {

	}

	public void OnExit () {
		_screen.Disable ();
		_ui.Unpause();
	}

	public ClimberState state {
		get { return _state; }
	}
}
