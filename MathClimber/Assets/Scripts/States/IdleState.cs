using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IGameState {

	ClimberState _state = ClimberState.IDLE;

	public IdleState(){

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
