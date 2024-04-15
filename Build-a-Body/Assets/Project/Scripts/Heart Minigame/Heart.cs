using UnityEngine;

public class Heart : MonoBehaviour
{

    private float pinchValueModifier = 50;

    [SerializeField]
    private float currentHoldingBlood;
    private RangeBar rangeBar;
    private float arrowValue = 100;
    private float arrowLoweringSpeed = 5;
    private float accumulativePinchValue;
    private float maxHoldingBlood = 15;
    private const float HEART_SCALE_MIN = 0.8F;
    private const float HEART_SCALE_MAX = 1.2F;


    private void Start()
    {
        rangeBar = FindObjectOfType<RangeBar>();
        rangeBar.SetGreenBarRange(0, 100, 60, 70);

        TouchscreenInputHandler.OnPinchDetected += Pinching;
    }

    private void OnDestroy()
    {
        TouchscreenInputHandler.OnPinchDetected -= Pinching;
    }

    private void Update()
    {
        UpdateArrow();
    }

    private void Pinching(float pinchAmount)
    {
        accumulativePinchValue += pinchAmount;
        accumulativePinchValue = Mathf.Clamp(accumulativePinchValue, 0, 1);

        LerpHeart();

        if (pinchAmount > 0)
        {
            TakeBloodIn();
        }
        else if (pinchAmount < 0)
        {
            PushBloodOut();
        }
    }

    private void LerpHeart()
    {
        transform.localScale = Vector3.Lerp(new Vector3(HEART_SCALE_MIN, HEART_SCALE_MIN, HEART_SCALE_MIN), new Vector3(HEART_SCALE_MAX, HEART_SCALE_MAX, HEART_SCALE_MAX), accumulativePinchValue);
    }

    private void TakeBloodIn()
    {
        currentHoldingBlood = Mathf.Lerp(0, maxHoldingBlood, accumulativePinchValue);
    }

    private void PushBloodOut()
    {
        float newHoldingBlood = Mathf.Lerp(0, maxHoldingBlood, accumulativePinchValue);
        float bloodDifference = currentHoldingBlood - newHoldingBlood;

        arrowValue += bloodDifference;
        arrowValue = Mathf.Clamp(arrowValue, 0, 100);

        currentHoldingBlood = newHoldingBlood;
    }

    private void UpdateArrow()
    {
        arrowValue -= arrowLoweringSpeed * Time.deltaTime;
        rangeBar.PlaceArrow(arrowValue);
    }
}
