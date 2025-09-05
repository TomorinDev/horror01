using UnityEngine;

public class ItemCameraManager : MonoBehaviour
{
    public GameObject cameraScreen; // Reference to the CameraScreen UI

    void Start()
    {
        if (cameraScreen != null)
        {
            cameraScreen.SetActive(false); // Initially hide the camera screen
        }
    }


    // Show Camera Screen
    public void ShowCameraScreen()
    {
        if (cameraScreen != null)
        {
            cameraScreen.SetActive(true);
        }
    }


    // Hide Camera Screen
    public void HideCameraScreen()
    {
        if (cameraScreen != null)
        {
            cameraScreen.SetActive(false);
        }
    }
}
