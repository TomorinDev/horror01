using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public static class PostProcessingInitializer
{
    // LD
    public static void InitializeLensDistortion(Volume volume, ref LensDistortion effect, float defaultValue)
    {
        if (volume.profile.TryGet(out LensDistortion ld))
        {
            effect = ld;
            effect.intensity.overrideState = true;
            effect.intensity.Override(defaultValue);
        }
        else
        {
            Debug.LogWarning("Lens Distortion not found in Global Volume");
        }
    }


    // FG
    public static void InitializeFilmGrain(Volume volume, ref FilmGrain effect, float defaultValue)
    {
        if (volume.profile.TryGet(out FilmGrain fg))
        {
            effect = fg;
            effect.intensity.overrideState = true;
            effect.intensity.Override(defaultValue);
        }
        else
        {
            Debug.LogWarning("Film Grain not found in Global Volume");
        }
    }


    // DOF
    public static void InitializeDepthOfField(Volume volume, ref DepthOfField effect, float defaultValue)
    {
        if (volume.profile.TryGet(out DepthOfField dof))
        {
            effect = dof;
            effect.focusDistance.overrideState = true;
            effect.focusDistance.Override(defaultValue);
        }
        else
        {
            Debug.LogWarning("Depth of Field not found in Global Volume");
        }
    }


    // VG
    public static void InitializeVignette(Volume volume, ref Vignette effect, float defaultValue)
    {
        if (volume.profile.TryGet(out Vignette vg))
        {
            effect = vg;
            effect.intensity.overrideState = true;
            effect.intensity.Override(defaultValue);
        }
        else
        {
            Debug.LogWarning("Vignette not found in Global Volume");
        }
    }


    // CA
    public static void InitializeChromaticAberration(Volume volume, ref ChromaticAberration effect, float defaultValue)
    {
        if (volume.profile.TryGet(out ChromaticAberration ca))
        {
            effect = ca;
            effect.intensity.overrideState = true;
            effect.intensity.Override(defaultValue);
        }
        else
        {
            Debug.LogWarning("Chromatic Aberration not found in Global Volume");
        }
    }
}
