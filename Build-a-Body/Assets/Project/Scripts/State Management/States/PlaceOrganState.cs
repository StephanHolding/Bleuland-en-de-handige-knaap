using Dialogue;
using UnityEngine;

public class PlaceOrganState : GameState
{
    private OrganSpawner organSpawner;
    private CameraMovementController camController;

    public override void OnStateEnter()
    {
        organSpawner = GameObject.FindObjectOfType<OrganSpawner>();
        camController = Camera.main.GetComponent<CameraMovementController>();

        organSpawner.SpawnLockedOrgans();
        camController.GoTo("bookcase", gradual: false);
        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_COMPLETED"), delegate { organSpawner.SpawnMoveableOrgans(); });
    }

    public override void PlayerCompletedTask()
    {
        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
