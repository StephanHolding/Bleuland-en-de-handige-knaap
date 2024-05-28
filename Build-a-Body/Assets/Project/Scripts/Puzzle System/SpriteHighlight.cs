using System.Collections;
using TMPro;
using UnityEngine;

public class SpriteHighlight : MonoBehaviour
{
    public string colorProperty = "_Color";

    private Renderer spriteRenderer;
    private Material spriteEmissiveMaterial;
    [ColorUsage(false, true)]
    private Color originalSpriteColor;
    private Color originalTextColor;
    private TextMeshPro textmesh;

    private void Awake()
    {
        spriteRenderer = GetComponent<Renderer>();
        spriteEmissiveMaterial = spriteRenderer.material;

        textmesh = GetComponent<TextMeshPro>();

        if (textmesh != null)
        {
            originalTextColor = textmesh.color;
        }
        else
        {
            originalSpriteColor = spriteEmissiveMaterial.GetColor(colorProperty);
        }
    }

    public void Highlight(Color highlightColor, float lerpSpeed)
    {
        StartCoroutine(HighlightCoroutine(highlightColor, lerpSpeed));
    }

    private IEnumerator HighlightCoroutine(Color highlightColor, float lerpSpeed)
    {
        float lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * lerpSpeed;
            spriteEmissiveMaterial.SetColor(colorProperty, Color.Lerp(originalSpriteColor, highlightColor, lerpValue));

            if (textmesh != null)
            {
                textmesh.color = Color.Lerp(originalTextColor, highlightColor, lerpValue);
            }


            yield return new WaitForEndOfFrame();
        }

        while (lerpValue > 0.5f)
        {
            lerpValue -= Time.deltaTime * lerpSpeed;
            spriteEmissiveMaterial.SetColor(colorProperty, Color.Lerp(originalSpriteColor, highlightColor, lerpValue));

            if (textmesh != null)
            {
                textmesh.color = Color.Lerp(originalTextColor, highlightColor, lerpValue);
            }

            yield return new WaitForEndOfFrame();
        }

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime * lerpSpeed;
            spriteEmissiveMaterial.SetColor(colorProperty, Color.Lerp(originalSpriteColor, highlightColor, lerpValue));

            if (textmesh != null)
            {
                textmesh.color = Color.Lerp(originalTextColor, highlightColor, lerpValue);
            }

            yield return new WaitForEndOfFrame();
        }

        while (lerpValue > 0)
        {
            lerpValue -= Time.deltaTime * lerpSpeed;
            spriteEmissiveMaterial.SetColor(colorProperty, Color.Lerp(originalSpriteColor, highlightColor, lerpValue));

            if (textmesh != null)
            {
                textmesh.color = Color.Lerp(originalTextColor, highlightColor, lerpValue);
            }

            yield return new WaitForEndOfFrame();
        }

        spriteEmissiveMaterial.SetColor(colorProperty, originalSpriteColor);
    }

}
