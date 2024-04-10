using UnityEngine;

public class WaitForNFCState : GameState
{

    private NFCScanner scanner;

    public override void OnStateEnter()
    {
        scanner = GameObject.FindObjectOfType<NFCScanner>();
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
        if (payload.StartsWith("scene="))
        {
            string sceneName = payload.Split('=', System.StringSplitOptions.RemoveEmptyEntries)[1];

            ScreenLogger.Log("trying to load " + sceneName);

            switch (sceneName)
            {
                case "Heart Minigame":
                    SceneHandler.instance.OnSceneLoaded_Once += delegate
                    {
                        GameStateManager.instance.GoToGamestate<HeartMinigameState>();
                    };
                    break;
            }

            SceneHandler.instance.LoadScene(sceneName);
        }
    }
}
