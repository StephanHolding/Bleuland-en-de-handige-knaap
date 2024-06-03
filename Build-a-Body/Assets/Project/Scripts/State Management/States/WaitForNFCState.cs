using FMOD_AudioManagement;
using HintSystem;
using UnityEngine;
public class WaitForNFCState : GameState
{

    private NFCScanner scanner;
    private AnimatedBook book;
    private CameraMovementController camController;
    private string organName;

    private GameObject cheatButtons;

    public WaitForNFCState(GameStateManager gameStateManager) : base(gameStateManager)
    {
    }

    public override void OnStateEnter()
    {
        scanner = GameObject.FindObjectOfType<NFCScanner>();
        book = GameObject.FindObjectOfType<AnimatedBook>();
        camController = Camera.main.GetComponent<CameraMovementController>();

        cheatButtons = GameObject.Find("CHEAT BUTTONS");

        cheatButtons.transform.GetChild(0).gameObject.SetActive(true);

        Hint.ShowHint("Gebruik de telefoon om een NFC tag te scannen", "nfcHint");

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
        ScreenLogger.Log(payload);

        if (IsOrganName(payload))
        {
            if (CanPlayMinigame(payload))
            {
                cheatButtons.transform.GetChild(0).gameObject.SetActive(false);

                organName = payload;
                camController.GoTo("bookcase", OnMoveFinished: StartBookAnimation);
            }
        }
    }

    private void StartBookAnimation()
    {
        Hint.SendCompletionKey("nfcHint");
        FMODAudioManager.instance.PlayOneShot("Book");
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
                SceneHandler.instance.LoadScene("Heart_Puzzle");

                break;

            case "lungs":
                SceneHandler.instance.OnSceneLoaded_Once += delegate
                {
                    GameStateManager.instance.GoToGamestate<LungsMinigameState>();
                };
                SceneHandler.instance.LoadScene("Lungs_Puzzle");
                break;
        }
    }

    private bool IsOrganName(string payload)
    {
        return payload == "heart" || payload == "lungs";
    }

    private bool CanPlayMinigame(string payload)
    {
        byte finishedOrgans = Blackboard.Read<byte>(BlackboardKeys.FINISHED_ORGANS);

        if (payload == "heart")
        {
            return (finishedOrgans & (1 << (int)BlackboardKeys.OrganBitIndex.Heart)) == 0;
        }
        else if (payload == "lungs")
        {
            return (finishedOrgans & (1 << (int)BlackboardKeys.OrganBitIndex.Lungs)) == 0;
        }

        return false;
    }
}
