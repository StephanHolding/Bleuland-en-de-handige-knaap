using Dialogue;
using FMOD_AudioManagement;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOrganState : GameState
{
    private OrganSpawner organSpawner;
    private CameraMovementController camController;
    private string lastCompletedMinigame;

    private const int ORGAN_AMOUNT_FOR_COMPLETION = 2;

    public override void OnStateEnter()
    {
        organSpawner = GameObject.FindObjectOfType<OrganSpawner>();
        camController = Camera.main.GetComponent<CameraMovementController>();

        FMODAudioManager.instance.Play("main bg");

        organSpawner.SpawnLockedOrgans();
        camController.GoTo("bookcase", gradual: false);

        lastCompletedMinigame = Blackboard.Read<string>(BlackboardKeys.LAST_FINISHED_MINIGAME);

        switch (lastCompletedMinigame)
        {
            case "Heart":
                DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_COMPLETED"), delegate { organSpawner.SpawnMoveableOrgans(); });
                break;
            case "Lungs":
                DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("LUNGS_COMPLETED"), delegate { organSpawner.SpawnMoveableOrgans(); });
                break;
        }
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
            switch (lastCompletedMinigame)
            {
                case "Heart":
                    DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
                    break;
                case "Lungs":
                    DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("LUNGS_PLACED"), delegate { organSpawner.SpawnMoveableOrgans(); });
                    break;
            }
        }
    }
}
