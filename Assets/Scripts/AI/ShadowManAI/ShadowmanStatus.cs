using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowmanStatus : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Maximum health of the Shadowman")]
    public float maxHealth = 100f;

    [Tooltip("Current health of the Shadowman")]
    public float currentHealth;




    void Start()
    {
        // Set Default Health
        currentHealth = maxHealth;
    }




    // Take Damage
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Shadowman takes {damage} damage. Current health: {currentHealth}");

        // Die
        Die();
    }




    // Die
    private void Die()
    {
        // No Health
        if (currentHealth <= 0)
        {
            HandleDie();
        }
    }

    // Handle Die
    private void HandleDie()
    {
        Debug.Log("Shadowman has died.");
        // Implement death logic here (e.g., disable the object, play an animation, etc.)
    }
}