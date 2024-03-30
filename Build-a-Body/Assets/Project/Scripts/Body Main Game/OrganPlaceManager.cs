using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using ProjectHKU.UI;

public class OrganPlaceManager : SingletonTemplateMono<OrganPlaceManager>
{

    [NonSerialized]
    public Dictionary<string, Organ> organDict = new Dictionary<string, Organ>();

    public HashSet<string> finishedOrgan = new HashSet<string>();

    public bool cameraMoving = false;
    public Vector3 defaultCameraPosition;
    public float cameraShiftVelocity = 1f;

    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene") return;

        CameraShift();
        foreach (var item in organDict)
        {
            if (GameManager.instance.placementFinish)
                item.Value.draggable = false;
            else if (item.Key != GameManager.instance.currentWinMinigame)
                item.Value.draggable = false;
            else
                item.Value.draggable = true;
            
            if (item.Key == GameManager.instance.currentWinMinigame || finishedOrgan.Contains(item.Key))
                item.Value.gameObject.SetActive(true);
            else
                item.Value.gameObject.SetActive(false);
        }
    }

    public void CameraShift()
    {
        Vector3 targetPosition;
        if (GameManager.instance.placementFinish || !organDict.ContainsKey(GameManager.instance.currentWinMinigame))
            targetPosition = defaultCameraPosition;
        else
            targetPosition = organDict[GameManager.instance.currentWinMinigame].targetCameraPosition;

        if ((targetPosition - Camera.main.transform.position).magnitude < cameraShiftVelocity)
        {
            Camera.main.transform.position = targetPosition;
            cameraMoving = false;
        }
        else
        {
            Camera.main.transform.position += cameraShiftVelocity * (targetPosition - Camera.main.transform.position).normalized;
            cameraMoving = true;
        }
    }

    public void FinishPlacement(string organID)
    {
        finishedOrgan.Add(organID);
        GameManager.instance.placementFinish = true;
        UIManager.instance.popup(organID + " Placed");
    }
}
