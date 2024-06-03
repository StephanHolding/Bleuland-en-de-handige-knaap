public class GameState
{

    public GameState(GameStateManager gameStateManager)
    {
        this.stateManager = gameStateManager;
    }

    protected GameStateManager stateManager;

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void OnStateUpdate() { }
    public virtual void PlayerCompletedTask() { }

}
