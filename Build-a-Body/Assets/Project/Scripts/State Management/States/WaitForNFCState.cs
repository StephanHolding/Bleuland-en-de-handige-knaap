using UnityEngine;

public class WaitForNFCState : GameState
{

    private NFCScanner scanner;
    private AnimatedBook book;
    private CameraMovementController camController;
    private string organName;

    public override void OnStateEnter()
    {
        scanner = GameObject.FindObjectOfType<NFCScanner>();
        book = GameObject.FindObjectOfType<AnimatedBook>();
        camController = Camera.main.GetComponent<CameraMovementController>();

        scanner.EnableBackgroundScanning();
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }

    public override void OnStateExit()
    {
        scanner.DisableBackgroundScanning();
        scanner.OnNfcTagFound -= Scanner_OnNfcTagFound;
    }

    private void Scanner_OnNfcTagFound(string id, string payload)
    {
        if (IsOrganName(payload))
        {
            organName = payload;
            camController.GoTo("bookcase", OnMoveFinished: StartBookAnimation);
        }
    }

    private void StartBookAnimation()
    {
        book.StartOpeningAnimation(LoadNextScene);
    }

    private void LoadNextScene()
    {
        switch (organName)
        {
            case "heart":

                SceneHandler.instance.OnSceneLoaded_Once += delegate
                {
                    GameStateManager.instance.GoToGamestate<HeartMinigameState>();
                };
                SceneHandler.instance.LoadScene("Heart Puzzle");

                break;

            case "lungs":
                SceneHandler.instance.OnSceneLoaded_Once += delegate
                {
                    GameStateManager.instance.GoToGamestate<LungsMinigameState>();
                };
                SceneHandler.instance.LoadScene("Lungs Puzzle");
                break;
        }
    }

    private bool IsOrganName(string payload)
    {
        return payload == "heart" || payload == "lungs";
    }
}
