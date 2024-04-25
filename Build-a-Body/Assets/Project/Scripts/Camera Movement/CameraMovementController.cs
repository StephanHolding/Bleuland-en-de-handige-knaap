using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // The speed at which the camera moves.
    public float rotationSpeed = 20.0f;

    private Dictionary<string, Transform> cameraTargets = new Dictionary<string, Transform>();
    private Transform target; // The transform component of the target object.

    private void Awake()
    {
        FillTargetDictionary();
    }

    void Update()
    {
        if (target != null)
        {
            // Smoothly move the camera to the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            transform.rotation = smoothedRotation;
        }
    }

    // Method to be called externally to move the camera to an object with a specified tag.
    public void GoTo(string tagName)
    {
        Transform targetObject = cameraTargets[tagName];
        if (targetObject != null)
        {
            target = targetObject; // Set the target transform if the object is found.
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
            cameraTargets.Add(cameraTarget.targetTag, cameraTarget.targetTransform);
        }
    }
}