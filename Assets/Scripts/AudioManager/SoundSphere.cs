using UnityEngine;
using System;

public class SoundSphere : MonoBehaviour
{
    public float maxRadius = 20f;
    public float expansionSpeed = 5f;
    private float currentRadius = 0f;
    private bool expanding = true;

    public event Action OnDestroyed; // Event to notify when destroyed

    private SphereCollider soundCollider;
    private Rigidbody rb;

    void Start()
    {
        soundCollider = gameObject.AddComponent<SphereCollider>();
        soundCollider.isTrigger = true;
        soundCollider.radius = 0f;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        if (expanding)
        {
            currentRadius += expansionSpeed * Time.deltaTime;
            if (currentRadius >= maxRadius)
            {
                expanding = false;
            }
        }
        else
        {
            currentRadius -= expansionSpeed * Time.deltaTime;
            if (currentRadius <= 0f)
            {
                OnDestroyed?.Invoke(); // Notify listeners before destroying
                Destroy(gameObject);
            }
        }

        soundCollider.radius = currentRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = expanding ? Color.cyan : Color.magenta;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}
