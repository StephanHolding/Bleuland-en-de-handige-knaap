using System;
using UnityEngine;

public class NFCScanner : MonoBehaviour
{

    public Action<int> nfcChipScanned;


    private void Update()
    {
        //placeholder for nfc functionalty
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            nfcChipScanned?.Invoke(1);
        }
    }

}
