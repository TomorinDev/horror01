using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Target
    public Transform targetLocation;

    // Player Is In Range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the target location
            other.transform.position = targetLocation.position;
        }
    }
}
