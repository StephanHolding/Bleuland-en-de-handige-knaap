using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public float speed = 5.0f; // The speed at which the camera moves.
    public Vector3 offset = new Vector3(0, 0, -10); // Offset from the target object, to position the camera.

    private Transform target; // The transform component of the target object.

    void Update()
    {
        if (target != null)
        {
            // Smoothly move the camera to the desired position.
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }

    // Method to be called externally to move the camera to an object with a specified tag.
    public void GoTo(string tagName)
    {
        GameObject targetObject = GameObject.FindWithTag(tagName);
        if (targetObject != null)
        {
            target = targetObject.transform; // Set the target transform if the object is found.
        }
        else
        {
            Debug.LogError("No object with tag: " + tagName + " found."); // Log an error if no object with the given tag is found.
        }
    }
}