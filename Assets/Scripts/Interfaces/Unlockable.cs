using UnityEngine;

public class Unlockable : MonoBehaviour
{
    [Header("Unlockable Settings")]
    public bool isLocked = true;
    public string requiredKey;

    // Generic unlock method for all unlockable objects
    public virtual void Unlock()
    {
        isLocked = false;
        Debug.Log($"{name} has been unlocked.");
    }
}
