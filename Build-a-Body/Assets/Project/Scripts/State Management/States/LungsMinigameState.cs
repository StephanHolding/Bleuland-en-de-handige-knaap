using FMOD_AudioManagement;

public class LungsMinigameState : GameState
{
    public override void OnStateEnter()
    {
        FMODAudioManager.instance.Play("puzzle bg");
    }

    public override void PlayerCompletedTask()
    {
        Blackboard.Write(BlackboardKeys.LAST_FINISHED_MINIGAME, "Lungs");

        byte finishedOrgans = Blackboard.Read<byte>(BlackboardKeys.FINISHED_ORGANS);
        finishedOrgans |= 1 << (int)BlackboardKeys.OrganBitIndex.Lungs;
        Blackboard.Write(BlackboardKeys.FINISHED_ORGANS, finishedOrgans);

        SceneHandler.instance.OnSceneLoaded_Once += delegate
        {
            GameStateManager.instance.GoToGamestate<PlaceOrganState>();
        };
        SceneHandler.instance.LoadScene("Main_Scene");
    }
}
