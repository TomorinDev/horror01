using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeviceUIController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Device Settings")]
    public bool isDevice = false;


    [Header("UI Settings")]
    public TMP_Text deviceStatusText;
    private DevicePower devicePower;


    private interactableObj myInteractableObj;


    void Start()
    {
        myInteractableObj = GetComponent<interactableObj>();

        // Get DevicePower component
        devicePower = GetComponent<DevicePower>();
        if (devicePower != null)
        {
            isDevice = true;
        }

        // Get Text Mesh 
        deviceStatusText = GetComponentInChildren<TMP_Text>();
        if (deviceStatusText == null && isDevice)
        {
            Debug.LogError("Not Text Mesh Found On Device");
        }

        // Set To Empty
        deviceStatusText.text = "";
    }




    // Update is called once per frame
    void Update()
    {
        HandleDeviceStatus();
    }




    // Handle Device Status
    private void HandleDeviceStatus()
    {
        if (deviceStatusText != null && isDevice)
        {
            if (myInteractableObj.IsPlayerInRange && selectionManager.instance.onTarget)
            {
                // On/Off Text & Color
                deviceStatusText.text = devicePower.isOn ? "Device is ON" : "Device is OFF";
                deviceStatusText.color = devicePower.isOn ? Color.green : Color.red;
            }
            else
            {
                deviceStatusText.text = "";
            }
        }
    }
}
