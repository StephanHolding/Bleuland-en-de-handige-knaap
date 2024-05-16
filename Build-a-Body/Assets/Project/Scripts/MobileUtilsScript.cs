using System.Collections;
using TMPro;
using UnityEngine;

public class MobileUtilsScript : MonoBehaviour
{

    private int FramesPerSec;
    private float frequency = 1.0f;
    private string fps;

    private TextMeshProUGUI fpstext;

    void Start()
    {
        fpstext = GetComponentInChildren<TextMeshProUGUI>();

        StartCoroutine(FPS());
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        fpstext.text = fps;
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }
}
