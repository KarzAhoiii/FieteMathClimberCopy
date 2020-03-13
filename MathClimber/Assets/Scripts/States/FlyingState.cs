using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : IGameState {

	ClimberState _state = ClimberState.FLYING;

	public FlyingState(){

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
