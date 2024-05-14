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

        SceneHandler.instance.OnSceneLoaded_Once += delegate
        {
            GameStateManager.instance.GoToGamestate<PlaceOrganState>();
        };
        SceneHandler.instance.LoadScene("Main_Scene");
    }
}
