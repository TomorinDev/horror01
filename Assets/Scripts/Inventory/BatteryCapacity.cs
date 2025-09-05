using UnityEngine;

public class BatteryCapacity : MonoBehaviour
{
    public float maxPower = 100f;     // Maximum battery capacity
    public float currentPower = 100f; // Current battery charge

    // Consume a certain amount of power
    public void ConsumePower(float amount)
    {
        currentPower -= amount;
        if (currentPower < 0) currentPower = 0;
    }

    // Check if the battery is depleted
    public bool IsDepleted()
    {
        return currentPower <= 0;
    }

}
