using System.Collections;
using UnityEngine;

public class EyeEffectController : MonoBehaviour
{
    [Header("Eye Effects Settings")]
    public GameObject eye;
    private bool isEyeEffectRunning = false; // Flag for coroutine

    [Header("Emission Settings")]
    public Color baseColor;
    public float emissionIntensityMin = 1.0f; // Minimum Emission Intensity
    public float emissionIntensityMax = 3.0f; // Maximum Emission Intensity
    public float emissionIncreaseDuration = 4.0f; // Time duration to increase emission to max

    [Header("Fade Settings")]
    public float fadeDuration = 4.0f; // Duration of fade-in effect

    private Material irisRedMaterial; // Iris_Red
    private Material irisRedLightMaterial; // Iris_RedLight




    private void Start()
    {
        // Get Materials
        GetEyeMaterial();
    }




    // Get Material
    private void GetEyeMaterial()
    {
        if (eye != null)
        {
            Renderer renderer = eye.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Get pecific material for "Iris_RedLight"
                Material[] materials = renderer.materials;
                if (materials.Length > 1)
                {
                    irisRedMaterial = materials[0]; // Iris_Red
                    irisRedLightMaterial = materials[1]; // Iris_RedLight
                }
            }
        }
    }




    // Start Eye Effects
    private IEnumerator StartEyeEffectsCoroutine()
    {
        // Enable the eye effect
        EnableEye();

        // Set the flag to indicate the coroutine is running
        isEyeEffectRunning = true;


        // Fade In
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            float emissionIntensity = Mathf.Lerp(emissionIntensityMin, emissionIntensityMax, elapsedTime / fadeDuration);

            // Set Iris
            SetIrisAlpha(irisRedMaterial, alpha);
            SetIrisEmissionIntensity(emissionIntensity);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }


        // Wait for 5 seconds
        yield return new WaitForSeconds(5);


        // Disable Effect
        if (isEyeEffectRunning)
        {
            // Disable
            DisableEye();
        }

        // Set the flag to indicate the coroutine has finished
        isEyeEffectRunning = false;
    }




    // Enable Eye Effect
    public void EnableEye()
    {
        // Enable
        eye.SetActive(true);
    }


    // Disable Eye Effect
    public void DisableEye()
    {
        // Reset Materials
        SetIrisEmissionIntensity(emissionIntensityMin);
        SetIrisAlpha(irisRedMaterial, 0f);

        // Disable
        eye.SetActive(false);

        // Flag, Eye Effect Coroutine Stop
        isEyeEffectRunning = false;
    }




    // Set Alpha for Fade-In Effect
    private void SetIrisAlpha(Material material, float alpha)
    {
        if (material != null)
        {
            // Set Float Value
            material.SetFloat("_Alpha", alpha);
        }
    }




    // Set Emission Intensity
    private void SetIrisEmissionIntensity(float intensity)
    {
        if (irisRedLightMaterial != null)
        {
            irisRedLightMaterial.SetColor("_EmissionColor", baseColor * intensity);

            // Enable keyword
            irisRedLightMaterial.EnableKeyword("_EMISSION");
        }
    }




    // Method to trigger the coroutine.
    public void TriggerEyeEffect()
    {
        // Check Currently is running coroutine
        if (!isEyeEffectRunning)
        {
            StartCoroutine(StartEyeEffectsCoroutine());
        }
    }
}
