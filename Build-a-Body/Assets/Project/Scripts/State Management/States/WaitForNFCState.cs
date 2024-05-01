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
        SceneHandler.instance.OnSceneLoaded_Once += delegate
        {
            GameStateManager.instance.GoToGamestate<HeartMinigameState>();
        };

        if (organName == "heart")
            SceneHandler.instance.LoadScene("Heart Puzzle");
        else if (organName == "lungs")
            SceneHandler.instance.LoadScene("Lungs Puzzle");
    }

    private bool IsOrganName(string payload)
    {
        return payload == "heart" || payload == "lungs";
    }
}
