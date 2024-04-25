using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load scenes with a fade in and fade out. (see ScreenFade class for more info)
/// </summary>

public class SceneHandler : SingletonTemplateMono<SceneHandler>
{
    public bool useScreenfade;

    public delegate void SceneEvent();

    //Events with _Once will unsubscribe all their methods after executing. 

    public event SceneEvent OnSceneLoaded;
    public event SceneEvent OnSceneLoaded_Once;
    public event SceneEvent OnFadeInFinished;
    public event SceneEvent OnFadeInFinished_Once;
    public event SceneEvent OnFadeOutFinished;
    public event SceneEvent OnFadeOutFinished_Once;

    private bool isFading;
    private string sceneToLoad;

    protected override void Awake()
    {
        base.Awake();

        if (useScreenfade)
        {
            ScreenFade.OnFadeInDone += FadeInIsDone;
            ScreenFade.OnFadeOutDone += FadeOutIsDone;
            SceneManager.sceneLoaded += OnSceneHasBeenLoaded;
            StartCoroutine(WaitForMainCamera());
        }
    }

    private void OnDestroy()
    {
        if (useScreenfade)
        {
            ScreenFade.OnFadeInDone -= FadeInIsDone;
            ScreenFade.OnFadeOutDone -= FadeOutIsDone;
            SceneManager.sceneLoaded -= OnSceneHasBeenLoaded;
        }
    }

    public void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;

        if (useScreenfade)
        {
            ScreenFade.FadeOut();
            isFading = true;
            StartCoroutine(WaitForFade());
        }
        else
        {
            SceneLoading();
        }
    }

    public void LoadScene(int sceneIndex)
    {
        LoadScene(GetSceneNameByIndex(sceneIndex));
    }

    public bool IsSceneCurrent(string sceneName)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }

    public bool IsSceneCurrent(int sceneIndex)
    {
        return IsSceneCurrent(GetSceneNameByIndex(sceneIndex));
    }

    [ContextMenu("Reload Current Scene")]
    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    [ContextMenu("Load Next Scene")]
    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadScene(nextSceneIndex);
    }

    [ContextMenu("Load First Scene")]
    public void LoadFirstScene()
    {
        LoadScene(0);
    }

    [ContextMenu("Load Last Scene")]
    public void LoadLastScene()
    {
        LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    [ContextMenu("Quit Game")]
    public void QuitGame()
    {
        Application.Quit();
    }

    public void CancelSceneLoading()
    {
        ScreenFade.Cancel();
        isFading = false;
        StopAllCoroutines();
    }

    public bool NextSceneIsPresent()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        return nextSceneIndex < SceneManager.sceneCountInBuildSettings;
    }

    private void FadeInIsDone()
    {
        isFading = false;
        OnFadeOutFinished?.Invoke();
        OnFadeInFinished_Once?.Invoke();
        OnFadeInFinished_Once = null;
    }

    private void FadeOutIsDone()
    {
        isFading = false;
        OnFadeOutFinished?.Invoke();
        OnFadeOutFinished_Once?.Invoke();
        OnFadeOutFinished_Once = null;
    }

    private void OnSceneHasBeenLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForMainCamera());
        OnSceneLoaded?.Invoke();
        OnSceneLoaded_Once?.Invoke();
        OnSceneLoaded_Once = null;
    }

    private string GetSceneNameByIndex(int sceneIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        string toReturn = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        return toReturn;
    }

    private IEnumerator WaitForFade()
    {
        yield return new WaitUntil(() => isFading == false);
        yield return new WaitForSeconds(0.5f);
        SceneLoading();
    }

    private IEnumerator WaitForMainCamera()
    {
        yield return new WaitUntil(() => Camera.main != null);
        ScreenFade.FadeIn();
    }

    private void SceneLoading()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
