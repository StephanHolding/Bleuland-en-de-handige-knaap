using System;
using System.Collections.Generic;
using ProjectHKU.UI;

public class GameManager : SingletonTemplateMono<GameManager>
{

    private Dictionary<string, bool> minigameCompletion = new Dictionary<string, bool>
    {
        { HEART_MINIGAME, false }
        //add more minigames later
    };

    private NFCScanner scanner;

    public const string HEART_MINIGAME = "Heart Minigame";

    public string currentWinMinigame = "";
    public bool placementFinish = true;

    protected override void Awake()
    {
        base.Awake();
        scanner = FindAnyObjectByType<NFCScanner>();
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }

    public void debugMinigame(string minigameName)
    {
        Scanner_OnNfcTagFound("DEBUG", "scene=" + minigameName);
    }

    public void CompleteMinigame(string miniGameName)
    {
        currentWinMinigame = miniGameName;
        placementFinish = false;
        minigameCompletion[miniGameName] = true;
    }

    public bool IsMinigameCompleted(string minigameName)
    {
        return minigameCompletion[minigameName];
    }

    private void OnDestroy()
    {
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }

    private void Scanner_OnNfcTagFound(string id, string payload)
    {
        if (!placementFinish) return;
        if (payload.StartsWith("scene="))
        {
            string sceneName = payload.Split('=', System.StringSplitOptions.RemoveEmptyEntries)[1];

            ScreenLogger.Log("trying to load " + sceneName);

            SceneHandler.instance.LoadScene(sceneName);
        }
    }

}
