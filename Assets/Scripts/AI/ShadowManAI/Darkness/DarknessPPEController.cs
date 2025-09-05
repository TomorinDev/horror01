using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class DarknessPPEController : MonoBehaviour
{
    [Header("Global Volume Settings")]
    public Volume globalVolume;

    private LensDistortion lensDistortion; // LD 
    private FilmGrain filmGrain; // FG 
    private DepthOfField depthOfField; // DOF 
    private Vignette vignette; // VG

    // LD Value
    private float lensDistortionDefault = -0.15f;
    private float lensDistortionTarget = 0.4f;

    // FG Value
    private float filmGrainDefault = 0.6f;
    private float filmGrainTarget = 1.0f;

    // DOF Value
    private float dofDefaultFocusDistance = 5f;
    private float dofTargetFocusDistance = 1.2f;

    // VG Value
    private float vgDefaultIntensity = 0.3f;
    private float vgTargetIntensity = 0.6f;

    // Coroutine Reference
    private Coroutine ppeCoroutine;




    private void Start()
    {
        // PPE
        InitializePostProcessing();
    }




    // Get PPE
    public void InitializePostProcessing()
    {
        if (globalVolume == null)
        {
            Debug.LogError("Global Volume is not assigned!");
            return;
        }

        // Get Each Effects from Global Volume
        PostProcessingInitializer.InitializeLensDistortion(globalVolume, ref lensDistortion, lensDistortionDefault);
        PostProcessingInitializer.InitializeFilmGrain(globalVolume, ref filmGrain, filmGrainDefault);
        PostProcessingInitializer.InitializeDepthOfField(globalVolume, ref depthOfField, dofDefaultFocusDistance);
        PostProcessingInitializer.InitializeVignette(globalVolume, ref vignette, vgDefaultIntensity);
    }




    // Stop any ongoing effect
    private void StopCoroutine()
    {
        if (ppeCoroutine != null)
        {
            StopCoroutine(ppeCoroutine);
        }
    }




    // Handle Start Visual Effects
    public void StartPPE(float attackStartTime, float attackDuration)
    {
        // Stop any ongoing effect
        StopCoroutine();

        // Start coroutine
        ppeCoroutine = StartCoroutine(StartPPECoroutine(attackStartTime, attackDuration));
    }


    private IEnumerator StartPPECoroutine(float attackStartTime, float attackDuration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < attackDuration)
        {
            elapsedTime = Time.time - attackStartTime;
            float t = elapsedTime / attackDuration;

            // LD
            if (lensDistortion != null)
            {
                lensDistortion.intensity.Override(Mathf.Lerp(lensDistortionDefault, lensDistortionTarget, t));
            }

            // FG
            if (filmGrain != null)
            {
                filmGrain.intensity.Override(Mathf.Lerp(filmGrainDefault, filmGrainTarget, t));
            }

            // DOF
            if (depthOfField != null)
            {
                depthOfField.focusDistance.Override(Mathf.Lerp(dofDefaultFocusDistance, dofTargetFocusDistance, t));
            }

            // VG
            if (vignette != null)
            {
                vignette.intensity.Override(Mathf.Lerp(vgDefaultIntensity, vgTargetIntensity, t));
            }

            yield return null;
        }
    }




    // Reset Post Processing Effects
    public void ResetPPE()
    {
        // Stop any ongoing effect
        StopCoroutine();

        // Start reset coroutine
        ppeCoroutine = StartCoroutine(ResetPPECoroutine());
    }

    // Reset PPE Coroutine
    private IEnumerator ResetPPECoroutine()
    {
        float duration = 3f; // Duration for reset effect
        float elapsed = 0f;

        // Store the current values
        float currentLensDistortion = lensDistortion.intensity.value;
        float currentFilmGrain = filmGrain.intensity.value;
        float currentDepthOfField = depthOfField.focusDistance.value;
        float currentVignette = vignette.intensity.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Handle Reset PPE
            HandleResetPPE(currentLensDistortion, currentFilmGrain, currentDepthOfField, currentVignette, t);

            yield return null;
        }

        // Ensure the final values are set
        HandleResetPPE(currentLensDistortion, currentFilmGrain, currentDepthOfField, currentVignette, 1f);
    }

    // Handle Reset PPE
    private void HandleResetPPE(float currentLensDistortion, float currentFilmGrain, float currentDepthOfField, float currentVignette, float t)
    {
        // LD
        if (lensDistortion != null)
        {
            lensDistortion.intensity.Override(Mathf.Lerp(currentLensDistortion, lensDistortionDefault, t));
        }

        // FG
        if (filmGrain != null)
        {
            filmGrain.intensity.Override(Mathf.Lerp(currentFilmGrain, filmGrainDefault, t));
        }

        // DOF
        if (depthOfField != null)
        {
            depthOfField.focusDistance.Override(Mathf.Lerp(currentDepthOfField, dofDefaultFocusDistance, t));
        }

        // VG
        if (vignette != null)
        {
            vignette.intensity.Override(Mathf.Lerp(currentVignette, vgDefaultIntensity, t));
        }
    }
}