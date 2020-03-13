using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : IGameState {

	ClimberState _state = ClimberState.SHOP;
	BankController _shopBank;

	public ShopState(BankController bank){
		_shopBank = bank;
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
