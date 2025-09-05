using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Public Fields
    public GameObject optionsPanel;

    [Header("FPS")]
    public Slider fpsSlider;
    public Text fpsLabel;

    [Header("Mouse Sensitivity")]
    public Slider mouseSensSliderX;
    public Slider mouseSensSliderY;
    public Text mouseSenLabelX;
    public Text mouseSenLabelY;

    // Private Fields
    private bool isOptionsVisible = false;
    public PlayerMove playerMove;
    public CameraController cameraController;



    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        playerMove = GameObject.Find("char1")?.GetComponent<PlayerMove>();
        if (playerMove == null)
        {
            Debug.LogError("PlayerMove component not found.");
            return;
        }

        cameraController = GameObject.Find("Main Camera")?.GetComponent<CameraController>();
        if (cameraController == null)
        {
            Debug.LogError("CameraController component not found.");
            return;
        }

        // Set FPS Limit
        setInitialFPSLimit();
        updateFPSLimit();

        // Set Mouse Sensitivity
        setInitialMouseSensitivity();
        updateMouseSensitvityX();
        updateMouseSensitvityY();

        // Initially hide the options panel
        optionsPanel.SetActive(isOptionsVisible);
    }




    void Update()
    {
        toggleOptionsPanel();
    }






    // Toggle Options Panel
    private void toggleOptionsPanel()
    {
        // Toggle options panel visibility
        if (Input.GetKeyDown(KeyCode.P))
        {
            isOptionsVisible = !isOptionsVisible;
            optionsPanel.SetActive(isOptionsVisible);

            // Toggle Lock Cursor
            toggleLockCursor();
        }
    }




    // Toggle Lock Cursor
    private void toggleLockCursor()
    {
        if (isOptionsVisible)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    // Lock Cursor
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Unlock Cursor
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // Set Initalial Mouse Sensitivity
    private void setInitialMouseSensitivity()
    {
        // Set initial slider and label values
        mouseSensSliderX.value = playerMove.getMouseSensitivityX(); // X
        mouseSenLabelX.text = $"Mouse X: {mouseSensSliderX.value}"; // X

        mouseSensSliderY.value = cameraController.getMouseSensitivityY(); // Y
        mouseSenLabelY.text = $"Mouse Y: {mouseSensSliderY.value}"; // Y
    }

    // Set Mouse Sensitivity X
    private void setMouseSensitvityX(float mouseSensitvityX)
    {
        playerMove.setMouseSensitivityX(Mathf.RoundToInt(mouseSensitvityX));
        mouseSenLabelX.text = $"Mouse X: {Mathf.RoundToInt(mouseSensitvityX)}";
    }

    // Set Mouse Sensitivity Y
    private void setMouseSensitvityY(float mouseSensitvityY)
    {
        cameraController.setMouseSensitivityY(Mathf.RoundToInt(mouseSensitvityY));
        mouseSenLabelY.text = $"Mouse Y: {Mathf.RoundToInt(mouseSensitvityY)}";
    }

    // Update Mouse Sensitivity X
    private void updateMouseSensitvityX()
    {
        // Update Mouse Sensitivity X when slider changes
        mouseSensSliderX.onValueChanged.AddListener(setMouseSensitvityX);
    }

    // Update Mouse Sensitivity Y
    private void updateMouseSensitvityY()
    {
        // Update Mouse Sensitivity Y when slider changes
        mouseSensSliderY.onValueChanged.AddListener(setMouseSensitvityY);
    }



    // Set Initial FPS Limit
    private void setInitialFPSLimit()
    {
        // Set initial slider and label values
        fpsSlider.value = 100;
        fpsLabel.text = $"FPS Limit: {fpsSlider.value}";
    }

    // Set FPS Limit
    private void setFPSLimit(float fps)
    {
        Application.targetFrameRate = Mathf.RoundToInt(fps);
        fpsLabel.text = $"FPS Limit: {Mathf.RoundToInt(fps)}";
    }

    // Update FPS Limit
    private void updateFPSLimit()
    {
        // Update FPS when slider changes
        fpsSlider.onValueChanged.AddListener(setFPSLimit);
    }
}
