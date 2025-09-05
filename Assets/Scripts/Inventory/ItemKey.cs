using System.Collections;
using UnityEngine;

public class ItemKey : MonoBehaviour, IUsable
{
    public string keyId; // The unique identifier for the key
    private InventoryController playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<InventoryController>();
    }

    public void Use()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Unlockable unlockable = hit.collider.GetComponent<Unlockable>();
            if (unlockable != null && unlockable.isLocked && unlockable.requiredKey == keyId)
            {
                unlockable.Unlock(); // Call the generic unlock method
                Debug.Log($"{unlockable.name} unlocked!");

                if (playerInventory != null)
                {
                    playerInventory.RemoveItem(keyId);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("This key doesn't unlock this object.");
            }
        }
    }
}
