using Dialogue;
using FMOD_AudioManagement;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class IntroState : GameState
{

    private CameraMovementController camController;

    public IntroState(GameStateManager gameStateManager) : base(gameStateManager)
    {
    }

    public override void OnStateEnter()
    {
        camController = Camera.main.transform.GetComponent<CameraMovementController>();
        camController.GoTo("overview");

        FMODAudioManager.instance.Play("main bg");

        Debug.Log("Current localization: " + LocalizationSettings.SelectedLocale.Identifier.Code);

        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_START"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
