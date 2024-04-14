using UnityEngine;

public class GoToCaller : MonoBehaviour
{
    public CameraMovementController cameraController; // reference to the CameraMovementController

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cameraController.GoTo("Cube");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            cameraController.GoTo("Capsule");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            cameraController.GoTo("Sphere");
        }
    }
}