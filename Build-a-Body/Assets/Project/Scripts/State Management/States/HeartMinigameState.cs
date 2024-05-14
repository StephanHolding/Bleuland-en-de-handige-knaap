using FMOD_AudioManagement;

public class HeartMinigameState : GameState
{
    public override void OnStateEnter()
    {
        FMODAudioManager.instance.Play("puzzle bg");
    }

    public override void PlayerCompletedTask()
    {
        Blackboard.Write(BlackboardKeys.LAST_FINISHED_MINIGAME, "Heart");

        SceneHandler.instance.OnSceneLoaded_Once += delegate
        {
            GameStateManager.instance.GoToGamestate<PlaceOrganState>();
        };
        SceneHandler.instance.LoadScene("Main_Scene");
    }
}
