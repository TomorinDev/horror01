using UnityEngine;

public class Target : MonoBehaviour,Damageable
{
    // Public Fields
    public float health = 100f;

    // Taken Damage
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    // Die
    private void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}