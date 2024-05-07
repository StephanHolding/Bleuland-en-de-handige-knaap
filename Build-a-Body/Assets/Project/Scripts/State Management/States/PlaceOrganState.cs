using Dialogue;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOrganState : GameState
{
    private OrganSpawner organSpawner;
    private CameraMovementController camController;

    private const int ORGAN_AMOUNT_FOR_COMPLETION = 2;

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
        List<string> lockedOrgans = Blackboard.Read<List<string>>(BlackboardKeys.LOCKED_ORGANS);
        if (lockedOrgans.Count >= ORGAN_AMOUNT_FOR_COMPLETION)
        {
            DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_END"), delegate
            {
                SceneHandler.instance.OnSceneLoaded_Once += delegate
                {
                    GameStateManager.instance.GoToGamestate<IntroState>();
                };

                SceneHandler.instance.LoadScene(0);
            });
        }
        else
        {
            DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
        }
    }
}
