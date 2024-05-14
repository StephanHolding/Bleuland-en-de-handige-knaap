using System.Collections.Generic;
using UnityEngine;

public class Organ : Draggable3D
{

    public Vector3 targetPosition;
    public Quaternion targetOrientation;
    public Vector3 targetCameraPosition;

    public string organID;


    private const float DISTANCE_THRESHOLD = 0.1f;
    private const float ROTATION_THRESHOLD = 30f;

    [ContextMenu("Save Position")]
    public void SaveLocation()
    {
        targetPosition = transform.position;
        targetOrientation = transform.rotation;
    }

    protected override void OnDraggingEnd()
    {
        if (IsInCorrectPosition())
        {
            OnOrganPlacedCorrectly();
        }
    }

    public bool IsInCorrectPosition()
    {
        //removed rotation check for testing. add again later
        //return (transform.position - targetPosition).magnitude <= DISTANCE_THRESHOLD && Mathf.Abs(transform.rotation.eulerAngles.y - targetOrientation.eulerAngles.y) <= ROTATION_THRESHOLD;

        return Vector3.Distance(targetPosition, transform.position) <= DISTANCE_THRESHOLD;
    }

    [ContextMenu("Override Organ Placing")]
    private void OnOrganPlacedCorrectly()
    {
        transform.position = targetPosition;
        transform.rotation = targetOrientation;
        draggable = false;

        if (GameStateManager.instance.IsGamestate<PlaceOrganState>())
        {
            List<string> lockedOrgans = Blackboard.Read<List<string>>(BlackboardKeys.LOCKED_ORGANS);

            if (lockedOrgans == null)
                lockedOrgans = new List<string>();

            lockedOrgans.Add(organID);
            Blackboard.Write(BlackboardKeys.LOCKED_ORGANS, lockedOrgans);

            GameStateManager.instance.PlayerCompletedTask();
        }
    }

    public override int GetLayerInfo()
    {
        return 0;
    }
}
