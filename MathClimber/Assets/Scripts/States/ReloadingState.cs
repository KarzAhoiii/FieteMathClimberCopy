using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingState : IGameState {

	ClimberState _state = ClimberState.RELOADING;

	public ReloadingState(){

	}

	public void OnEnter () {

	}
	public void Update () {

	}

	public void OnExit () {

	}

	public ClimberState state {
		get { return _state; }
	}
}
