using System;
using System.Collections.Generic;

public class GameStateManager : SingletonTemplateMono<GameStateManager>
{
    private GameState currentState;

    private Dictionary<Type, GameState> allGameStates = new Dictionary<Type, GameState>();

    protected override void Awake()
    {
        base.Awake();
        GenerateGameStates();
    }

    private void Start()
    {
        GoToGamestate<IntroState>();
    }

    private void Update()
    {
        currentState?.OnStateUpdate();
    }

    public void GoToGamestate<T>()
    {
        currentState?.OnStateExit();
        currentState = allGameStates[typeof(T)];
        currentState.OnStateEnter();
    }

    public bool IsGamestate<T>()
    {
        return currentState != null && currentState.GetType() == typeof(T);
    }

    public void PlayerCompletedTask()
    {
        currentState?.PlayerCompletedTask();
    }

    private void GenerateGameStates()
    {
        AddStateToDictionary(new IntroState());
        AddStateToDictionary(new WaitForNFCState());
        AddStateToDictionary(new LungsMinigameState());
        AddStateToDictionary(new PlaceOrganState());
        AddStateToDictionary(new HeartMinigameState());
    }

    private void AddStateToDictionary(GameState newState)
    {
        allGameStates.Add(newState.GetType(), newState);
    }

}
