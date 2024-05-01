using Dialogue;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class IntroState : GameState
{

    private CameraMovementController camController;

    public override void OnStateEnter()
    {
        camController = Camera.main.transform.GetComponent<CameraMovementController>();
        camController.GoTo("overview");


        Debug.Log("Current localization: " + LocalizationSettings.SelectedLocale.Identifier.Code);

        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_START"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
