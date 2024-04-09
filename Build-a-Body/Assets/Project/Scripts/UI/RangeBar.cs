using UnityEngine;

public class RangeBar : MonoBehaviour
{
    private RectTransform redBar;
    private RectTransform greenBar;
    private RectTransform arrow;

    private float savedMinValue, savedMaxValue;

    private void Awake()
    {
        redBar = transform.Find("Red Bar").GetComponent<RectTransform>();
        greenBar = transform.Find("Green Bar").GetComponent<RectTransform>();
        arrow = transform.Find("Arrow").GetComponent<RectTransform>();
    }

    public void SetGreenBarRange(float minValue, float maxValue, float greenMinValue, float greenMaxValue)
    {
        this.savedMinValue = minValue;
        this.savedMaxValue = maxValue;

        float greenRangePercentage = (greenMaxValue - greenMinValue) / (maxValue - minValue);
        float positionPercentage = (greenMinValue - minValue) / (maxValue - minValue);

        greenBar.sizeDelta = new Vector2(redBar.rect.width * greenRangePercentage, greenBar.rect.height);
        greenBar.anchoredPosition = new Vector2(redBar.rect.x + positionPercentage * redBar.rect.width, greenBar.anchoredPosition.y);
    }

    public void PlaceArrow(float value)
    {
        float percentage;

        if (value <= savedMinValue)
        {
            percentage = 0;
        }
        else if (value >= savedMaxValue)
        {
            percentage = 1;
        }
        else
        {
            percentage = (value - savedMinValue) / (savedMaxValue - savedMinValue);
        }


        arrow.anchoredPosition = new Vector2(redBar.rect.x + redBar.rect.width * percentage, arrow.anchoredPosition.y);
    }

}
