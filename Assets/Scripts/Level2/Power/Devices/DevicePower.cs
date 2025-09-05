using System.Collections;
using TMPro;
using UnityEngine;

public class DevicePower : MonoBehaviour
{
    [Header("Device Settings")]
    public bool isOn = false;


    [Header("Audio Clips")]
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;


    private AudioSource audioSource;
    private interactableObj myInteractableObj;




    void Start()
    {
        // Get components
        audioSource = GetComponent<AudioSource>();
        myInteractableObj = GetComponent<interactableObj>();

        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource component on the device.");
        }
    }



    void Update()
    {
        HandleDeviceInteraction();
    }




    // Handle Device Interaction
    private void HandleDeviceInteraction()
    {
        if (myInteractableObj.IsPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isOn)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    // Turn On
    public void TurnOn()
    {
        if (!isOn)
        {
            isOn = true;
            PlayAudio(turnOnSound);
        }
    }

    // Turn Off
    public void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            PlayAudio(turnOffSound);
        }
    }




    // Play Audio
    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }




    // Set Device State
    private void SetDeviceState(bool state)
    {
        // Turn Off Light or something?
        if (TryGetComponent(out Light deviceLight))
        {
            deviceLight.enabled = state;
        }
    }
}
