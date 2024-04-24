using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    public string targetTag;
    public Transform targetTransform;


    public void Init()
    {
        if (string.IsNullOrEmpty(targetTag))
            targetTag = gameObject.name;

        if (targetTransform == null)
            targetTransform = transform;
    }

}
