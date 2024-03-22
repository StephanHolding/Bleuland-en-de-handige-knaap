public class GameManager : SingletonTemplateMono<GameManager>
{

    private NFCScanner scanner;

    protected override void Awake()
    {
        base.Awake();
        scanner = FindAnyObjectByType<NFCScanner>();
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }



    private void OnDestroy()
    {
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }

    private void Scanner_OnNfcTagFound(string id, string payload)
    {
        if (payload.StartsWith("scene="))
        {
            string sceneName = payload.Split('=', System.StringSplitOptions.RemoveEmptyEntries)[1];

            ScreenLogger.Log("trying to load " + sceneName);

            SceneHandler.instance.LoadScene(sceneName);
        }
    }

}
