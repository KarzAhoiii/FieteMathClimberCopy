using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatingState : IGameState {

	//private ClimberState _state = ClimberState.JUMPING;
	private int _direction;

	public AnimatingState(int dir){
		_direction = dir;
	}

	public void OnEnter () {

	}
	public void Update () {

	}

	public void OnExit () {

	}

	public ClimberState state {
		get {
			if (_direction > 0) {
				return ClimberState.JUMPING; 
			}
			else {
				return ClimberState.FALLING;
			}
		}
	}
}
