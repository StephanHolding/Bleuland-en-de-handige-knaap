using FMOD_AudioManagement;

public class HeartMinigameState : GameState
{
    public HeartMinigameState(GameStateManager gameStateManager) : base(gameStateManager)
    {
    }

    public override void OnStateEnter()
    {
        FMODAudioManager.instance.Play("puzzle bg");
    }

    public override void PlayerCompletedTask()
    {
        Blackboard.Write(BlackboardKeys.LAST_FINISHED_MINIGAME, "Heart");

        byte finishedOrgans = Blackboard.Read<byte>(BlackboardKeys.FINISHED_ORGANS);
        finishedOrgans |= 1 << (int)BlackboardKeys.OrganBitIndex.Heart;
        Blackboard.Write(BlackboardKeys.FINISHED_ORGANS, finishedOrgans);

        SceneHandler.instance.OnSceneLoaded_Once += delegate
        {
            GameStateManager.instance.GoToGamestate<PlaceOrganState>();
        };
        SceneHandler.instance.LoadScene("Main_Scene");
    }
}
