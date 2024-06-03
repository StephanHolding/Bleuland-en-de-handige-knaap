using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpPostRequestManager
{
    private static readonly string url = "https://game.monsterlady.cloud/service/email/pdf";
    private static readonly string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzZW5kZXIiOnRydWUsImlhdCI6MTcxNTkwMzE3NH0.0g2OtjLCAcVSHJL8w_PxTL1m2hwUCNmkai7joBQ7UaA";

    public static void PostRequest(string name, string email, string language, MonoBehaviour caller)
    {
        string jsonData = $"{{\"name\":\"{name}\",\"email\":\"{email}\",\"language\":\"{language}\"}}";
        caller.StartCoroutine(SendPostRequest(jsonData));
    }

    private static IEnumerator SendPostRequest(string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
}