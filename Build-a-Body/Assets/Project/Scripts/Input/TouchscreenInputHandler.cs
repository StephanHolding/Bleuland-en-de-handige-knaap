using System;
using UnityEngine;

public class TouchscreenInputHandler : MonoBehaviour
{

    public static event Action<float> OnPinchDetected;

    private TouchActions actionMap;
    private bool twoFingersInContact = false;

    void Awake()
    {
        actionMap = new TouchActions();
        actionMap.Touchscreen.OnSecondFingerContact.started += delegate { twoFingersInContact = true; };
        actionMap.Touchscreen.OnSecondFingerContact.canceled += delegate { twoFingersInContact = false; };
    }

    private void OnDestroy()
    {
        actionMap.Dispose();
    }

    private void OnEnable()
    {
        actionMap.Enable();
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }


    void Update()
    {
        if (twoFingersInContact)
        {
            DetectPinch();
        }
    }

    private void DetectPinch()
    {
        float distanceBetweenFingers = Vector2.Distance(actionMap.Touchscreen.FirstFingerPosition.ReadValue<Vector2>(), actionMap.Touchscreen.SecondFingerPosition.ReadValue<Vector2>());
        float lastDistance = 0.0f;
        if (distanceBetweenFingers != lastDistance)
        {
            OnPinchDetected(distanceBetweenFingers - lastDistance);
        }

        lastDistance = distanceBetweenFingers;
    }
}
