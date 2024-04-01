using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuicktimeEvent : MonoBehaviour
{

    public Image quickTimeEventCircle;
    public Button uiButton;

    public event Action OnEventFailed;
    public event Action OnEventSuccess;


    private float reactionTime;

    private const float MAX_SCALE = 1.75f;
    private const float MIN_SCALE = 0.9f;

    private void Awake()
    {
        uiButton.onClick.AddListener(ButtonPressed);
    }

    public void EnableWithQuicktimeEvent(float reactionTime)
    {
        quickTimeEventCircle.gameObject.SetActive(true);
        uiButton.gameObject.SetActive(true);

        this.reactionTime = reactionTime;
        StartCoroutine(ScaleOverTime());
    }

    public void Enable()
    {
        uiButton.gameObject.SetActive(true);
        quickTimeEventCircle.gameObject.SetActive(false);
    }

    public void Disable()
    {
        quickTimeEventCircle.gameObject.SetActive(false);
        uiButton.gameObject.SetActive(false);
    }

    public void ButtonPressed()
    {
        StopAllCoroutines();
        OnEventSuccess?.Invoke();

        Disable();
    }

    public void DebugPress()
    {
        uiButton.onClick.Invoke();
    }

    private IEnumerator ScaleOverTime()
    {
        float quicktimeTimer = 0;

        while (quicktimeTimer < reactionTime)
        {
            quicktimeTimer += Time.deltaTime;

            float diffPercentage = quicktimeTimer / reactionTime;
            float scale = Mathf.Lerp(MAX_SCALE, MIN_SCALE, diffPercentage);
            quickTimeEventCircle.transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForEndOfFrame();
        }

        OnEventFailed?.Invoke();
    }

}
