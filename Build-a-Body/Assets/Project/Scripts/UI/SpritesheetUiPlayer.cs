using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpritesheetUiPlayer : MonoBehaviour
{

    public SpriteArray toPlay;
    private Image output;

    private void Awake()
    {
        output = GetComponent<Image>();
        output.preserveAspect = true;
        output.enabled = false;
    }

    public void Play()
    {
        StartCoroutine(PlaySpritesheet());
    }

    public void Stop()
    {
        StopAllCoroutines();
        output.enabled = false;
        output.sprite = null;
    }

    private IEnumerator PlaySpritesheet()
    {
        output.enabled = true;

        float frameWaitTime = 1 / toPlay.animationFps;
        bool once = false;

        while (toPlay.loop || !once)
        {
            for (int i = 0; i < toPlay.sprites.Length; i++)
            {
                output.sprite = toPlay.sprites[i];
                yield return new WaitForSeconds(frameWaitTime);
            }

            once = true;
        }

        if (toPlay.lingerOnLastSprite)
        {
            output.sprite = toPlay.sprites[^1];
        }
        else
        {
            output.enabled = false;
            output.sprite = null;
        }
    }

}
