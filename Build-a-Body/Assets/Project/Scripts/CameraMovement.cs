using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //old camera movement code from the OrganPlaceManager

    /*    public void CameraShift()
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
        }*/


}
