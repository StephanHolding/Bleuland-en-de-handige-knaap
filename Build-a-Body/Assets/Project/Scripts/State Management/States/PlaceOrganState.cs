using Dialogue;
using Dialogue.Blackboard;
using FMOD_AudioManagement;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOrganState : GameState
{
    private OrganSpawner organSpawner;
    private CameraMovementController camController;
    private string lastCompletedMinigame;
    private bool taskCompleted = false;

    private const int ORGAN_AMOUNT_FOR_COMPLETION = 2;

    public PlaceOrganState(GameStateManager gameStateManager) : base(gameStateManager)
    {
    }

    public override void OnStateEnter()
    {
        organSpawner = GameObject.FindObjectOfType<OrganSpawner>();
        camController = Camera.main.GetComponent<CameraMovementController>();

        FMODAudioManager.instance.Play("main bg");

        organSpawner.SpawnLockedOrgans();
        camController.GoTo("bookcase", gradual: false);

        lastCompletedMinigame = Blackboard.Read<string>(BlackboardKeys.LAST_FINISHED_MINIGAME);
        taskCompleted = false;

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
        if (taskCompleted) return;
        taskCompleted = true;

        List<string> lockedOrgans = Blackboard.Read<List<string>>(BlackboardKeys.LOCKED_ORGANS);

        if (lockedOrgans.Count >= ORGAN_AMOUNT_FOR_COMPLETION)
        {
            DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("GAME_END"), CheckSendEmail);
        }
        else
        {
            switch (lastCompletedMinigame)
            {
                case "Heart":
                    DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
                    break;
                case "Lungs":
                    DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("LUNGS_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
                    break;

            }
        }
    }

    private void CheckSendEmail()
    {
        if (DialogueBlackboard.HasKey("email"))
        {
            string emailAdress = DialogueBlackboard.GetVariable<string>("email");

            if (!string.IsNullOrEmpty(emailAdress))
            {
                string playerName = DialogueBlackboard.GetVariable<string>("player_name");

                HttpPostRequestManager.PostRequest(playerName, emailAdress, "en", stateManager);
            }
        }

        SceneHandler.instance.LoadScene(0);
    }
}
