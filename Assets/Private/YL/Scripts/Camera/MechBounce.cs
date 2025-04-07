using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public CameraMotionController cameraController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) cameraController.StartBounce();
        if (Input.GetKeyDown(KeyCode.F)) cameraController.StartFall();
        if (Input.GetKeyDown(KeyCode.Z)) cameraController.StartZoomInOut();
        if (Input.GetKeyDown(KeyCode.X)) cameraController.StartZoomIn();
        if (Input.GetKeyDown(KeyCode.T)) cameraController.StartTilt();
    }
}
