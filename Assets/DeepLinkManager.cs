using UnityEngine;
using UnityEngine.SceneManagement;

public class DeepLinkManager : MonoBehaviour
{
    public static DeepLinkManager Instance { get; private set; }
    public string deeplinkURL;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;                
            Application.deepLinkActivated += onDeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                onDeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    private void onDeepLinkActivated(string url)
    {
        // Update DeepLink Manager global variable, so URL can be accessed from anywhere.
        deeplinkURL = url;
        
// Decode the URL to determine action. 
// In this example, the application expects a link formatted like this:
// unitydl://mylink?scene1
        string sceneName = url.Split('?')[1];
        bool validScene;
        int sceneNum = 0;
        switch (sceneName)
        {
            case "1":
                validScene = true;
                sceneNum = 1;
                break;
            case "2":
                validScene = true;
                sceneNum = 2;
                break;
            case "3":
                validScene = true;
                sceneNum = 3;
                break;
            default:
                validScene = false;
                break;
        }
        if (validScene) SceneManager.LoadScene(sceneNum);
    }
}