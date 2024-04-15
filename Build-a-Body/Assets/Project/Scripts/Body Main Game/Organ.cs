using UnityEngine;

public class Organ : Draggable
{

    public Vector3 targetPosition;
    public Quaternion targetOrientation;
    public Vector3 targetCameraPosition;

    public string organID;
    

    private const float DISTANCE_THRESHOLD = 1f;
    private const float ROTATION_THRESHOLD = 30f;

    private void Awake()
    {
        if (OrganPlaceManager.instance.organDict.ContainsKey(organID))
            OrganPlaceManager.instance.organDict[organID] = this;
        else
            OrganPlaceManager.instance.organDict.Add(organID, this);
    }

    public void Update()
    {
        base.Update();
        if (!dragging && IsInCorrectPosition())
        {
            transform.position = targetPosition;
            transform.rotation = targetOrientation;
            OrganPlaceManager.instance.FinishPlacement(organID);
        }
    }

    [ContextMenu("Save Position")]
    public void SaveLocation()
    {
        targetPosition = transform.position;
        targetOrientation = transform.rotation;
    }

    public bool IsInCorrectPosition()
    {
        return (transform.position - targetPosition).magnitude <= DISTANCE_THRESHOLD
            && Mathf.Abs(transform.rotation.eulerAngles.y - targetOrientation.eulerAngles.y) <= ROTATION_THRESHOLD;
    }
}
