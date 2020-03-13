using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IGameState {

	ClimberState _state = ClimberState.MOVING;

	public MovingState(){

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
