public class GameManager : SingletonTemplateMono<GameManager>
{

    private NFCScanner scanner;

    protected override void Awake()
    {
        base.Awake();
        scanner = FindAnyObjectByType<NFCScanner>();
        scanner.nfcChipScanned += OnNfcChipScanned;
    }

    private void OnDestroy()
    {
        scanner.nfcChipScanned -= OnNfcChipScanned;
    }

    public void OnNfcChipScanned(int index)
    {
        switch (index)
        {
            case 1:
                SceneHandler.instance.LoadScene("Heart Minigame");
                break;
        }
    }

}
