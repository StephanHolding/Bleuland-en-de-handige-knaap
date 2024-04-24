using Dialogue;
using UnityEngine;

public class IntroState : GameState
{

    private CameraMovementController camController;

    public override void OnStateEnter()
    {
        camController = Camera.main.transform.GetComponent<CameraMovementController>();
        camController.GoTo("overview");


        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_START"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
