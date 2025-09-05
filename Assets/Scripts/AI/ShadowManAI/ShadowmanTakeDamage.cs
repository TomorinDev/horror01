using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowmanTakeDamage : MonoBehaviour
{
    // Damage Settings
    [Header("Damage Settings")]
    [Tooltip("Damage taken when exposed to light")]
    public float damageFromLight = 20f; // Damage taken when exposed to light


    // Enemy OBJ
    [Header("Enemy Object Reference")]
    public GameObject enemyObject;


    // Shadowman Status
    private ShadowmanStatus shadowmanStatus;

    // Flashlights
    private ItemFlashlight[] FlashlightsList;




    void Start()
    {
        // Get Components
        InitialGetComponents();
    }




    void Update()
    {
        if (enemyObject != null && enemyObject.activeSelf)
        {
            // Check If In Light
            CheckIfInLight();
        }
    }




    // Get Components
    private void InitialGetComponents()
    {
        // Shadow Status
        shadowmanStatus = GetComponent<ShadowmanStatus>();
        if (shadowmanStatus == null)
        {
            Debug.LogError("ShadowmanStatus component not found on the GameObject.");
        }

        // Flashlights
        FlashlightsList = FindObjectsOfType<ItemFlashlight>();
        if(FlashlightsList.Length == 0)
        {
            Debug.LogError("No Flashlight found");
        }
    }




    // Check if the Shadowman is in flashlight
    private void CheckIfInLight()
    {
        foreach (ItemFlashlight flashlight in FlashlightsList)
        {
            // Get Light Component on flashlight
            Light flashlight_Light = flashlight.GetComponentInChildren<Light>();
            if (flashlight.isOn && IsInLightRange(flashlight_Light))
            {
                // Take Damage
                TakeDamage();
                break;
            }
        }
    }




    // Check if Shadowman is in the range of a Light
    private bool IsInLightRange(Light light)
    {
        float distance = Vector3.Distance(transform.position, light.transform.position);
        return distance <= light.range;
    }




    // Take Damage
    private void TakeDamage()
    {
        shadowmanStatus.TakeDamage(damageFromLight);
    }
}