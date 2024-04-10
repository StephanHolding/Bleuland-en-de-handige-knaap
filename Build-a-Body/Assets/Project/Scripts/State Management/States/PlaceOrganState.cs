using Dialogue;
using UnityEngine;

public class PlaceOrganState : GameState
{
    private OrganSpawner organSpawner;

    public override void OnStateEnter()
    {
        organSpawner = GameObject.FindObjectOfType<OrganSpawner>();
        organSpawner.SpawnLockedOrgans();


        string completedMinigame = Blackboard.Read<string>(BlackboardKeys.LAST_FINISHED_MINIGAME);
        switch (completedMinigame)
        {
            case "Heart":
                DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_COMPLETED"), delegate { organSpawner.SpawnMoveableOrgans(); });
                break;
        }
    }

    public override void PlayerCompletedTask()
    {
        DialogueManager.instance.Say(DialogueManager.LoadStoryFromResources("HEART_PLACED"), delegate { GameStateManager.instance.GoToGamestate<WaitForNFCState>(); });
    }
}
