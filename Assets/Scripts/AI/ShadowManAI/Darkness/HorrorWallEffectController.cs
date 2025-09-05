using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorWallEffectController : MonoBehaviour
{
    [Header("Horror Wall Settings")]
    public Material horrorWallMaterial;

    // Initial Value
    public float horrorInitialThresholdValue = 0.0f; // 0
    public float normalInitialThresholdValue = 0.4f; // 0.4

    // Target Values
    private float horrorThresholdTarget = 6.0f;
    private float normalThresholdTarget = 0.0f;

    // Transition Duration
    private float effectStartTransitionDuration = 2.0f;
    private float effectEndTransitionDuration = 10.0f;

    // Current Values
    private float currentHorrorThresholdValue;
    private float currentNormalThresholdValue;




    // Coroutine References
    private Coroutine horrorThresholdCoroutine;
    private Coroutine normalThresholdCoroutine;




    // Start is called before the first frame update
    private void Start()
    {
        // Get Materials
        GetHorrorWallMaterials();
    }




    // Get Horror Wall Material
    private void GetHorrorWallMaterials()
    {
        if (horrorWallMaterial != null)
        {
            currentHorrorThresholdValue = horrorInitialThresholdValue;
            currentNormalThresholdValue = normalInitialThresholdValue;

            // Update Current Material
            UpdateCurrentMaterial();

            // Update Material
            UpdateMaterial();
        }
        else
        {
            Debug.LogError("Horror Wall Material is not assigned!");
        }
    }




    // Update Shared Material
    private void UpdateCurrentMaterial()
    {
        horrorWallMaterial.SetFloat("_HorrorThreshold", currentHorrorThresholdValue);
        horrorWallMaterial.SetFloat("_NormalThreshold", currentNormalThresholdValue);
    }




    // Update Material
    private void UpdateMaterial()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        if(renderers.Length != 0)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer.material.name == horrorWallMaterial.name)
                {
                    Debug.Log("Set Material");
                    renderer.material = horrorWallMaterial;
                }
            }
        }
        else
        {
            Debug.Log("No Material Found");
        }
    }




    // Enable Horror Wall Effect
    public void EnableHorrorWall()
    {
        // Stop any ongoing effect
        StopCoroutine();

        float horrorThresholdStartValue = horrorWallMaterial.GetFloat("_HorrorThreshold");
        float normalThresholdStartValue = horrorWallMaterial.GetFloat("_NormalThreshold");

        horrorThresholdCoroutine = StartCoroutine(SmoothTransitionHorrorWall("_HorrorThreshold", horrorThresholdStartValue, horrorThresholdTarget, effectStartTransitionDuration)); // 0 to 5
        normalThresholdCoroutine = StartCoroutine(SmoothTransitionHorrorWall("_NormalThreshold", normalThresholdStartValue, normalThresholdTarget, effectStartTransitionDuration)); // 0.4 to 0
    }


    // Disable Horror Wall Effect
    public void DisableHorrorWall()
    {
        // Stop any ongoing effect
        StopCoroutine();

        float horrorThresholdStartValue = horrorWallMaterial.GetFloat("_HorrorThreshold");
        float normalThresholdStartValue = horrorWallMaterial.GetFloat("_NormalThreshold");

        horrorThresholdCoroutine = StartCoroutine(SmoothTransitionHorrorWall("_HorrorThreshold", horrorThresholdStartValue, horrorInitialThresholdValue, effectEndTransitionDuration)); // 5 to 0
        normalThresholdCoroutine = StartCoroutine(SmoothTransitionHorrorWall("_NormalThreshold", normalThresholdStartValue, normalInitialThresholdValue, effectEndTransitionDuration)); // 0 to 0.4
    }


    // Stop any ongoing effect
    private void StopCoroutine()
    {
        if (horrorThresholdCoroutine != null)
        {
            StopCoroutine(horrorThresholdCoroutine);
        }
        if (normalThresholdCoroutine != null)
        {
            StopCoroutine(normalThresholdCoroutine);
        }
    }


    // Smooth Transition
    private IEnumerator SmoothTransitionHorrorWall(string propertyName, float startValue, float targetValue, float duration)
    {
        // Elapsed Time
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);

            // Material
            horrorWallMaterial.SetFloat(propertyName, currentValue);

            // Set Current Values
            if (propertyName == "_HorrorThreshold")
            {
                currentHorrorThresholdValue = currentValue;
            }
            else if (propertyName == "_NormalThreshold")
            {
                currentNormalThresholdValue = currentValue;
            }

            // Next Frame
            yield return null;
        }

        horrorWallMaterial.SetFloat(propertyName, targetValue);

        // Set Current Values
        if (propertyName == "_HorrorThreshold")
        {
            currentHorrorThresholdValue = targetValue;
        }
        else if (propertyName == "_NormalThreshold")
        {
            currentNormalThresholdValue = targetValue;
        }
    }
}
