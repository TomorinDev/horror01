using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Public Fields
    public float mouseSensitivity = 2.0f;

    // Private Fields
    private float verticalRotation = 0.0f;
    private float minRotation = -75.0f;
    private float maxRotation = 75.0f;

    public float getMouseSensitivityY()
    {
        return this.mouseSensitivity;
    }
    public void setMouseSensitivityY(float mouseSensitivityY)
    {
        this.mouseSensitivity = mouseSensitivityY;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code if needed
    }

    // Update is called once per frame
    void Update()
    {
        CameraVerticalRotation(); // Control vertical rotation with the mouse
    }

    // Handles camera vertical rotation using the mouse
    private void CameraVerticalRotation()
    {
        // Get mouse Y-axis movement
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Calculate vertical rotation angle
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minRotation, maxRotation); // Limit rotation

        // Apply rotation
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0.0f);
    }

}
