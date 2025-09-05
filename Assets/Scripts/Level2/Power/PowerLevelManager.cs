using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerLevelManager : MonoBehaviour
{
    [Header("General Settings")]
    public List<Light> lights; // Lights
    public List<GameObject> devices; // Devices


    [Header("Power Settings")]
    public float maxPower = 100f; 
    public float currentPower; 
    public float baseDecayRate = 0.2f;
    public float deviceDecayRateMultiplication = 1f;


    [Header("UI Settings")]
    public Slider powerSlider;


    void Start()
    {
        currentPower = maxPower;
        InitializeSlider();
    }




    void Update()
    {
        UpdatePowerLevel();
        UpdateUISlider();
        CheckPowerDepletion();
    }




    // Initailize Slider
    private void InitializeSlider()
    {
        if (powerSlider != null)
        {
            powerSlider.maxValue = maxPower;
            powerSlider.value = currentPower;
        }
    }

    // Update UI Slider
    void UpdateUISlider()
    {
        if (powerSlider != null)
        {
            powerSlider.value = currentPower;
        }
    }



    // Update Power Level
    void UpdatePowerLevel()
    {
        int activeDeviceCount = GetActiveDeviceCount();
        float decayRate = baseDecayRate * (1 + activeDeviceCount * deviceDecayRateMultiplication);
        currentPower -= decayRate * Time.deltaTime;
        currentPower = Mathf.Clamp(currentPower, 0, maxPower);
    }




    // Get Active Device Count
    int GetActiveDeviceCount()
    {
        int count = 0;
        foreach (var device in devices)
        {
            DevicePower devicePower = device.GetComponent<DevicePower>();
            if (devicePower != null && devicePower.isOn)
                count++;
        }
        return count;
    }




    // Turn Off All Lights
    private void TurnOffAllLights()
    {
        foreach (var light in lights)
        {
            // Intensity
            light.intensity = 0.01f;

            // Turn Off
            if (light.enabled)
            {
                // Disable
                light.enabled = false;
            }
        }
    }




    // Check Power Depletion
    void CheckPowerDepletion()
    {
        if (currentPower <= 0)
        {
            // Turn Off All Devices
            foreach (var device in devices)
            {
                device.GetComponent<DevicePower>().TurnOff();
            }

            // Turn Off All Lights
            TurnOffAllLights();
        }
    }
}
