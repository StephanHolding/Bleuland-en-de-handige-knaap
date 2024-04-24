using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public float moveTime = 2;

    private Dictionary<string, Transform> cameraTargets = new Dictionary<string, Transform>();
    private Transform target; // The transform component of the target object.

    private void Awake()
    {
        FillTargetDictionary();
    }

    private IEnumerator CameraLerp(Transform target, Action callback)
    {
        float lerp = 0;

        Vector3 originalPos = transform.position;
        Quaternion originalRot = transform.rotation;

        while (lerp < 1)
        {
            lerp += Time.deltaTime * moveTime;

            Vector3 smoothedPosition = Vector3.Lerp(originalPos, target.position, Mathf.SmoothStep(0, 1, lerp));
            Quaternion smoothedRotation = Quaternion.Lerp(originalRot, target.rotation, Mathf.SmoothStep(0, 1, lerp));
            transform.position = smoothedPosition;
            transform.rotation = smoothedRotation;

            yield return new WaitForEndOfFrame();
        }

        transform.position = target.position;
        transform.rotation = target.rotation;

        callback?.Invoke();
    }


    // Method to be called externally to move the camera to an object with a specified tag.
    public void GoTo(string tagName, Action OnMoveFinished = null)
    {
        Transform targetObject = cameraTargets[tagName];
        if (targetObject != null)
        {
            StartCoroutine(CameraLerp(targetObject, OnMoveFinished));
        }
        else
        {
            Debug.LogError("No object with tag: " + tagName + " found."); // Log an error if no object with the given tag is found.
        }
    }

    private void FillTargetDictionary()
    {
        CameraTarget[] targets = GameObject.FindObjectsOfType<CameraTarget>();
        foreach (CameraTarget cameraTarget in targets)
        {
            cameraTarget.Init();
            cameraTargets.Add(cameraTarget.targetTag, cameraTarget.targetTransform);
        }
    }
}