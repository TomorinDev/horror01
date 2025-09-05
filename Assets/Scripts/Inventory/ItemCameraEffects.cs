using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCameraEffects : MonoBehaviour
{
    [Header("Camera Flash Settings")]
    private Light cameraLight;
    public float flashIntensity = 5f;
    public float flashOriginalIntensity = 0.1f;
    public float flashDuration = 0.1f;
    public Color flashColor = Color.white;


    [Header("Sound Settings")]
    private AudioSource audioSource;
    public AudioClip attackSound;

    private Color originalColor;
    private bool isFlashing = false;




    void Start()
    {
        // Light
        cameraLight = GetComponentInChildren<Light>();
        if (cameraLight == null)
        {
            Debug.LogError("Light component not found in children!");
        }

        // Audio Source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }

        if (cameraLight != null)
        {
            originalColor = cameraLight.color;
        }
    }




    void Update()
    {

    }

    


    // Camera Action
    public void CameraAction()
    {
        if (Input.GetButtonDown("Fire1") && !isFlashing)
        {
            StartCoroutine(CameraFlashEffect());

            // Play Sound
            PlayCameraSound();
        }
    }

    // Camera Flash Effect
    private IEnumerator CameraFlashEffect()
    {
        if (cameraLight != null)
        {
            isFlashing = true;

            // Change Camera Light Color
            cameraLight.color = flashColor;
            cameraLight.intensity = flashIntensity; // Increase Intensity
            yield return new WaitForSeconds(flashDuration);

            // Reset
            cameraLight.color = originalColor;
            cameraLight.intensity = flashOriginalIntensity;
            isFlashing = false;
        }
    }


    // Play Camera Sound
    private void PlayCameraSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}
