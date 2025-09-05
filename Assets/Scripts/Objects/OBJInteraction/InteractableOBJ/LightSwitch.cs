using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSwitch : MonoBehaviour
{
    // Public Fields
    public List<Light> lights = new List<Light>(); // List of lights


    [Header("Audio Clips")]
    public AudioClip lightTurnOnSound; // Audio clip for light turn on
    public AudioClip lightTurnOffSound; // Audio clip for light turn off


    // Private Fields
    private bool isLightOn = false;
    private AudioSource audioSource; // AudioSource for playing sounds
    private interactableObj myInteractableObj; // Use interactableObj




    void Start()
    {
        // Get Component
        audioSource = GetComponent<AudioSource>();
        myInteractableObj = GetComponent<interactableObj>();

        // Check if interactableObj is null
        if (myInteractableObj == null)
        {
            Debug.LogError("LightSwitch: myInteractableObj not found on the same GameObject!");
        }

        // Automatically populate lights list
        PopulateLightsList();

        // Validate lights list
        if (lights.Count == 0)
        {
            Debug.LogError("LightSwitch: No lights in the list!");
        }
    }




    void Update()
    {
        ToggleLight();
    }




    // Check if all lights are Children of Switch
    private void OnValidate()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i] != null && !lights[i].transform.IsChildOf(transform))
            {
                Debug.LogError($"LightSwitch: Light at index {i} must be a child of the switch!");
                lights[i] = null;
            }
        }
    }




    // Automatically populate the lights list
    private void PopulateLightsList()
    {
        lights.Clear(); // Clear the list before populating
        Light[] childLights = GetComponentsInChildren<Light>(); // Get all Light components in children

        foreach (Light light in childLights)
        {
            lights.Add(light);
        }
    }




    // Toggle the light state
    private void ToggleLight()
    {
        // Check if the E key is pressed, the player is in range, and the target is selected
        if (Input.GetKeyDown(KeyCode.E) && myInteractableObj.IsPlayerInRange && selectionManager.instance.onTarget)
        {
            // Flip the light state and Apply
            isLightOn = !isLightOn;
            foreach (Light light in lights)
            {
                if (light != null)
                {
                    light.enabled = isLightOn;
                }
            }

            // Play Audio
            lightAudioPlay();
        }
    }




    // Light Audio Play
    private void lightAudioPlay()
    {
        // Play sound based on light state
        if (audioSource != null)
        {
            if (isLightOn && lightTurnOnSound != null)
            {
                audioSource.PlayOneShot(lightTurnOnSound);
            }
            else if (!isLightOn && lightTurnOffSound != null)
            {
                audioSource.PlayOneShot(lightTurnOffSound);
            }
        }
    }
}
