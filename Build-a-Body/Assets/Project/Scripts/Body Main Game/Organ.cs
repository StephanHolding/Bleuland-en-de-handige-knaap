using UnityEngine;

public class Organ : Draggable
{

    public Vector3 targetPosition;

    public string organName;

    private const float THRESHOLD = 1f;

    private void Start()
    {
        if (string.IsNullOrEmpty(organName))
        {
            organName = gameObject.name;
        }
    }


    [ContextMenu("Save Position")]
    public void SaveLocation()
    {
        targetPosition = transform.localPosition;
    }
}
