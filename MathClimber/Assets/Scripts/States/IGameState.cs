
public interface IGameState  {

	void OnEnter();
	void Update();
	void OnExit();

	ClimberState state{ get; }
}
