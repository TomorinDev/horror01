
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    // Rotation Speed
    public float rotationSpeed = 100f;

    void Update()
    {
        // Y axis Rotation
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}