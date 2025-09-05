using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityDetector : MonoBehaviour
{
    private IPlayerIsInRangeTrigger playerTrigger;

    private void Awake()
    {
        playerTrigger = GetComponent<IPlayerIsInRangeTrigger>();
    }

    // Player Is In Range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger?.OnPlayerEnter();
        }
    }

    // Player Is Not In Range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger?.OnPlayerExit();
        }
    }
}
