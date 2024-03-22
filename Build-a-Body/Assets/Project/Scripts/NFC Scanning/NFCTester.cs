using TMPro;
using UnityEngine;

public class NFCTester : MonoBehaviour
{

    public TextMeshProUGUI output;

    void Start()
    {
        output.text = "nothing...";

        NFCScanner scanner = FindObjectOfType<NFCScanner>();
        scanner.EnableBackgroundScanning();
        scanner.OnNfcTagFound += Scanner_OnNfcTagFound;
    }

    private void Scanner_OnNfcTagFound(string id, string payload)
    {
        output.text = "id: " + id + "\n \n payload: " + payload;
    }
}
