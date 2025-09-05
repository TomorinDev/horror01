using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    public float healAmount = 25f;  // Amount of health to restore

    private CharStatusManager playerStatusManager;

    // Heal Player
    public void HealPlayer()
    {
        // Get Player CharStatusManager
        playerStatusManager = GameObject.FindWithTag("Player").GetComponent<CharStatusManager>();

        if (playerStatusManager != null)
        {
            playerStatusManager.Heal(healAmount); // Heal the player
            Debug.Log("Player healed by: " + healAmount);
        }
        else
        {
            Debug.LogWarning("No CharStatusManager found on player!");
        }
    }
}
