using Dialogue;

public class IntroState : GameState
{
    public override void OnStateEnter()
    {
        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_START"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
