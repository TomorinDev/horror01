using UnityEngine;

public class InventoryItemPickedUp : MonoBehaviour
{
    public InventoryItem inventoryItem;  // The general inventory item data
    public BatteryCapacity batteryCapacity;  // Reference to the battery's capacity
    public AmmoCapacity ammoCapacity; // Reference to the ammo capacity

    private void Awake()
    {
        // 初始化时获取 BatteryCapacity 和 AmmoCapacity
        if (batteryCapacity == null)
        {
            batteryCapacity = GetComponent<BatteryCapacity>();
        }

        if (ammoCapacity == null)
        {
            ammoCapacity = GetComponent<AmmoCapacity>();
        }
    }

    public BatteryCapacity GetBatteryCapacity() => batteryCapacity;
    public AmmoCapacity GetAmmoCapacity() => ammoCapacity;

}
