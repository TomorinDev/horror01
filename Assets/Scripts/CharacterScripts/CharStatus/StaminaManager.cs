using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    private CharStatusManager statusManager;
    private PlayerMove playerMove;

    private void Start()
    {
        // Get Component
        statusManager = GetComponent<CharStatusManager>();
        playerMove = GetComponent<PlayerMove>();
    }

    // Manage Stamina
    public void ManageStamina()
    {
        // Running Decrease Stamina
        if (Input.GetKey(KeyCode.LeftShift) && statusManager.CanRun() && (playerMove.GetCurrentSpeed() > 0.01f))
        {
            statusManager.DrainStamina(Time.deltaTime);
        }
        else
        {
            // Stop Running Increase Stamina
            if (Input.GetKey(KeyCode.LeftShift) && !statusManager.CanRun())
            {
                helperManager.instance.showHint("Release Left Shift To Restore Stamina");
            }
            statusManager.RecoverStamina(Time.deltaTime);
        }
    }
}

