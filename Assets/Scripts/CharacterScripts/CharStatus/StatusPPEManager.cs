using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class StatusPPEManager : MonoBehaviour
{
    [Header("Global Volume Settings")]
    public Volume globalVolume;


    private LensDistortion lensDistortion; // LD 
    private FilmGrain filmGrain; // FG 
    private DepthOfField depthOfField; // DOF 
    private Vignette vignette; // VG
    private ChromaticAberration chromaticAberration; // CA


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
    private float vgTargetIntensity = 0.45f;

    // CA Value
    private float caDefaultIntensity = 0.35f;
    private float caTargetIntensity = 1.0f;


    private float adjustmentSpeed = 1f;




    void Start()
    {
        GetPPE();
    }




    // Get Post Process Effects
    private void GetPPE()
    {
        // Get Each Effects from Global Volume
        PostProcessingInitializer.InitializeLensDistortion(globalVolume, ref lensDistortion, lensDistortionDefault);
        PostProcessingInitializer.InitializeFilmGrain(globalVolume, ref filmGrain, filmGrainDefault);
        PostProcessingInitializer.InitializeDepthOfField(globalVolume, ref depthOfField, dofDefaultFocusDistance);
        PostProcessingInitializer.InitializeVignette(globalVolume, ref vignette, vgDefaultIntensity);
        PostProcessingInitializer.InitializeChromaticAberration(globalVolume, ref chromaticAberration, caDefaultIntensity);
    }

    // Update Post Processing Effects
    public void UpdatePPE(float currentStamina, float maxStamina, float currentHealth, float maxHealth, float currentSanity, float maxSanity)
    {
        LDFGupdateBasedOnStamina(currentStamina, maxStamina);
        VGupdateBasedOnHealth(currentHealth, maxHealth);
        CAupdateBasedOnSanity(currentSanity, maxSanity);
    }

    // Smooth Transition
    private float smoothTransition(float value, float target)
    {
        return Mathf.MoveTowards(value, target, adjustmentSpeed * Time.deltaTime);
    }



    // LD, FG Update
    private void LDFGupdateBasedOnStamina(float currentStamina, float maxStamina)
    {
        if (lensDistortion != null && filmGrain != null)
        {
            float staminaFactor = Mathf.Clamp01((maxStamina - currentStamina) / maxStamina); // Stamina

            float targetLensDistortion = Mathf.Lerp(lensDistortionDefault, lensDistortionTarget, staminaFactor); // LD
            float targetFilmGrain = Mathf.Lerp(filmGrainDefault, filmGrainTarget, staminaFactor); // FG
            float targetFocusDistance = Mathf.Lerp(dofDefaultFocusDistance, dofTargetFocusDistance, staminaFactor); // DOF

            // Transition
            lensDistortion.intensity.Override(smoothTransition(lensDistortion.intensity.value, targetLensDistortion)); // LD
            filmGrain.intensity.Override(smoothTransition(filmGrain.intensity.value, targetFilmGrain)); // FG
            depthOfField.focusDistance.Override(smoothTransition(depthOfField.focusDistance.value, targetFocusDistance)); // DOF
        }
    }




    // VG Update
    private void VGupdateBasedOnHealth(float currentHealth, float maxHealth)
    {
        if (vignette != null)
        {
            float healthFactor = Mathf.Clamp01((maxHealth - currentHealth) / maxHealth); // Health

            float targetVignette = Mathf.Lerp(vgDefaultIntensity, vgTargetIntensity, healthFactor); // VG

            // Transition
            vignette.intensity.Override(smoothTransition(vignette.intensity.value, targetVignette)); // VG
        }
    }




    // CA Update
    private void CAupdateBasedOnSanity(float currentSanity, float maxSanity)
    {
        if (chromaticAberration != null)
        {
            float sanityFactor = Mathf.Clamp01((maxSanity - currentSanity) / maxSanity); // Sanity

            float targetChromaticAberration = Mathf.Lerp(caDefaultIntensity, caTargetIntensity, sanityFactor); // CA

            // Transition
            chromaticAberration.intensity.Override(smoothTransition(chromaticAberration.intensity.value, targetChromaticAberration)); // CA
        }
    }
}
