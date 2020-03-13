using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClimberState{
	IDLE, JUMPING, FALLING, SLIDING, MOVING, PAUSE, LEVELUP, SHOP, RELOADING, FLYING
}

public class ClimberStateManager {
	//ClimberState _prevState;
	//ClimberState _curState;
	List<IPauseHandler> pauseListeners;

	public IGameState _curState;
	IGameState _prevState;


	TaskUI ui;
	LevelUpScreen levelUpScreen;
	BankController shopBank;
	BankController normalBank;
	GameController main;

	public static ClimberStateManager _instance;
    

	public ClimberStateManager() {
		if (_instance == null) {
			_instance = this;
		}
		else if (_instance != this){
			//_instance = this;
			Debug.LogWarning ("There already is one State Manager");
			return;
		}
		pauseListeners = new List<IPauseHandler> ();
		_curState = new IdleState ();
	}

	public static void Init(Dependencies dep){

		_instance.ui = dep.ui;
		_instance.levelUpScreen = dep.levelUpScreen;
		_instance.shopBank = dep.shopBank;
		_instance.normalBank = dep.normalBank;
		_instance.main = dep.main;
	}

	public static void SwitchState(){

		SwitchState(_instance._prevState.state);
	}

	public static void SwitchState (ClimberState newState) {
		if (_instance._curState.state != newState) {
			if (newState == ClimberState.PAUSE || newState == ClimberState.LEVELUP || newState == ClimberState.SHOP) {
                for (int i = 0; i < _instance.pauseListeners.Count; i++) {
					_instance.pauseListeners [i].Pause ();
				}
			} else if (isPaused) {
				for (int i = 0; i < _instance.pauseListeners.Count; i++) {
					_instance.pauseListeners [i].Unpause ();
				}
			
			} else if (_instance._curState.state == ClimberState.RELOADING) {
				//Purge listeners
				_instance.pauseListeners = new List<IPauseHandler> ();
			}

			if (!isPaused) {
				_instance._prevState = _instance._curState;

			}
			_instance._curState.OnExit ();
            
            
			switch(newState){
            
    			case ClimberState.IDLE:
    				_instance._curState = new IdleState ();
    				break;
    			case ClimberState.JUMPING: 
    				_instance._curState = new AnimatingState (1);
    				break;
    			case ClimberState.FALLING: 
    				_instance._curState = new AnimatingState (-1);
    				break;
    			case ClimberState.MOVING:
    				_instance._curState = new MovingState ();
    				break;
    			case ClimberState.PAUSE: 
    				_instance._curState = new PauseState (_instance.ui);
    				break;
    			case ClimberState.LEVELUP:
    				_instance._curState = new LevelUpState (_instance.ui, _instance.levelUpScreen);
    				break;
    			case ClimberState.SHOP:
    				_instance._curState = new ShopState (_instance.shopBank);
    				break;
    			case ClimberState.FLYING: 
    				_instance._curState = new FlyingState ();
    				break;
    			case ClimberState.RELOADING: 
    				_instance._curState = new ReloadingState ();
    				break;
			}

			_instance._curState.OnEnter ();
		}
	}


	public static void Subscribe(IPauseHandler listener){
		_instance.pauseListeners.Add (listener);
	}
	public static void Unsubscribe(IPauseHandler listener){
		_instance.pauseListeners.Remove (listener);
	}

	public static ClimberState state{
		get{ return _instance._curState.state; }
	}
	public static bool isAnimating{
		get {return _instance._curState.state == ClimberState.JUMPING || _instance._curState.state == ClimberState.FALLING; }
	}

	public static bool isPaused{
		get {return _instance._curState.state == ClimberState.PAUSE || _instance._curState.state == ClimberState.LEVELUP || _instance._curState.state == ClimberState.SHOP; }
	}

	public static bool isFlying{
		get { return _instance._curState.state == ClimberState.FLYING; } 
	}

	public static bool inTestMode{
		get { return FMC_GameDataController.instance == null; }
	}
	public static bool inCampaign{
		get{ return FMC_GameDataController.instance != null && FMC_GameDataController.instance.getCurrentPlayMode () == FMC_Settings_Controller.activeSetting.storyMode; }
	}
	public static bool inTraining{
		get{ return FMC_GameDataController.instance != null && FMC_GameDataController.instance.getCurrentPlayMode () != FMC_Settings_Controller.activeSetting.storyMode; }
	}

	public static bool isInitialized{
		get{ return _instance != null; }
	}
}

public class Dependencies{
	public TaskUI ui;
	public LevelUpScreen levelUpScreen;
	public BankController shopBank;
	public BankController normalBank;
	public GameController main;
}
