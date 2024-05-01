using System;
using UnityEngine;

public class NFCScanner : MonoBehaviour
{

    public event Action<string, string> OnNfcTagFound;

    private bool tagFound = false;
    private bool allowScanning = false;

    void Update()
    {
        if (allowScanning)
        {
            Scan();

            //DEBUGGING IN EDTOR ONLY:
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnNfcTagFound?.Invoke("1", "heart");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnNfcTagFound?.Invoke("2", "lungs");
            }
        }
    }

    public void EnableBackgroundScanning()
    {
        allowScanning = true;
    }

    public void DisableBackgroundScanning()
    {
        allowScanning = false;
    }

    private void Scan()
    {
        if (tagFound) return;

        if (Application.platform == RuntimePlatform.Android)
        {

            // Create new NFC Android object
            AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
            AndroidJavaObject mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
            string sAction = mIntent.Call<String>("getAction"); // results are returned in the Intent object

            if (sAction == "android.nfc.action.NDEF_DISCOVERED")
            {
                Debug.LogError("Tag of type NDEF");
            }
            else if (sAction == "android.nfc.action.TECH_DISCOVERED")
            {
                Debug.Log("TAG DISCOVERED");

                OnNfcTagFound?.Invoke(GetTagId(mIntent), GetTagPayload(mIntent));

                tagFound = true;

                ForgetNFC(mIntent);
                return;
            }
            else if (sAction == "android.nfc.action.TAG_DISCOVERED")
            {
                Debug.LogError("This type of tag is not supported !");
            }
        }
    }

    private string GetTagId(AndroidJavaObject mIntent)
    {
        AndroidJavaObject mNdefMessage = mIntent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
        if (mNdefMessage != null)
        {
            byte[] id = mNdefMessage.Call<byte[]>("getId");
            string toReturn = System.Convert.ToBase64String(id);
            return toReturn;
        }
        else
        {
            Debug.LogError("No Tag ID found!");
            return null;
        }
    }

    private string GetTagPayload(AndroidJavaObject mIntent)
    {
        AndroidJavaObject[] mNdefMessage = mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
        AndroidJavaObject[] mNdefRecord = mNdefMessage[0].Call<AndroidJavaObject[]>("getRecords");
        byte[] payLoad = mNdefRecord[0].Call<byte[]>("getPayload");
        string payloadstring = System.Text.Encoding.UTF8.GetString(payLoad);

        return payloadstring.Substring(3);
    }

    private void ForgetNFC(AndroidJavaObject mIntent)
    {
        mIntent.Call("removeExtra", "android.nfc.extra.TAG");
        mIntent.Call("removeExtra", "android.nfc.extra.NDEF_MESSAGES");
        tagFound = false;
    }
}