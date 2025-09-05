using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGeneratorRadiation : MonoBehaviour
{
    public float damagePerSecond = 5f; // Health Drain

    private bool isPlayerInside = false;
    private CharStatusManager playerStatusManager;

    // On Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // If Play Is In Range
        if (other.CompareTag("Player"))
        {
            // Get the player's CharStatusManager
            playerStatusManager = other.GetComponent<CharStatusManager>();
            if (playerStatusManager != null)
            {
                isPlayerInside = true;
            }
        }
    }

    // On Trigger Exit
    private void OnTriggerExit(Collider other)
    {
        // If Play Is Not In Range
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerStatusManager = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Drain Health
        if (isPlayerInside && playerStatusManager != null)
        {
            playerStatusManager.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
